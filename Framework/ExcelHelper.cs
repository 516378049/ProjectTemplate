using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;

namespace Framework
{
    public class ExcelHelper : IDisposable
    {
        public List<HSSFWorkbook> WorkBookList { get; private set; }

        public ExcelHelper()
        {
            this.WorkBookList = new List<HSSFWorkbook>();
        }
        public ExcelHelper(string fileName)
        {
            this.WorkBookList = new List<HSSFWorkbook>();
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                this.WorkBookList.Add(new HSSFWorkbook(fs));
            }
        }

        public HSSFWorkbook OpenWorkBook(string fileName)
        {
            this.WorkBookList = new List<HSSFWorkbook>();
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(fs);
                WorkBookList.Add(workbook);
                return workbook;
            }
        }
        public HSSFWorkbook CreateWorkBook()
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            this.WorkBookList.Add(workbook);
            return workbook;
        }

        public void Save(HSSFWorkbook workbook, string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (FileStream file = new FileStream(fileName, FileMode.Create))
            {
                workbook.Write(file);
                file.Close();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (this.WorkBookList != null)
                {
                    this.WorkBookList = null;
                    GC.Collect();
                }
            }
        }
        ~ExcelHelper()
        {
            Dispose(false);
        }
    }
}
