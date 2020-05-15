/**** 
 * Desc: fetch the data from excel file
 * Author: Jack
 *****************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;


namespace JR.NewTenancy.Models.Common
{
    public static class ExcelConvert
    {
        /// <summary>
        ///  Get Data from excel file
        /// </summary>
        /// <param name="stream">upload file stream</param>
        /// <param name="fieldsMapping">Field Mapping keys</param>
        /// <param name="batchId">Execute Batch Id</param>
        /// <param name="s_fileName">Upload File Name</param>
        /// <param name="curDate">Execute Date</param>
        /// <returns></returns>
        public static System.Data.DataTable ReadUploadExcel(Stream stream, int startRowNum, List<MappingKey> fields)
        {
            System.Data.DataTable dtSource = new System.Data.DataTable();
            try
            {
                #region Read Data
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, false))
                {
                    // Get Workbook Part of Spread Sheet Document
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    IEnumerable<Sheet> sheets = workbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                    string firstSheetId = sheets.First().Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(firstSheetId);
                    Worksheet workSheet = worksheetPart.Worksheet;

                    // Get Data in Excel file
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rowcollection = sheetData.Descendants<Row>();
                    List<Row> rows = rowcollection.ToList();
                    int eRowsCount = rows.Count();

                    List<ExcelField> excelFields = getExcelFields(doc, rows[startRowNum - 2]);

                    List<ExcelField> sInsertFields = new List<ExcelField>();
                    #region DataTable 结构定义
                    foreach (ExcelField e_field in excelFields)
                    {
                        MappingKey mk = fields.Where(fm => (fm.Key.ToUpper() == e_field.CellField.ToUpper() && fm.DBField != null)).FirstOrDefault();
                        if (mk != null)
                        {
                            DataColumn dc = new DataColumn(mk.DBField);
                            dtSource.Columns.Add(dc);
                            e_field.DBField = mk.DBField;
                            e_field.fType = mk.fType;
                            sInsertFields.Add(e_field);
                        }
                    }
                    #endregion

                    // Add rows into DataTable
                    for (int i = startRowNum - 1; i < eRowsCount; i++)
                    {
                        DataRow dRow = dtSource.NewRow();
                        IEnumerable<Cell> row = rows[i].Descendants<Cell>();
                        foreach (Cell cell in row)
                        {
                            string sValue = GetValueOfCell(doc, cell);
                            StringValue sv = cell.CellReference;
                            if (sv != null)
                            {
                                string sCellReference = RemoveNumFromCellRef(sv.ToString(), (i + 1).ToString().Length);
                                ExcelField eField = sInsertFields.Where(sIF => sIF.CellReference == sCellReference && sCellReference != "").FirstOrDefault();

                                if (eField != null)
                                {
                                    if (eField.fType == FieldDataType.Date || eField.fType == FieldDataType.DateAndString)
                                    {
                                        dRow[eField.DBField] = ConvertToDate(sValue);
                                    }
                                    else if (eField.fType == FieldDataType.Time)
                                    {
                                        dRow[eField.DBField] = ConvertToTime(sValue);
                                    }
                                    
                                    else if(eField.fType == FieldDataType.Float)
                                    {
                                        dRow[eField.DBField] = ConvertToFloat(sValue);
                                    }
                                    else
                                    {
                                        dRow[eField.DBField] = sValue;
                                    }
                                }

                            }
                        }

                        //The current For loop will be terminated if the product code is empty.
                        MappingKey pCodeKey = fields.Where(mk => mk.DBField == "ContactCode").FirstOrDefault();
                        if (pCodeKey == null) continue;
                        if (dRow[pCodeKey.DBField] == null || string.IsNullOrWhiteSpace(dRow[pCodeKey.DBField].ToString())) continue;
                        // Add other field value

                        dtSource.Rows.Add(dRow);
                    }
                }

                #endregion
            }
            catch (System.Exception ex)
            {
                //DTCLog.WriteLog("ReadMMXExcel :" + ex.Message);
            }
            return dtSource;
        }
        public static void ExtendColumns(System.Data.DataTable dt, int batchId,string WriteName)
        {
            try
            {
                dt.SetColumnDefaultValue("BatchId", batchId);
                dt.SetColumnDefaultValue("WriteName", WriteName); 
                //dt.SetColumnDefaultValue("FileName", fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public static void ExtendColumns(System.Data.DataTable dt, TemplateType temp, int batchId, string fileName, SessionUser user)
        //{
        //    try
        //    {
        //        DateTime curDate = DateTime.Now;
        //        //扩展DB列
        //        if (temp == TemplateType.MMX)
        //        {
        //            //AddExtendedMMXColumns(dt);
        //            dt.SetColumnDefaultValue("Batch_ID", batchId);
        //            dt.SetColumnDefaultValue("Load_Date", curDate);
        //        }
        //        else
        //        {
        //            //AddExtendedOtherColumns(dt);
        //            if (temp == TemplateType.MLPMMX)
        //            {
        //                dt.SetColumnDefaultValue(Util.Col_Name_MMX_ProductInfoOwner, user.UserName);
        //            }
        //            dt.SetColumnDefaultValue("BatchID", batchId);
        //            dt.SetColumnDefaultValue("LoadDate", curDate);
        //            dt.SetColumnDefaultValue("SheetName", temp.ToString());
        //        }
        //        dt.SetColumnDefaultValue("FlatFileName", fileName);
        //        dt.SetColumnDefaultValue("LoadUser", user.UserId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        /// <summary>
        ///  Read Data from selected excel file into DataTable
        /// </summary>
        /// <param name="filename">Excel File Path</param>
        /// <returns></returns>
        public static System.Data.DataTable ReadExcelFromStream(Stream stream, List<MappingKey> mKeys, string s_table_name, int tIndex)
        {
            // Initialize an instance of DataTable
            System.Data.DataTable dt = new System.Data.DataTable(s_table_name);
            try
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(stream, false))
                {
                    // Get Workbook Part of Spread Sheet Document
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    IEnumerable<Sheet> sheets = workbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                    string firstSheetId = sheets.First().Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(firstSheetId);
                    Worksheet workSheet = worksheetPart.Worksheet;
                    //WorksheetPart wsPart = workbookPart.WorksheetParts.FirstOrDefault();

                    // Get Data in Excel file
                    //SheetData sheetData = wsPart.Worksheet.GetFirstChild<SheetData>();
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rowcollection = sheetData.Descendants<Row>();
                    List<Row> rows = rowcollection.ToList();
                    UInt32Value eRowsCount = (uint)rows.Count();

                    if (eRowsCount <= tIndex + 1)
                    {
                        return dt;
                    }
                    else
                    {
                        Row _eRow = rows[tIndex];
                        foreach (Cell cell in _eRow.Descendants<Cell>())
                        {
                            // Get Cell Column Index                        
                            string sCellName = GetValueOfCell(spreadsheetDocument, cell);
                            if (!string.IsNullOrEmpty(sCellName))
                            {
                                dt.Columns.Add(sCellName);
                                MappingKey mKey = mKeys.Where(key => key.Key.ToUpper() == sCellName.ToUpper()).FirstOrDefault();
                                if (mKey != null) mKey.CallReference = GetCellReference(cell.CellReference);
                            }
                            else { break; }
                        }
                    }

                    int colsCount = dt.Columns.Count;
                    // Add rows into DataTable
                    for (int i = tIndex + 1; i < eRowsCount; i++)
                    {
                        DataRow temprow = dt.NewRow();
                        foreach (Cell cell in rows[i].Descendants<Cell>())
                        {

                            string sValue = GetValueOfCell(spreadsheetDocument, cell);
                            StringValue sv = cell.CellReference;
                            if (sv != null)
                            {
                                string sCellReference = RemoveNumFromCellRef(sv.ToString(), (i + 1).ToString().Length);
                                MappingKey mKey = mKeys.Where(k => k.CallReference == sCellReference && sCellReference != "").FirstOrDefault();
                                if (mKey != null)
                                {
                                    if (mKey.fType == FieldDataType.Date)
                                    {
                                        temprow[mKey.Key] = ConvertToDate(sValue);
                                    }
                                    else if (mKey.fType == FieldDataType.Time)
                                    {
                                        temprow[mKey.Key] = ConvertToTime(sValue);
                                    }
                                    else
                                    {
                                        temprow[mKey.Key] = sValue;
                                    }
                                }
                            }
                        }

                        //Add the row to DataTable
                        dt.Rows.Add(temprow);
                    }
                }
                return dt;
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message);
            }
        }

        private static string RemoveNumFromCellRef(string sCelRef, int nLen)
        {
            if (string.IsNullOrWhiteSpace(sCelRef)) return "";
            return sCelRef.Substring(0, sCelRef.Length - nLen);
        }
        public static void AddExtendedMMXColumns(System.Data.DataTable dtSource)
        {
            dtSource.Columns.Add("FlatFileName");
            dtSource.Columns.Add("LoadUser");
            dtSource.Columns.Add("Batch_ID", typeof(uint));
            dtSource.Columns.Add("Load_Date", typeof(DateTime));
        }

        public static void AddExtendedOtherColumns(System.Data.DataTable dtSource)
        {
            dtSource.Columns.Add("FlatFileName");
            dtSource.Columns.Add("LoadUser");
            dtSource.Columns.Add("SheetName"); //用SheetName标记文件类型(MMX/OPS.....Allocation)
            dtSource.Columns.Add("BatchID", typeof(uint));
            dtSource.Columns.Add("LoadDate", typeof(DateTime));
        }
        public static void SetColumnDefaultValue(this System.Data.DataTable dt, string colName, object colVal)
        {
            DataColumn dc = new DataColumn(colName);
            dc.DefaultValue = colVal;
            dt.Columns.Add(dc);
        }

        /// <summary>
        ///  Read Data from selected excel file into DataTable
        /// </summary>
        /// <param name="filename">Excel File Path</param>
        /// <returns></returns>
        public static System.Data.DataTable ReadAllocationFeedbackExcelFromStream(Stream stream, string s_table_name, int tIndex)
        {
            // Initialize an instance of DataTable
            System.Data.DataTable dt = new System.Data.DataTable(s_table_name);
            try
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(stream, false))
                {
                    // Get Workbook Part of Spread Sheet Document
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    WorksheetPart wsPart = workbookPart.WorksheetParts.FirstOrDefault();

                    // Get Data in Excel file
                    SheetData sheetData = wsPart.Worksheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rowcollection = sheetData.Descendants<Row>();
                    List<Row> rows = rowcollection.ToList();
                    UInt32Value eRowsCount = (uint)rows.Count();

                    if (eRowsCount <= tIndex + 1)
                    {
                        return dt;
                    }
                    else
                    {
                        Row _eRow = rows[tIndex];
                        bool iCnComExist = false;
                        foreach (Cell cell in _eRow.Descendants<Cell>())
                        {
                            // Get Cell Column Index                        
                            string sCellName = GetValueOfCell(spreadsheetDocument, cell);
                            if (!string.IsNullOrEmpty(sCellName))
                            {
                                if (sCellName.Equals("CN.COM") == false)
                                {
                                    dt.Columns.Add(sCellName);
                                }
                                else
                                {
                                    if (!iCnComExist)
                                    {
                                        dt.Columns.Add(sCellName);
                                        iCnComExist = true;
                                    }
                                    else
                                    {
                                        dt.Columns.Add(sCellName.Replace(".", "_"));
                                    }
                                }
                            }
                            else { break; }
                        }
                    }
                    int colsCount = dt.Columns.Count;
                    // Add rows into DataTable
                    for (int i = tIndex + 1; i < eRowsCount; i++)
                    {

                        DataRow temprow = dt.NewRow();
                        // temprow["StoreNumber"] = storenumber;
                        int columnIndex = 0;
                        foreach (Cell cell in rows[i].Descendants<Cell>())
                        {
                            temprow[columnIndex] = GetValueOfCell(spreadsheetDocument, cell);
                            columnIndex++;
                            if (columnIndex > colsCount) continue;
                        }
                        // Add the row to DataTable
                        dt.Rows.Add(temprow);
                    }
                }
                return dt;
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message);
            }
        }

        /// <summary>
        /// Get Column Name From given cell name
        /// </summary>
        /// <param name="cellReference">Cell Name(For example,A1)</param>
        /// <returns>Column Name(For example, A)</returns>
        public static string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name of cell
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            return match.Value;
        }

        /// <summary>
        ///  Get Value of Cell 
        /// </summary>
        /// <param name="spreadsheetdocument">SpreadSheet Document Object</param>
        /// <param name="cell">Cell Object</param>
        /// <returns>The Value in Cell</returns>
        private static string GetValueOfCell(SpreadsheetDocument spreadsheetdocument, Cell cell)
        {
            #region Change before 2015.4.16
            /*Change before 2015.4.16
            // Get value in Cell
            SharedStringTablePart sharedString = spreadsheetdocument.WorkbookPart.SharedStringTablePart;
            if (cell.InnerText == null)
            {
                return string.Empty;
            }
            string cellValue = cell.InnerText;
            // The condition that the Cell DataType is SharedString
                                 
            if (cell.DataType != null)
            {
                switch (cell.DataType.Value)
                {
                    case CellValues.SharedString:
                        // For shared strings, look up the value in the
                        // shared strings table.
                        cellValue = sharedString.SharedStringTable.ChildElements[int.Parse(cellValue)].InnerText;
                        break;
                    case CellValues.Boolean:
                        switch (cellValue)
                        {
                            case "0":
                                cellValue = "FALSE";
                                break;
                            default:
                                cellValue = "TRUE";
                                break;
                        }
                        break;
                }
            }
           
            //below sentence will process datatime cell           
            //if (cell.StyleIndex!=null&&cell.DataType==null) 
            //{
            //    cellValue = DateTime.FromOADate(Convert.ToDouble(cellValue)).ToShortDateString();
            //}

            return cellValue;
             */
            #endregion

            #region Hason Change for EmpDisStartDate field
            //Hason Change 2015.4.16
            WorkbookPart wp = spreadsheetdocument.WorkbookPart;
            SharedStringTablePart sharedString = wp.SharedStringTablePart;

            string strCellValue = string.Empty;

            if (null != cell)
            {
                CellValue cv = cell.CellValue;
                if (null != cv)
                {
                    strCellValue = cv.Text;

                    if (null != cell.DataType)
                    {
                        if (cell.DataType.Value == CellValues.SharedString)
                        {
                            strCellValue = sharedString.SharedStringTable.ChildElements[int.Parse(strCellValue)].InnerText;
                        }
                    }
                }
                //Hason 2015.4.20 Because dowload template can't get cellvalue, just can get the inner text.
                else
                {
                    strCellValue = cell.InnerText;
                    if (null != cell.DataType)
                    {
                        if (cell.DataType.Value == CellValues.SharedString)
                        {
                            strCellValue = sharedString.SharedStringTable.ChildElements[int.Parse(strCellValue)].InnerText;
                        }
                    }
                }
            }

            return strCellValue;
            //
            #endregion
        }
        private static string ConvertToFloat(string CellValue)
        {
            string sValue = CellValue;
            try
            {
                if (!string.IsNullOrWhiteSpace(CellValue))
                {

                    float dValue = 0;
                    float.TryParse(CellValue, out dValue);
                    sValue = dValue.ToString();
                }
            }
            catch
            {
                sValue = CellValue;
            }
            return sValue;
        }
        private static string ConvertToDate(string sDateValue)
        {
            string sValue = sDateValue;
            try
            {
                if (!string.IsNullOrWhiteSpace(sDateValue))
                {

                    double dDateValue = 0;
                    double.TryParse(sDateValue, out dDateValue);
                    if (dDateValue > 0d)
                    {
                        //sValue = DateTime.FromOADate(dDateValue).ToShortDateString();
                        sValue = DateTime.FromOADate(dDateValue).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        return sValue;
                    }

                }

            }
            catch
            {
                sValue = sDateValue;
            }
            return sValue;
        }
        private static string ConvertToTime(string sTime)
        {
            string sValue = sTime;
            try
            {
                if (!string.IsNullOrWhiteSpace(sTime))
                {

                    double dDateValue = 0;
                    double.TryParse(sTime, out dDateValue);
                    if (dDateValue > 0d)
                    {
                        sValue = DateTime.FromOADate(dDateValue).ToString("hh:mm t\\M");
                        if (sValue.StartsWith("0"))
                        {
                            sValue = sValue.Substring(1);
                        }
                    }

                }

            }
            catch
            {
                sValue = sTime;
            }
            return sValue;
        }
        /// <summary>
        ///  Get Cell Value from Excel
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="wsPart"></param>
        /// <param name="sCellReference"></param>
        /// <returns></returns>
        private static string GetValueOfCell(SpreadsheetDocument doc, WorksheetPart wsPart, string sCellReference)
        {
            string value = "";
            Cell theCell = wsPart.Worksheet.Descendants<Cell>().Where(c => c.CellReference == sCellReference).FirstOrDefault();

            // If the cell does not exist, return an empty string.
            if (theCell != null)
            {
                value = theCell.InnerText;

                // If the cell represents an integer number, you are done. 
                // For dates, this code returns the serialized value that 
                // represents the date. The code handles strings and 
                // Booleans individually. For shared strings, the code 
                // looks up the corresponding value in the shared string 
                // table. For Booleans, the code converts the value into 
                // the words TRUE or FALSE.
                if (theCell.DataType != null)
                {
                    switch (theCell.DataType.Value)
                    {
                        case CellValues.SharedString:
                            // For shared strings, look up the value in the
                            // shared strings table.
                            var stringTable = doc.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

                            // If the shared string table is missing, something 
                            // is wrong. Return the index that is in
                            // the cell. Otherwise, look up the correct text in 
                            // the table.
                            if (stringTable != null)
                            {
                                value =
                                    stringTable.SharedStringTable
                                    .ElementAt(int.Parse(value)).InnerText;
                            }
                            break;
                        case CellValues.Boolean:
                            switch (value)
                            {
                                case "0":
                                    value = "FALSE";
                                    break;
                                default:
                                    value = "TRUE";
                                    break;
                            }
                            break;
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// Excel Fields
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static List<ExcelField> getExcelFields(SpreadsheetDocument doc, Row row)
        {
            List<ExcelField> fields = new List<ExcelField>();
            int cIndex = 0;
            IEnumerable<Cell> cells = row.Descendants<Cell>();
            foreach (Cell cell in cells)
            {
                fields.Add(new ExcelField() { Index = cIndex, CellField = GetValueOfCell(doc, cell), CellReference = GetCellReference(cell.CellReference) });
                cIndex++;
            }
            return fields;
        }

        /// <summary>
        ///  Get Cell Reference Name
        /// </summary>
        /// <param name="sCellReference"></param>
        /// <returns></returns>
        private static string GetCellReference(string sCellReference)
        {
            if (string.IsNullOrEmpty(sCellReference)) { return string.Empty; }
            return sCellReference.Substring(0, sCellReference.Length - 1);
        }


    }
}
