using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Framework
{
    public class ExcelHelperV2 : IDisposable
    {
        private bool disposed = false;
        private IWorkbook workbook = null;
        public ExcelHelperV2(Stream stream)
        {
            workbook = new HSSFWorkbook(stream);
        }

        public ExcelHelperV2(Stream stream, string fileName)
        {
            if (fileName.EndsWith(".xls"))
                workbook = new HSSFWorkbook(stream);
            else if (fileName.EndsWith(".xlsx"))
                workbook = new XSSFWorkbook(stream);
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="firstRowNum">第一行行号</param>
        /// <param name="lastRowNum">最后一行行号</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(int firstRowNum = 0, int lastRowNum = -1)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("ExcelHelperV2", "ExcelHelperV2 is disposed");
            }

            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                ISheet sheet = workbook.GetSheetAt(0);

                if (sheet != null)
                {

                    startRow = firstRowNum;
                    //最后一列的标号
                    int rowCount = lastRowNum >= 0 ? lastRowNum : sheet.LastRowNum;

                    IRow firstRow = sheet.GetRow(startRow);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        //创建表结构
                        DataColumn column = new DataColumn();
                        data.Columns.Add(column);
                    }

                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// DataRow行内是否有数据
        /// </summary>
        /// <param name="row">行对象</param>
        /// <param name="itemCount">需要检验的列(从第一列到itemcount列)</param>
        /// <returns>true为无数据，false有数据</returns>
        public static bool RowDontHasValue(DataRow row, int itemCount = 0)
        {
            bool result = true;
            for (int i = 0; i <= itemCount; i++)
            {
                if (!string.IsNullOrEmpty(row[i].ToString().Trim(' ')))
                    result = false;
            }
            return result;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                // free managed resources
                if (this.workbook != null)
                {
                    this.workbook = null;
                }
            }
            disposed = true;
        }

        ~ExcelHelperV2()
        {
            Dispose(false);
        }
    }
}
