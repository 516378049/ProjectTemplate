/**** 
 * Desc: covert object data into excel file
 * Author: Jack
 *****************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;


namespace JR.NewTenancy.Models.Common
{
    public abstract class OpenXMLHelpers
    {
        public const UInt32 DateFormateIndex = 299U;
        public const UInt32 PercentIndex = 300U;
        public static void AppendDataToMMXExcel(Stream stream, DataTable data, int tIndex, List<MappingKey> lMappingKeys)
        {
            stream.Seek(0, SeekOrigin.Begin);

            // Open a SpreadsheetDocument based on a stream.
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(stream, true))
            {

                // Get Workbook Part of Spread Sheet Document
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                //WorksheetPart wsPart = workbookPart.WorksheetParts.FirstOrDefault();
                IEnumerable<Sheet> sheets = workbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string firstSheetId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(firstSheetId);
                Worksheet workSheet = worksheetPart.Worksheet;

                //Add By Hason 2015.4.24
                WorkbookStylesPart workBookStylePart = workbookPart.WorkbookStylesPart;
                Stylesheet styleSheet = workBookStylePart.Stylesheet;
                CellFormats cellFormarts = styleSheet.CellFormats;
                NumberingFormats numberFormats = styleSheet.NumberingFormats;
                if (null == numberFormats)
                {
                    numberFormats = new NumberingFormats();
                    styleSheet.PrependChild<NumberingFormats>(numberFormats);
                }
                ApplyFormat(cellFormarts, numberFormats);

                // Get Data in Excel file
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rowcollection = sheetData.Descendants<Row>();
                UInt32Value eRowItemIndex = (uint)rowcollection.Count();

                List<string> cellNames = new List<string>();
                if (eRowItemIndex > (uint)tIndex)
                {
                    List<Row> rows = rowcollection.ToList();
                    cellNames = GetCellNames(spreadsheetDocument, rows, tIndex);
                }


                foreach (DataRow dr in data.Rows)
                {
                    eRowItemIndex++;
                    var excelRow = new Row
                    {
                        RowIndex = eRowItemIndex
                    };
                    uint i = 0;
                    foreach (string cellName in cellNames)
                    {
                        NumberFormat numberFormat = NumberFormat.Normal;
                        CellValues cValueType = GetCellType(lMappingKeys, cellName, out numberFormat);
                        var cell = new Cell();
                        if (cValueType == CellValues.Number)
                        {
                            if (numberFormat == NumberFormat.Percentage)
                            {
                                uint styleIndex = GetStyleIndex(cellFormarts, PercentIndex);
                                cell.StyleIndex = styleIndex;
                            }
                            string value = GetDataColumnValue(dr, lMappingKeys, cellName);
                            if (!string.IsNullOrEmpty(value))
                                cell.CellValue = new CellValue(value);
                        }
                        else if (cValueType == CellValues.Date)
                        {
                            uint styleIndex = GetStyleIndex(cellFormarts, DateFormateIndex);
                            string value = GetDataColumnValue(dr, lMappingKeys, cellName);
                            cell.DataType = CellValues.Date;
                            cell.StyleIndex = styleIndex;
                            cell.CellValue = new CellValue(value);
                        }
                        else
                        {
                            string value = GetDataColumnValue(dr, lMappingKeys, cellName);

                            var text = new Text()
                            {
                                Text = value
                            };

                            var inlineString = new InlineString();
                            inlineString.AppendChild(text);

                            cell.DataType = CellValues.InlineString;
                            cell.CellReference = GetColumnName((UInt32)i + 1) + excelRow.RowIndex;
                            if (!string.IsNullOrEmpty(value))
                            {
                                cell.AppendChild(inlineString);
                            }
                        }
                        excelRow.AppendChild<Cell>(cell);

                        i++;
                    }

                    sheetData.AppendChild<Row>(excelRow);
                }
                //wsPart.Worksheet.Save();
                workSheet.Save();
            }
        }
        /// <summary>
        /// Create Master List Excel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="stream"></param>
        public static void CreateMasterListExcel(DataSet ds, Stream stream, List<MappingKey> lMappingKeys)
        {
            using (var doc = SpreadsheetDocument.Open(stream, true))
            {
                // Get Workbook Part of Spread Sheet Document
                WorkbookPart wbpart = doc.WorkbookPart;
                WorksheetPart wsPart = wbpart.WorksheetParts.FirstOrDefault();

                //Get SheetData of existing template sheet
                SheetData ExistSheetData = wsPart.Worksheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rowcollection = ExistSheetData.Descendants<Row>();

                UInt32Value eRowsCount = (uint)rowcollection.Count();

                int tIndex = 1;
                List<string> cellNames = new List<string>();
                if (eRowsCount > (uint)tIndex)
                {
                    List<Row> rows = rowcollection.ToList();
                    cellNames = GetCellNames(doc, rows, tIndex);
                }
                #region "Generate Excel Sheets based on DataSet Source"
                foreach (DataTable table in ds.Tables)
                {
                    var sheetPart = doc.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = ExistSheetData.Clone() as SheetData;

                    sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = doc.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                    string relationshipId = doc.WorkbookPart.GetIdOfPart(sheetPart);

                    uint sheetId = 1;
                    if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);
                    uint eRowItemIndex = 2;
                    foreach (System.Data.DataRow dsrow in table.Rows)
                    {
                        eRowItemIndex++;
                        var excelRow = new Row
                        {
                            RowIndex = eRowItemIndex
                        };
                        uint i = 0;
                        foreach (string cellName in cellNames)
                        {
                            NumberFormat numberFormat = NumberFormat.Normal;
                            CellValues cValueType = GetCellType(lMappingKeys, cellName, out numberFormat);
                            var cell = new Cell();
                            if (cValueType == CellValues.Number)
                            {
                                string value = GetDataColumnValue(dsrow, lMappingKeys, cellName);
                                if (!string.IsNullOrEmpty(value))
                                    cell.CellValue = new CellValue(value);
                            }
                            else
                            {
                                var text = new Text()
                                {
                                    Text = GetDataColumnValue(dsrow, lMappingKeys, cellName)
                                };

                                var inlineString = new InlineString();
                                inlineString.AppendChild(text);

                                cell.DataType = CellValues.InlineString;
                                cell.CellReference = GetColumnName((UInt32)i + 1) + excelRow.RowIndex;

                                cell.AppendChild(inlineString);
                            }
                            excelRow.AppendChild<Cell>(cell);

                            i++;
                        }

                        sheetData.AppendChild<Row>(excelRow);
                    }

                }

                #endregion

                #region "delete existing template sheet"
                //Sheet theSheet = wbpart.Workbook.Descendants<Sheet>().Where(s => s.Name == Util.dtc_master_list_template_sheetname).FirstOrDefault();

                //Store the SheetID for the reference
                //uint Sheetid = theSheet.SheetId;

                // Remove the sheet reference from the workbook.
                //WorksheetPart worksheetPart = (WorksheetPart)(wbpart.GetPartById(theSheet.Id));
                //theSheet.Remove();

                // Delete the worksheet part.
                //wbpart.DeletePart(worksheetPart);
                #endregion
            }
        }
        private static List<DocumentFormat.OpenXml.Spreadsheet.Row> GetMasterListHeaders(string s_file)
        {
            List<DocumentFormat.OpenXml.Spreadsheet.Row> headers = null;
            try
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(s_file, true))
                {
                    // Get Workbook Part of Spread Sheet Document
                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    WorksheetPart wsPart = workbookPart.WorksheetParts.FirstOrDefault();

                    // Get Data in Excel file
                    SheetData sheetData = wsPart.Worksheet.GetFirstChild<SheetData>();
                    List<Row> rowcollection = sheetData.Descendants<Row>().ToList();
                    for (int i = 0; i < rowcollection.Count; i++)
                    {
                        if (i > 2) { break; }
                        else
                        {
                            headers.Add(rowcollection[i]);
                        }

                    }
                }
            }
            catch
            { }
            return headers;
        }
        /// <summary>
        ///  To Do: Get Cell Name as a mapping keyword
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="_rows"></param>
        /// <param name="tIndex">Starting from 0 </param>
        /// <returns></returns>
        static List<string> GetCellNames(SpreadsheetDocument doc, List<Row> _rows, int tIndex)
        {
            List<string> cNames = new List<string>();
            Row _eRow = _rows[tIndex];
            foreach (Cell cell in _eRow.Descendants<Cell>())
            {
                // Get Cell Column Index                        
                string sCellName = GetValueOfCell(doc, cell);
                if (!string.IsNullOrEmpty(sCellName))
                {
                    cNames.Add(sCellName);
                }
                else { break; }
            }

            return cNames;
        }
        /// <summary>
        ///  Get Value of Cell 
        /// </summary>
        /// <param name="spreadsheetdocument">SpreadSheet Document Object</param>
        /// <param name="cell">Cell Object</param>
        /// <returns>The Value in Cell</returns>
        private static string GetValueOfCell(SpreadsheetDocument spreadsheetdocument, Cell cell)
        {
            // Get value in Cell
            SharedStringTablePart sharedString = spreadsheetdocument.WorkbookPart.SharedStringTablePart;
            if (cell.InnerText == null)
            {
                return string.Empty;
            }

            string cellValue = cell.InnerText;

            // The condition that the Cell DataType is SharedString
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return sharedString.SharedStringTable.ChildElements[int.Parse(cellValue)].InnerText;
            }
            else
            {
                return cellValue;
            }
        }
        /// <summary>
        ///  To do : Get Value of Datarow 
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        static string GetDataColumnValue(DataRow dr, List<MappingKey> keys, string cellName)
        {
            string sValue = string.Empty;
            try
            {
                MappingKey mKey = keys.Where(k => k.Key.ToUpper() == cellName.ToUpper()).FirstOrDefault();
                if (mKey != null)
                {
                    if (dr.Table.Columns.Contains(mKey.Value))
                    {
                        switch (mKey.fType)
                        {
                            case FieldDataType.Date:
                                sValue = ConvertToDateString(dr[mKey.Value]);
                                break;
                            default:
                                sValue = dr[mKey.Value] == null ? "" : dr[mKey.Value].ToString();
                                break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            { }
            return sValue;
        }
        static CellValues GetCellType(List<MappingKey> keys, string cellName, out NumberFormat numberFormat)
        {
            MappingKey key = keys.Where(k => k.Key.ToUpper() == cellName.ToUpper()).FirstOrDefault();
            numberFormat = NumberFormat.Normal;
            if (key != null)
            {
                numberFormat = key.NumberFormat;
                CellValues cv;
                switch (key.fType)
                {
                    case FieldDataType.Int:
                    case FieldDataType.Float:
                        cv = CellValues.Number;
                        break;
                    //Add by Hason 2015.4.24
                    case FieldDataType.Date:
                        cv = CellValues.Date;
                        break;
                    default:
                        cv = CellValues.InlineString;
                        break;
                }
                return cv;
            }
            else
            {
                return CellValues.InlineString;

            }
        }
        static string ConvertToDateString(object oValue)
        {
            if (oValue == null) return "";
            if (oValue.ToString().Trim().Equals(string.Empty)) return "";
            try
            {
                return (DateTime.Parse(oValue.ToString())).ToString("yyyy-MM-dd");
            }
            catch
            {
                //For DAM "Interior PDP Image Deliver Date" Column
                return oValue.ToString();
                //return "";
            }
        }
        /// <summary>
        /// get 
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private static string GetColumnName(uint columnIndex)
        {
            var dividend = (int)columnIndex;
            var columnName = string.Empty;
            int modifier;

            while (dividend > 0)
            {
                modifier = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modifier).ToString() + columnName;
                dividend = (int)((dividend - modifier) / 26);
            }

            return columnName;
        }
        public static void DoExport(DataSet ds, Stream stream/*, string filePath*/)
        {
            using (var doc = SpreadsheetDocument.Open(stream, true))
            {
                // Get Workbook Part of Spread Sheet Document
                WorkbookPart wbpart = doc.WorkbookPart;


                WorksheetPart wsPart = wbpart.WorksheetParts.FirstOrDefault();

                //Stylesheet styleSheet = CreateStylesheet(wbpart);
                //wbpart.WorkbookStylesPart.Stylesheet = styleSheet;


                //Get SheetData of existing template sheet
                SheetData ExistSheetData = wsPart.Worksheet.GetFirstChild<SheetData>();

                CreateStylesheet(wbpart);

                //Columns columns = new Columns();

                //columns.Append(CreateColumnData(2, 2, 30));//第2列宽度13

                //wsPart.Worksheet.Append(columns);



                IEnumerable<Row> rowcollection = ExistSheetData.Descendants<Row>();

                UInt32Value eRowsCount = (uint)rowcollection.Count();

                #region "Generate Excel Sheets based on DataSet Source"
                foreach (DataTable table in ds.Tables)
                {
                    var sheetPart = doc.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = ExistSheetData.Clone() as SheetData;
                    List<string> cellNames = new List<string>();

                    sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = doc.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                    string relationshipId = doc.WorkbookPart.GetIdOfPart(sheetPart);

                    uint sheetId = 0;
                    if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);

                    uint eRowItemIndex = 1;
                    var excelRow = new Row
                    {
                        RowIndex = eRowItemIndex
                    };
                    foreach (DataColumn dc in table.Columns)
                    {
                        if (dc.ColumnName == "Ischanged" || dc.ColumnName == "Isadded")
                        {
                            continue;
                        }

                        // dc.ColumnName = dc.ColumnName.Replace("_", " ");

                        var cell = new Cell();
                        if (!string.IsNullOrEmpty(dc.ColumnName))
                        {
                            var text = new Text()
                            {
                                Text = dc.ColumnName
                            };
                            var inlineStringColumn = new InlineString();
                            inlineStringColumn.AppendChild(text);

                            cellNames.Add(text.Text);

                            cell.DataType = CellValues.InlineString;
                            cell.StyleIndex = (UInt32Value)2U;

                            cell.AppendChild(inlineStringColumn);

                        }
                        excelRow.AppendChild<Cell>(cell);
                    }

                    sheetData.AppendChild<Row>(excelRow);

                    foreach (System.Data.DataRow dsrow in table.Rows)
                    {
                        eRowItemIndex++;
                        excelRow = new Row
                        {
                            RowIndex = eRowItemIndex
                        };

                        foreach (string cellName in cellNames)
                        {
                            var cell = new Cell();
                            cell.StyleIndex = (UInt32Value)3U;

                            string sValue = string.Empty;
                            sValue = dsrow[cellName].ToString();

                            if (!(table.TableName == "Top5"))
                            {
                                string isadded = dsrow["Isadded"].ToString();

                                if (isadded.Equals("True"))
                                {
                                    cell.StyleIndex = (UInt32Value)5U;
                                }
                            }


                            if (!sValue.Equals(string.Empty) && sValue.Length >= 4 &&
                             sValue.Substring(0, 4).Equals("||||"))
                            {
                                sValue = sValue.Substring(4);
                                cell.StyleIndex = (UInt32Value)4U;
                            }

                            var text = new Text()
                            {
                                Text = sValue
                            };

                            var inlineString = new InlineString();
                            inlineString.AppendChild(text);

                            cell.DataType = CellValues.InlineString;


                            cell.AppendChild(inlineString);

                            excelRow.AppendChild<Cell>(cell);

                        }
                        sheetData.AppendChild<Row>(excelRow);
                    }
                }
                #endregion

                #region "delete existing template sheet"
                Sheet theSheet = wbpart.Workbook.Descendants<Sheet>().Where(s => s.Name == "Sheet1").FirstOrDefault();

                //Store the SheetID for the reference
                uint Sheetid = theSheet.SheetId;

                // Remove the sheet reference from the workbook.
                WorksheetPart worksheetPart = (WorksheetPart)(wbpart.GetPartById(theSheet.Id));
                theSheet.Remove();

                // Delete the worksheet part.
                wbpart.DeletePart(worksheetPart);
                #endregion
            }
        }
        public static void DownloadMaster(DataSet ds, Stream stream, List<MappingKey> mp)
        {

            {
                using (var doc = SpreadsheetDocument.Open(stream, true))
                {
                    WorkbookPart wbpart = doc.WorkbookPart;
                    WorksheetPart wsPart = wbpart.WorksheetParts.FirstOrDefault();
                    SheetData ExistSheetData = wsPart.Worksheet.GetFirstChild<SheetData>();

                    CreateStylesheet(wbpart);

                    IEnumerable<Row> rowcollection = ExistSheetData.Descendants<Row>();

                    UInt32Value eRowsCount = (uint)rowcollection.Count();

                    #region "Generate Excel Sheets based on DataSet Source"
                    DataTableCollection excelDt = ds.Tables;
                    foreach (DataTable table in excelDt)
                    {
                        var sheetPart = doc.WorkbookPart.AddNewPart<WorksheetPart>();
                       
                        var sheetData = ExistSheetData.Clone() as SheetData;
                        List<string> cellNames = new List<string>();

                        sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                        DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = doc.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                        string relationshipId = doc.WorkbookPart.GetIdOfPart(sheetPart);

                        uint sheetId = 0;
                        if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                        {
                            sheetId =
                                sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                        }

                        DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                        sheets.Append(sheet);

                        uint eRowItemIndex = 1;
                        var excelRow = new Row
                        {
                            RowIndex = eRowItemIndex
                        };
                        DataColumnCollection excelCols = table.Columns;
                        foreach (DataColumn dc in excelCols)
                        {
                            if (dc.ColumnName == "ISCHANGE" || dc.ColumnName == "ISADD")
                            {
                                cellNames.Add(dc.ColumnName);
                                continue;
                            }
                           
                            if (!string.IsNullOrEmpty(dc.ColumnName))
                            {
                                MappingKey mKey = mp.Where(k => k.Value.ToUpper() == dc.ColumnName.ToUpper()).FirstOrDefault();
                                string strValue = string.Empty;
                                if (mKey != null)
                                {
                                    cellNames.Add(dc.ColumnName);
                                    var cell = new Cell();

                                    strValue = mKey.Key;

                                    var text = new Text()
                                    {
                                        Text = strValue
                                    };
                                    
                                    var inlineStringColumn = new InlineString();
                                    inlineStringColumn.AppendChild(text);

                                    cell.DataType = CellValues.InlineString;
                                    //cell.DataType = CellValues.Number;
                                    cell.StyleIndex = (UInt32Value)2U;

                                    cell.AppendChild(inlineStringColumn);

                                    excelRow.AppendChild<Cell>(cell);
                                }
                            }
                        }

                        sheetData.AppendChild<Row>(excelRow);
                        DataRowCollection excelData = table.Rows;
                        foreach (System.Data.DataRow dsrow in excelData)
                        {
                            eRowItemIndex++;
                            excelRow = new Row
                            {
                                RowIndex = eRowItemIndex
                            };

                            foreach (string cellName in cellNames)
                            {
                                if (cellName == "ISCHANGE" || cellName == "ISADD")
                                {
                                    continue;
                                }
                                MappingKey mKey = mp.Where(k => k.Value.ToUpper() == cellName.ToUpper()).FirstOrDefault();
                                if (mKey == null)
                                {
                                    continue;
                                }
                                var cell = new Cell();
                                cell.StyleIndex = (UInt32Value)3U;

                                string sValue = string.Empty;
                                sValue = dsrow[cellName].ToString();
                                if (dsrow.Table.Columns.Contains("ISADD") && dsrow["ISADD"].ToString() == "1")
                                {
                                    cell.StyleIndex = (UInt32Value)5U;
                                }
                                if (!sValue.Equals(string.Empty) && sValue.Length >= 4 && sValue.Substring(0, 4).Equals("||||"))
                                {
                                    sValue = sValue.Substring(4);
                                    cell.StyleIndex = (UInt32Value)4U;
                                }
                                if(mKey.fType==FieldDataType.Date)
                                {
                                    if (!string.IsNullOrEmpty(sValue))
                                    {
                                        sValue = DateTime.Parse(sValue).ToString("yyyy-MM-dd");
                                    }
                                }
                                var text = new Text()
                                {
                                    Text = sValue
                                };



                                if (mKey.fType == FieldDataType.Float)
                                {
                                    cell.CellValue = new CellValue(sValue);
                                    cell.DataType = CellValues.Number;
                                    
                                }
                                else
                                {
                                    cell.DataType = CellValues.InlineString;
                                    var inlineString = new InlineString();
                                    inlineString.AppendChild(text);
                                    cell.AppendChild(inlineString);
                                }
                                excelRow.AppendChild<Cell>(cell);

                            }
                            sheetData.AppendChild<Row>(excelRow);
                        }
                    }
                    #endregion

                    #region "delete existing template sheet"
                    Sheet theSheet = wbpart.Workbook.Descendants<Sheet>().Where(s => s.Name == "Sheet1").FirstOrDefault();

                    //Store the SheetID for the reference
                    uint Sheetid = theSheet.SheetId;

                    // Remove the sheet reference from the workbook.
                    WorksheetPart worksheetPart = (WorksheetPart)(wbpart.GetPartById(theSheet.Id));
                    theSheet.Remove();

                    // Delete the worksheet part.
                    //wbpart.DeletePart(worksheetPart);
                    #endregion
                }
            }
        }
        /// <summary>
        /// 导出会计凭证
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="stream"></param>
        public static void DownloadAccountingDocument_back(DataSet ds, Stream stream)
        {
            try
            {
                using (var doc = SpreadsheetDocument.Open(stream, true))
                {
                    WorkbookPart wbpart = doc.WorkbookPart;
                    WorksheetPart wsPart = wbpart.WorksheetParts.FirstOrDefault();
                    SheetData ExistSheetData = wsPart.Worksheet.GetFirstChild<SheetData>();

                    CreateStylesheet(wbpart);

                    IEnumerable<Row> rowcollection = ExistSheetData.Descendants<Row>();

                    UInt32Value eRowsCount = (uint)rowcollection.Count();

                    #region "Generate Excel Sheets based on DataSet Source"
                    DataTableCollection excelDt = ds.Tables;
                    foreach (DataTable table in excelDt)
                    {
                        var sheetPart = doc.WorkbookPart.AddNewPart<WorksheetPart>();
                        var sheetData = ExistSheetData.Clone() as SheetData;
                        List<string> cellNames = new List<string>();

                        sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                        DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = doc.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                        string relationshipId = doc.WorkbookPart.GetIdOfPart(sheetPart);

                        uint sheetId = 0;
                        if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                        {
                            sheetId =
                                sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                        }

                        DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                        sheets.Append(sheet);

                        int eRowItemIndex = 2;//开始行
                        int eColItemIndex = 2;//开始列

                        int PeriodIndex = 0;
                        for (uint i = 1; i <= 50; i++)//设置20行
                        {
                            var excelRow = new Row
                            {
                                RowIndex = i
                            };
                            for (int j = 0; j <= 10; j++)
                            {
                                var cell = new Cell();
                                cell.DataType = CellValues.InlineString;
                                excelRow.AppendChild<Cell>(cell);
                            }
                            
                            sheetData.AppendChild<Row>(excelRow);
                        }


                        DataRowCollection excelData = table.Rows;

                        Cell cellNew = new Cell();
                        cellNew.DataType = CellValues.InlineString;

                        int OneLocalNum = 0;
                        int TwoLocalNum = 0;
                        int ThreeLocalNum = 0;
                        foreach (System.Data.DataRow dsrow in excelData)
                        {
                            double RightOfUserAssets = 0.0 ;
                            double.TryParse(dsrow["RightOfUserAssets"].ToString(), out RightOfUserAssets);
                            

                            double AmountContract = 0.0;
                            double.TryParse(dsrow["AmountOfPeriods"].ToString(), out AmountContract);

                            double InterestExpense = 0.0;
                            double Amortization = 0.0;
                            if(excelData.Count>1)
                            {
                                if (excelData.Count > PeriodIndex + 1)//第一期和最后一期没有利息
                                {
                                    double.TryParse(excelData[PeriodIndex + 1]["InterestExpense"].ToString(), out InterestExpense);
                                }

                                if (excelData.Count > 1)
                                {
                                    double.TryParse(excelData[1]["Amortization"].ToString(), out Amortization);
                                }
                            }
                           

                            double EarlyPament = 0.0;
                            double.TryParse(excelData[PeriodIndex]["EarlyPament"].ToString(), out EarlyPament);

                            string ProceedsEndDate = "";
                            //if (PeriodIndex > 0)
                            //{
                                ProceedsEndDate = excelData[PeriodIndex]["ProceedsEndDate"].ToString().Split(' ')[0];
                           // }
                            string ProceedsStartDate = "";
                            //if (PeriodIndex > 0)
                            //{
                                ProceedsStartDate = excelData[PeriodIndex]["ProceedsStartDate"].ToString().Split(' ')[0];
                           // }


                            if (PeriodIndex == 0)
                            {
                                //---------------------------1、租赁期开始日（初始确认租赁相关的资产和负债）
                                Row eRowItemOne = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItem1_cell = eRowItemOne.Descendants<Cell>().ElementAt(eColItemIndex - 1);
                                eRowItem1_cell.AppendChild(new InlineString(new Text() { Text = "1、租赁期开始日（初始确认租赁相关的资产和负债）" }));
                                eRowItemIndex += 1;

                                //每个借贷前空一行
                                eRowItemIndex++;
                                Row eRowItem = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                eRowItemIndex += 1;
                                Row eRowItem2 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var cell_ = eRowItem.Descendants<Cell>().ElementAt(eColItemIndex - 1);
                                var cell2_ = eRowItem2.Descendants<Cell>().ElementAt(eColItemIndex - 1);

                                
                                cell_.AppendChild(new InlineString(new Text() { Text = "借：使用权资产" }));
                                cell2_.AppendChild(new InlineString(new Text() { Text = " 贷：租赁负债 - 应付租赁款" }));
                                var cell = eRowItem.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cel2 = eRowItem2.Descendants<Cell>().ElementAt(eColItemIndex);

                                cell.AppendChild(new InlineString(new Text() { Text = RightOfUserAssets.ToString() }));
                                cel2.AppendChild(new InlineString(new Text() { Text = RightOfUserAssets.ToString() }));

                                //eRowItemIndex+=1;
                                eRowItemIndex += 1;
                                //每个借贷前空一行
                                eRowItemIndex++;
                                Row eRowItem3 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                eRowItemIndex += 1;
                                Row eRowItem4 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var cell3_ = eRowItem3.Descendants<Cell>().ElementAt(eColItemIndex - 1);
                                var cell4_ = eRowItem4.Descendants<Cell>().ElementAt(eColItemIndex - 1);
                                cell3_.AppendChild(new InlineString(new Text() { Text = "借：租赁负债-未确认融资费用" }));
                                cell4_.AppendChild(new InlineString(new Text() { Text = "  贷：租赁负债-应付租赁款" }));
                                var cell3 = eRowItem3.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cell4 = eRowItem4.Descendants<Cell>().ElementAt(eColItemIndex);
                                cell3.AppendChild(new InlineString(new Text() { Text = (AmountContract - RightOfUserAssets).ToString() }));
                                cell4.AppendChild(new InlineString(new Text() { Text = (AmountContract - RightOfUserAssets).ToString() }));



                                //--------------2、各期利息费用/折旧摊销
                                //1）利息费用
                                //eRowItemIndex+=1;
                                eRowItemIndex += 1;
                                //每个借贷前空一行
                                eRowItemIndex++;
                                Row eRowItemTwo = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItem2_cell = eRowItemTwo.Descendants<Cell>().ElementAt(eColItemIndex-1);
                                eRowItem2_cell.AppendChild(new InlineString(new Text() { Text = "2、各期利息费用/折旧摊销" }));

                                eRowItemIndex += 1;
                                Row eRowItemTwo_1 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItem2_1cell = eRowItemTwo_1.Descendants<Cell>().ElementAt(eColItemIndex - 1);
                                eRowItem2_1cell.AppendChild(new InlineString(new Text() { Text = "1）利息费用" }));

                                eRowItemIndex += 1;
                                OneLocalNum = eRowItemIndex;
                                Row eRowItemTwo_1_1 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItemTwo_1_1_cell = eRowItemTwo_1_1.Descendants<Cell>().ElementAt(eColItemIndex);
                                eRowItemTwo_1_1_cell.AppendChild(new InlineString(new Text() { Text = ProceedsEndDate }));



                                eRowItemIndex += 1;
                                Row eRowItem7 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                eRowItemIndex += 1;
                                Row eRowItem8 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var cell7 = eRowItem7.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cell8 = eRowItem8.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cell7_ = eRowItem7.Descendants<Cell>().ElementAt(eColItemIndex - 1);
                                var cell8_ = eRowItem8.Descendants<Cell>().ElementAt(eColItemIndex - 1);
                                cell7_.AppendChild(new InlineString(new Text() { Text = "借：财务费用---利息费用" }));
                                cell8_.AppendChild(new InlineString(new Text() { Text = "  贷：租赁负债-未确认融资费用" }));
                                //if (excelData.Count == 1)
                                //{
                                //    cell7.AppendChild(new InlineString(new Text() { Text = "" }));
                                //    cell8.AppendChild(new InlineString(new Text() { Text = "" }));
                                //}
                                //else
                                //{
                                    cell7.AppendChild(new InlineString(new Text() { Text = InterestExpense.ToString() }));
                                    cell8.AppendChild(new InlineString(new Text() { Text = InterestExpense.ToString() }));
                                //}

                                //2）折旧摊销
                                //eRowItemIndex+=1;
                                //每个借贷前空一行
                                eRowItemIndex++;
                                eRowItemIndex += 1;
                                Row eRowItemTwo_2 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItemTwo_2_cell = eRowItemTwo_2.Descendants<Cell>().ElementAt(eColItemIndex-1);
                                eRowItemTwo_2_cell.AppendChild(new InlineString(new Text() { Text = "2）折旧摊销" }));

                                eRowItemIndex += 1;
                                TwoLocalNum = eRowItemIndex;
                                Row eRowItemTwo_1_2 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItemTwo_1_2_cell = eRowItemTwo_1_2.Descendants<Cell>().ElementAt(eColItemIndex);
                                eRowItemTwo_1_2_cell.AppendChild(new InlineString(new Text() { Text = ProceedsEndDate }));


                                eRowItemIndex+=1;
                                Row eRowItem10 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                eRowItemIndex += 1;
                                Row eRowItem11 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var cell10 = eRowItem10.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cell11 = eRowItem11.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cell10_ = eRowItem10.Descendants<Cell>().ElementAt(eColItemIndex - 1);
                                var cell11_ = eRowItem11.Descendants<Cell>().ElementAt(eColItemIndex - 1);
                                cell10_.AppendChild(new InlineString(new Text() { Text = "借：成本/费用" }));
                                cell11_.AppendChild(new InlineString(new Text() { Text = "  贷：使用权资产" }));
                                //if (excelData.Count == 1)
                                //{
                                //    cell10.AppendChild(new InlineString(new Text() { Text = "" }));
                                //    cell11.AppendChild(new InlineString(new Text() { Text = "" }));
                                //}
                                //else
                                //{
                                    cell10.AppendChild(new InlineString(new Text() { Text = Amortization.ToString() }));
                                    cell11.AppendChild(new InlineString(new Text() { Text = Amortization.ToString() }));
                                //}

                                //------------3、支付租金
                                //eRowItemIndex += 1;
                                //每个借贷前空一行
                                eRowItemIndex++;
                                eRowItemIndex +=1;
                                Row eRowItemTwo_3 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItemTwo_3_cell = eRowItemTwo_3.Descendants<Cell>().ElementAt(eColItemIndex-1);
                                eRowItemTwo_3_cell.AppendChild(new InlineString(new Text() { Text = "3、支付租金" }));

                                eRowItemIndex += 1;
                                ThreeLocalNum = eRowItemIndex;
                                Row eRowItemTwo_1_3 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItemTwo_1_3_cell = eRowItemTwo_1_3.Descendants<Cell>().ElementAt(eColItemIndex);
                                eRowItemTwo_1_3_cell.AppendChild(new InlineString(new Text() { Text = ProceedsStartDate }));

                                eRowItemIndex += 1;
                                Row eRowItem13 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                eRowItemIndex += 1;
                                Row eRowItem14 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var cell13 = eRowItem13.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cell14 = eRowItem14.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cell13_ = eRowItem13.Descendants<Cell>().ElementAt(eColItemIndex - 1);
                                var cell14_ = eRowItem14.Descendants<Cell>().ElementAt(eColItemIndex - 1);
                                cell13_.AppendChild(new InlineString(new Text() { Text = "借：租赁负债-应付租赁款" }));
                                cell14_.AppendChild(new InlineString(new Text() { Text = "贷：银行存款" }));

                                cell13.AppendChild(new InlineString(new Text() { Text = EarlyPament.ToString() }));
                                cell14.AppendChild(new InlineString(new Text() { Text = EarlyPament.ToString() }));

                            }
                            if (0 < PeriodIndex && PeriodIndex < excelData.Count - 1)
                            {
                                //--------------2、各期利息费用/折旧摊销
                                //1）利息费用

                                //double.TryParse(dsrow[PeriodIndex+1].ToString(), out InterestExpense);
                                eRowItemIndex = OneLocalNum;
                                Row eRowItemTwo_1 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItemTwo_1_cell = eRowItemTwo_1.Descendants<Cell>().ElementAt(eColItemIndex);
                                eRowItemTwo_1_cell.AppendChild(new InlineString(new Text() { Text = ProceedsEndDate }));

                                eRowItemIndex++;
                                Row eRowItem7 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                eRowItemIndex++;
                                Row eRowItem8 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var cell7 = eRowItem7.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cell8 = eRowItem8.Descendants<Cell>().ElementAt(eColItemIndex);

                                cell7.AppendChild(new InlineString(new Text() { Text = InterestExpense.ToString() }));
                                cell8.AppendChild(new InlineString(new Text() { Text = InterestExpense.ToString() }));



                                //2）折旧摊销
                                eRowItemIndex = TwoLocalNum;
                                Row eRowItemTwo_2 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItemTwo_2_cell = eRowItemTwo_2.Descendants<Cell>().ElementAt(eColItemIndex);
                                eRowItemTwo_2_cell.AppendChild(new InlineString(new Text() { Text = ProceedsEndDate }));

                                eRowItemIndex += 1;
                                Row eRowItem10 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                eRowItemIndex += 1;
                                Row eRowItem11 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var cell10 = eRowItem10.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cell11 = eRowItem11.Descendants<Cell>().ElementAt(eColItemIndex);

                                cell10.AppendChild(new InlineString(new Text() { Text = Amortization.ToString() }));
                                cell11.AppendChild(new InlineString(new Text() { Text = Amortization.ToString() }));


                                //------------3、支付租金
                                eRowItemIndex = ThreeLocalNum;
                                Row eRowItemTwo_3 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItemTwo_3_cell = eRowItemTwo_3.Descendants<Cell>().ElementAt(eColItemIndex);
                                eRowItemTwo_3_cell.AppendChild(new InlineString(new Text() { Text = ProceedsStartDate }));

                                eRowItemIndex++;
                                Row eRowItem13 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                eRowItemIndex += 1;
                                Row eRowItem14 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var cell13 = eRowItem13.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cell14 = eRowItem14.Descendants<Cell>().ElementAt(eColItemIndex);

                                cell13.AppendChild(new InlineString(new Text() { Text = EarlyPament.ToString() }));
                                cell14.AppendChild(new InlineString(new Text() { Text = EarlyPament.ToString() }));
                            }
                            if (PeriodIndex == excelData.Count - 1)
                            {
                                if (excelData.Count == 1)
                                {
                                    eColItemIndex++;
                                }

                                //--------------2、各期利息费用/折旧摊销
                                //1）利息费用
                                eRowItemIndex = OneLocalNum;
                                Row eRowItemTwo_1 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItemTwo_1_cell = eRowItemTwo_1.Descendants<Cell>().ElementAt(eColItemIndex);
                                eRowItemTwo_1_cell.AppendChild(new InlineString(new Text() { Text = ProceedsEndDate }));

                                eRowItemIndex++;
                                Row eRowItem7 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                eRowItemIndex++;
                                Row eRowItem8 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var cell7 = eRowItem7.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cell8 = eRowItem8.Descendants<Cell>().ElementAt(eColItemIndex);
                                cell7.AppendChild(new InlineString(new Text() { Text = InterestExpense.ToString() }));
                                cell8.AppendChild(new InlineString(new Text() { Text = InterestExpense.ToString() }));



                                //2）折旧摊销
                                //2）折旧摊销
                                eRowItemIndex = TwoLocalNum;
                                // eRowItemIndex++;
                                Row eRowItemTwo_2 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItemTwo_2_cell = eRowItemTwo_2.Descendants<Cell>().ElementAt(eColItemIndex);
                                eRowItemTwo_2_cell.AppendChild(new InlineString(new Text() { Text = ProceedsEndDate }));
                                eRowItemIndex++;
                                Row eRowItem10 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                eRowItemIndex++;
                                Row eRowItem11 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var cell10 = eRowItem10.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cell11 = eRowItem11.Descendants<Cell>().ElementAt(eColItemIndex);
                                cell10.AppendChild(new InlineString(new Text() { Text = Amortization.ToString() }));
                                cell11.AppendChild(new InlineString(new Text() { Text = Amortization.ToString() }));


                                //------------3、支付租金
                                //------------3、支付租金
                                eRowItemIndex = ThreeLocalNum;
                                Row eRowItemTwo_3 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var eRowItemTwo_3_cell = eRowItemTwo_3.Descendants<Cell>().ElementAt(eColItemIndex);
                                eRowItemTwo_3_cell.AppendChild(new InlineString(new Text() { Text = ProceedsStartDate }));
                                eRowItemIndex++;
                                Row eRowItem13 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                eRowItemIndex++;
                                Row eRowItem14 = sheetData.Descendants<Row>().ElementAt<Row>(eRowItemIndex);
                                var cell13 = eRowItem13.Descendants<Cell>().ElementAt(eColItemIndex);
                                var cell14 = eRowItem14.Descendants<Cell>().ElementAt(eColItemIndex);
                                cell13.AppendChild(new InlineString(new Text() { Text = EarlyPament.ToString() }));
                                cell14.AppendChild(new InlineString(new Text() { Text = EarlyPament.ToString() }));

                                }
                                PeriodIndex++;
                            eColItemIndex++;
                        }
                        
                    }
                    #endregion

                    #region "delete existing template sheet"
                    Sheet theSheet = wbpart.Workbook.Descendants<Sheet>().Where(s => s.Name == "Sheet1").FirstOrDefault();

                    //Store the SheetID for the reference
                    uint Sheetid = theSheet.SheetId;

                    // Remove the sheet reference from the workbook.
                    WorksheetPart worksheetPart = (WorksheetPart)(wbpart.GetPartById(theSheet.Id));
                    theSheet.Remove();

                    // Delete the worksheet part.
                    //wbpart.DeletePart(worksheetPart);
                    #endregion
                }
            }

            catch(Exception e)
            {


            }
            
            }
        //设置列样式
        private static Column CreateColumnData(UInt32 StartColumnIndex, UInt32 EndColumnIndex, double ColumnWidth)
        {
            Column column;
            column = new Column();
            column.Min = StartColumnIndex;//开始列号
            column.Max = EndColumnIndex;//结束列号
            //column.Width = ColumnWidth;//宽度
            column.CustomWidth = true;//自定义宽度
            column.BestFit = true;//自动调整大小
         
            return column;
        }
    private static Columns AutoFit(SheetData sheetData)
         {
             var maxColWidth = GetMaxCharacterWidth(sheetData);
 
             Columns columns = new Columns();
 
             double maxWidth = 7;
             foreach (var item in maxColWidth)
             {
                 /*三种单位宽度公式*/
                 double width = Math.Truncate((item.Value * maxWidth + 5) / maxWidth * 256) / 256;
                 double pixels = Math.Truncate(((256 * width + Math.Truncate(128 / maxWidth)) / 256) * maxWidth);
                 double charWidth = Math.Truncate((pixels - 5) / maxWidth * 100 + 0.5) / 100;
 
                 Column col = new Column() { BestFit = true, Min = (UInt32)(item.Key + 1), Max = (UInt32)(item.Key + 1), CustomWidth = true, Width = (DoubleValue)width };
                 columns.Append(col);
             }
             return columns;
         }

    private static Dictionary<int, int> GetMaxCharacterWidth(SheetData sheetData)
    {
        Dictionary<int, int> maxColWidth = new Dictionary<int, int>();
        var rows = sheetData.Elements<Row>();
        foreach (var r in rows)
        {
            var cells = r.Elements<Cell>().ToArray();
            for (int i = 0; i < cells.Length; i++)
            {
                var cell = cells[i];
                var cellValue = cell.CellValue == null ? string.Empty : cell.CellValue.InnerText;
                var cellTextLength = cellValue.Length;
                if (maxColWidth.ContainsKey(i))
                {
                    var current = maxColWidth[i];
                    if (cellTextLength > current)
                    {
                        maxColWidth[i] = cellTextLength;
                    }
                }
                else
                {
                    maxColWidth.Add(i, cellTextLength);
                }
            }
        }
        return maxColWidth;
    }


        private static void setRow(SheetData sheetData, DataRowCollection excelData, string columnName, uint eRowItemIndex, string FirstVAlue,int showIndex)
        {
            var excelRow = new Row
            {
                RowIndex = eRowItemIndex
            };

            var cell = new Cell();
            var text = new Text()
            {
                Text = FirstVAlue.ToString()
            };
            var inlineStringColumn = new InlineString();
            inlineStringColumn.AppendChild(text);


            
            cell.DataType = CellValues.InlineString;

            cell.AppendChild(inlineStringColumn);
            excelRow.AppendChild<Cell>(cell);

            int num = 0;
            foreach (System.Data.DataRow dsrow in excelData)
            {
                num++;
                string Cellvalue = "";
                
                if (columnName != "")
                {
                    Cellvalue = dsrow[columnName].ToString();
                }

                if (columnName == "ProceedsEndDate")
                {
                    Cellvalue = dsrow["ProceedsStartDate"].ToString().Split(' ')[0] + "至\r\n" + dsrow[columnName].ToString().Split(' ')[0];
                }
                if (columnName == "ProceedsStartDate")
                {
                    Cellvalue = dsrow["ProceedsStartDate"].ToString().Split(' ')[0] ;
                }
         
                cell = new Cell();
                text = new Text()
                {
                    Text = Cellvalue
                };
                inlineStringColumn = new InlineString();
                inlineStringColumn.AppendChild(text);

                cell.DataType = CellValues.InlineString;

                cell.AppendChild(inlineStringColumn);
                excelRow.AppendChild<Cell>(cell);

                if(num >= showIndex)
                { break; }
            }
            sheetData.AppendChild(excelRow);
        }
        /// <summary>
        /// 导出会计凭证
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="stream"></param>
        public static void DownloadAccountingDocument(DataSet ds, Stream stream)
        {
            try
            {
                using (var doc = SpreadsheetDocument.Open(stream, true))
                {
                    WorkbookPart wbpart = doc.WorkbookPart;
                    WorksheetPart wsPart = wbpart.WorksheetParts.FirstOrDefault();
                    SheetData ExistSheetData = wsPart.Worksheet.GetFirstChild<SheetData>();

                    CreateStylesheet(wbpart);

                    IEnumerable<Row> rowcollection = ExistSheetData.Descendants<Row>();

                    UInt32Value eRowsCount = (uint)rowcollection.Count();

                    #region "Generate Excel Sheets based on DataSet Source"
                    DataTableCollection excelDt = ds.Tables;
                    foreach (DataTable table in excelDt)
                    {
                        var sheetPart = doc.WorkbookPart.AddNewPart<WorksheetPart>();
                        var sheetData = ExistSheetData.Clone() as SheetData;

                        sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                        DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = doc.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                        string relationshipId = doc.WorkbookPart.GetIdOfPart(sheetPart);

                        uint sheetId = 0;
                        if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                        {
                            sheetId =
                                sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                        }

                        DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                        sheets.Append(sheet);
                        uint eRowItemIndex = 1;
                        DataRowCollection excelData = table.Rows;
                        DataColumnCollection excelCols = table.Columns;
                        setRow(sheetData, excelData, "", 3, "1、租赁期开始日（初始确认租赁相关的资产和负债）",100);
                        setRow(sheetData, excelData, "TotlePresentValue", 4, "借：使用权资产",1);
                        setRow(sheetData, excelData, "TotlePresentValue", 5, "  贷：租赁负债 -应付租赁款", 1);
                        setRow(sheetData, excelData, "TotleInterestExpense", 7, "借：租赁负债-未确认融资费用", 1);
                        setRow(sheetData, excelData, "TotleInterestExpense", 8, "  贷：租赁负债-应付租赁款", 1);
                        setRow(sheetData, excelData, "", 10, "2、各期利息费用 / 折旧摊销", 100);
                        setRow(sheetData, excelData, "", 11, "1）利息费用", 100);
                        setRow(sheetData, excelData, "ProceedsEndDate", 12, "", 100);
                        setRow(sheetData, excelData, "InterestExpense", 13, "借：财务费用---利息费用", 100);
                        setRow(sheetData, excelData, "InterestExpense", 14, " 贷：租赁负债-未确认融资费用", 100);
                        setRow(sheetData, excelData, "", 16, "                         2）折旧摊销", 100);
                        setRow(sheetData, excelData, "ProceedsEndDate", 17, "", 100);
                        setRow(sheetData, excelData, "Amortization", 18, "借：成本/费用", 100);
                        setRow(sheetData, excelData, "Amortization", 19, " 贷：使用权资产", 100);
                        setRow(sheetData, excelData, "", 21, "   3、支付租金", 100);
                        setRow(sheetData, excelData, "ProceedsStartDate", 22, "", 100);
                        setRow(sheetData, excelData, "AnnualAmountTaxExclusive", 23, "借：租赁负债-应付租赁款", 100);
                        setRow(sheetData, excelData, "RateAmount", 24, "     应交税费-应交增值税（进项税）13%", 100);
                        setRow(sheetData, excelData, "AnnualAmount", 25, "  贷：银行存款", 100);
                    }
                    #endregion
                    
                    #region "delete existing template sheet"
                    Sheet theSheet = wbpart.Workbook.Descendants<Sheet>().Where(s => s.Name == "Sheet1").FirstOrDefault();

                    //Store the SheetID for the reference
                    uint Sheetid = theSheet.SheetId;

                    // Remove the sheet reference from the workbook.
                    WorksheetPart worksheetPart = (WorksheetPart)(wbpart.GetPartById(theSheet.Id));
                    theSheet.Remove();

                    // Delete the worksheet part.
                    //wbpart.DeletePart(worksheetPart);
                    #endregion
                }
            }

            catch (Exception e)
            {
                throw e;
            }

        }

        #region Excel Style
        private static void CreateStylesheet(WorkbookPart workbookPart)
        {
            Stylesheet stylesheet = null;

            stylesheet = workbookPart.WorkbookStylesPart.Stylesheet;

            stylesheet.Fonts = new Fonts()
            {
                Count = (UInt32Value)3U
            };

            //fontId =0,默认样式
            Font fontDefault = new Font(
                                         new FontSize() { Val = 11D },
                                         new FontName() { Val = "Calibri" },
                                         new FontFamily() { Val = 2 },
                                         new FontScheme() { Val = FontSchemeValues.Minor });

            stylesheet.Fonts.Append(fontDefault);

            //fontId =1，标题样式
            Font fontTitle = new Font(new FontSize() { Val = 15D },
                                         new Bold() { Val = true },
                                         new FontName() { Val = "Calibri" },
                                         new FontFamily() { Val = 2 },
                                         new FontScheme() { Val = FontSchemeValues.Minor });
            stylesheet.Fonts.Append(fontTitle);

            //fontId =2，列头样式
            Font fontHeader = new Font(new FontSize() { Val = 11D },
                              new Bold() { Val = true },
                              new FontName() { Val = "Calibri" },
                              new FontFamily() { Val = 2 },
                              new FontScheme() { Val = FontSchemeValues.Major });
            stylesheet.Fonts.Append(fontHeader);

            //fillId,0总是None，1总是gray125，自定义的从fillid =2开始
            stylesheet.Fills = new Fills()
            {
                Count = (UInt32Value)4U
            };

            //fillid=0
            Fill fillDefault = new Fill(new PatternFill() { PatternType = PatternValues.None });
            stylesheet.Fills.Append(fillDefault);

            //fillid=1
            Fill fillGray = new Fill();
            PatternFill patternFillGray = new PatternFill()
            {
                PatternType = PatternValues.Gray125
            };
            fillGray.Append(patternFillGray);
            stylesheet.Fills.Append(fillGray);

            //fillid=2
            Fill fillGray2 = new Fill();
            PatternFill patternFillGray2 = new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFC0C0C0" } })
            {
                PatternType = PatternValues.Solid
            };
            fillGray2.Append(patternFillGray2);
            stylesheet.Fills.Append(fillGray2);

            //fillid=3
            Fill fillYellow = new Fill();
            PatternFill patternFillYellow = new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFFF00" } })
            {
                PatternType = PatternValues.Solid
            };
            fillYellow.Append(patternFillYellow);
            stylesheet.Fills.Append(fillYellow);

            //fillid=3
            Fill fillIsadded = new Fill();
            PatternFill patternFillIsadded = new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "97FFFF" } })
            {
                PatternType = PatternValues.Solid
            };
            fillIsadded.Append(patternFillIsadded);
            stylesheet.Fills.Append(fillIsadded);

            stylesheet.Borders = new Borders()
            {
                Count = (UInt32Value)2U
            };

            //borderID=0
            Border borderDefault = new Border(new LeftBorder(), new RightBorder(), new TopBorder() { }, new BottomBorder(), new DiagonalBorder());
            stylesheet.Borders.Append(borderDefault);

            //borderID=1
            Border borderContent = new Border(
               new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
               new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                new DiagonalBorder()
                );
            stylesheet.Borders.Append(borderContent);

            stylesheet.CellFormats = new CellFormats();
            stylesheet.CellFormats.Count = 3;

            //styleIndex =0U
            CellFormat cfDefault = new CellFormat();
            cfDefault.Alignment = new Alignment();
            cfDefault.NumberFormatId = 0;
            cfDefault.FontId = 0;
            cfDefault.BorderId = 0;
            cfDefault.FillId = 0;
            cfDefault.ApplyAlignment = true;
            cfDefault.ApplyBorder = true;
            stylesheet.CellFormats.Append(cfDefault);

            //styleIndex =1U
            CellFormat cfTitle = new CellFormat();
            cfTitle.Alignment = new Alignment();
            cfTitle.NumberFormatId = 0;
            cfTitle.FontId = 1;
            cfTitle.BorderId = 1;
            cfTitle.FillId = 0;
            cfTitle.ApplyBorder = true;
            cfTitle.ApplyAlignment = true;
            cfTitle.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            stylesheet.CellFormats.Append(cfTitle);

            //styleIndex =2U
            CellFormat cfHeader = new CellFormat();
            cfHeader.Alignment = new Alignment();
            cfHeader.NumberFormatId = 0;
            cfHeader.FontId = 2;
            cfHeader.BorderId = 1;
            cfHeader.FillId = 2;
            cfHeader.ApplyAlignment = true;
            cfHeader.ApplyBorder = true;
            cfHeader.ApplyFill = true;
            cfHeader.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            stylesheet.CellFormats.Append(cfHeader);

            //styleIndex =3U
            CellFormat cfContent = new CellFormat();
            cfContent.Alignment = new Alignment();
            cfContent.NumberFormatId = 0;
            cfContent.FontId = 0;
            cfContent.BorderId = 1;
            cfContent.FillId = 0;
            cfContent.ApplyAlignment = true;
            cfContent.ApplyBorder = true;
            stylesheet.CellFormats.Append(cfContent);

            //styleIndex =4U
            CellFormat cfYellowContent = new CellFormat();
            cfYellowContent.Alignment = new Alignment();
            cfYellowContent.NumberFormatId = 0;
            cfYellowContent.FontId = 0;
            cfYellowContent.BorderId = 1;
            cfYellowContent.FillId = 3;
            cfYellowContent.ApplyAlignment = true;
            cfYellowContent.ApplyBorder = true;
            stylesheet.CellFormats.Append(cfYellowContent);

            //styleIndex =5U
            CellFormat cfIsaddedContent = new CellFormat();
            cfIsaddedContent.Alignment = new Alignment();
            cfIsaddedContent.NumberFormatId = 0;
            cfIsaddedContent.FontId = 0;
            cfIsaddedContent.BorderId = 1;
            cfIsaddedContent.FillId = 4;
            cfIsaddedContent.ApplyAlignment = true;
            cfIsaddedContent.ApplyBorder = true;
            stylesheet.CellFormats.Append(cfIsaddedContent);

            workbookPart.WorkbookStylesPart.Stylesheet.Save();
            //return stylesheet;
        }
        private static NumberingFormat GetFormate(uint formatIndex, string formatCode)
        {
            NumberingFormat numberFormat = new NumberingFormat()
            {
                NumberFormatId = formatIndex,
                FormatCode = formatCode
            };
            return numberFormat;
        }
        private static void ApplyFormat(CellFormats cellFormates, NumberingFormats numberFormats)
        {
            Dictionary<uint, string> formatInfos = new Dictionary<uint, string>();
            formatInfos.Add(DateFormateIndex, "yyyy\\-mm\\-dd");
            formatInfos.Add(PercentIndex, "0.0%");
            foreach (KeyValuePair<uint, string> formatInfo in formatInfos)
            {
                NumberingFormat numberFormat = GetFormate(formatInfo.Key, formatInfo.Value);
                numberFormats.Append(numberFormat);
                CellFormat cf = SetCellFormat(formatInfo.Key);
                cellFormates.Append(cf);
            }
        }
        private static CellFormat SetCellFormat(uint formatIndex)
        {
            CellFormat cellFormat = new CellFormat()
            {
                NumberFormatId = formatIndex,
                FontId = (UInt32Value)0U,
                FillId = (UInt32Value)0U,
                BorderId = (UInt32Value)0U,
                FormatId = (UInt32Value)0U,
                ApplyNumberFormat = true
            };
            return cellFormat;
        }
        private static uint GetStyleIndex(CellFormats cfs, UInt32Value formateIndex)
        {
            uint index = 0;
            foreach (CellFormat cf in cfs)
            {
                index++;
                if (cf.NumberFormatId.InnerText == formateIndex.InnerText)
                {
                    return index - 1;
                }
                continue;
            }
            return 0U;
        }
        #endregion
        #region Effeciency test
        //Just for test
        public static void HasonData(Stream stream, DataTable data, int tIndex, List<MappingKey> lMappingKeys)
        {
            stream.Seek(0, SeekOrigin.Begin);

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(stream, true))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                //Set style of the sheet
                CellFormats cellFormarts = HasonGetSetFormat(workbookPart);
                //Get the work sheet
                string firstSheetId = HasonGetUsedSheetId(workbookPart);
                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(firstSheetId);
                Worksheet workSheet = worksheetPart.Worksheet;
                // Get Data in Excel file
                List<string> cellNames = new List<string>();
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rowcollection = sheetData.Descendants<Row>();
                int rowCount = rowcollection.Count();
                //UInt32Value eRowItemIndex = (uint)rowCount;
                List<Row> rows = rowcollection.ToList();
                cellNames = GetCellNames(spreadsheetDocument, rows, tIndex);
                int rowTitleIndex = 1;
                //
                WorksheetPart replacementPart = workbookPart.AddNewPart<WorksheetPart>();
                string replacementPartId = workbookPart.GetIdOfPart(replacementPart);
                OpenXmlReader reader = OpenXmlReader.Create(worksheetPart);
                OpenXmlWriter writer = OpenXmlWriter.Create(replacementPart);
                //Outout the data from table to excel
                DataRowCollection drs = data.Rows;
                while (reader.Read())
                {
                    if (reader.ElementType == typeof(Row))
                    {
                        if (reader.IsStartElement)
                        {
                            writer.WriteStartElement(reader);
                        }
                        else if (reader.IsEndElement)
                        {
                            writer.WriteEndElement();
                            if (rowTitleIndex == rowCount)
                            {
                                //Get the data from table
                                foreach (DataRow dr in drs)
                                {
                                    //eRowItemIndex++;
                                    //uint i = 0;
                                    writer.WriteStartElement(new Row());
                                    foreach (string cellName in cellNames)
                                    {
                                        NumberFormat numberFormat = NumberFormat.Normal;
                                        Cell c = new Cell();
                                        CellValues cValueType = GetCellType(lMappingKeys, cellName, out numberFormat);
                                        if (cValueType == CellValues.Number)
                                        {
                                            string value = GetDataColumnValue(dr, lMappingKeys, cellName);
                                            //if (!string.IsNullOrEmpty(value))
                                            c.CellValue = new CellValue(value);
                                        }
                                        else if (cValueType == CellValues.Date)
                                        {
                                            uint styleIndex = GetStyleIndex(cellFormarts, DateFormateIndex);
                                            string value = GetDataColumnValue(dr, lMappingKeys, cellName);
                                            List<OpenXmlAttribute> attrs = new List<OpenXmlAttribute>();
                                            attrs.Add(new OpenXmlAttribute("", "s", "", styleIndex.ToString()));
                                            attrs.Add(new OpenXmlAttribute("", "t", "", "d"));
                                            c.SetAttributes(attrs);
                                            //if (!string.IsNullOrEmpty(value))
                                            c.CellValue = new CellValue(value);
                                        }
                                        else
                                        {
                                            string value = GetDataColumnValue(dr, lMappingKeys, cellName);
                                            var text = new Text() { Text = value };
                                            var inlineString = new InlineString { Text = text };
                                            List<OpenXmlAttribute> attrs = new List<OpenXmlAttribute>();
                                            attrs.Add(new OpenXmlAttribute("", "t", "", "inlineStr"));
                                            c.SetAttributes(attrs);
                                            //if (!string.IsNullOrEmpty(value))
                                            //{
                                            //c.CellValue = new CellValue();
                                            c.InlineString = inlineString;
                                            //}
                                        }
                                        writer.WriteElement(c);
                                    }
                                    writer.WriteEndElement();
                                }
                                //continue;
                            }
                            rowTitleIndex++;
                        }
                    }
                    else if (reader.ElementType == typeof(CellValue))
                    {
                        writer.WriteElement(reader.LoadCurrentElement());
                    }
                    else
                    {
                        if (reader.IsStartElement)
                        {
                            writer.WriteStartElement(reader);
                        }
                        else if (reader.IsEndElement)
                        {
                            writer.WriteEndElement();
                        }
                    }

                }
                reader.Close();
                writer.Close();

                Sheet sheet = workbookPart.Workbook.Descendants<Sheet>()
                .Where(s => s.Id.Value.Equals(firstSheetId)).First();
                sheet.Id.Value = replacementPartId;
                workbookPart.DeletePart(worksheetPart);
            }
        }
        private static CellFormats HasonGetSetFormat(WorkbookPart workbookPart)
        {
            WorkbookStylesPart workBookStylePart = workbookPart.WorkbookStylesPart;
            Stylesheet styleSheet = workBookStylePart.Stylesheet;
            CellFormats cellFormarts = styleSheet.CellFormats;
            NumberingFormats numberFormats = styleSheet.NumberingFormats;
            ApplyFormat(cellFormarts, numberFormats);

            return cellFormarts;
        }
        private static string HasonGetUsedSheetId(WorkbookPart workbookPart)
        {
            IEnumerable<Sheet> sheets = workbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
            string firstSheetId = sheets.First().Id.Value;

            return firstSheetId;
        }
        //
        #endregion
    }
}
