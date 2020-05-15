using ICSharpCode.SharpZipLib.Zip;
//using Runda.B2B.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ThinkDev.Logging;

namespace Framework
{
    public class FileHelper
    {
        /// <summary>
        /// 将一组文件打包成一个ZIP文件，并返回相对地址
        /// </summary>
        /// <param name="lstFileZipInfo"></param>
        /// <returns></returns>
        public static string CreateZipFile(List<FileZipInfo> lstFileZipInfo, out bool hasError)
        {
            hasError = false;
            if (lstFileZipInfo != null && lstFileZipInfo.Count > 0)
            {
                string zipFilePath = ConfigHelper.LocalZipSavePath + Guid.NewGuid().ToString() + ".zip";//ZIP文件保存路径
                string zipFileDownloadUrl = "http://" + ConfigHelper.MainDomain + zipFilePath;//ZIP文件下载地址
                zipFileDownloadUrl = zipFileDownloadUrl.Replace("\\", "/");

                zipFilePath = HttpContext.Current.Server.MapPath(zipFilePath);//将保存地址转换为绝对路径

                try
                {
                    using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFilePath)))
                    {
                        s.SetLevel(0); // 压缩级别 0-9  // 0 - store only to 9 - means best compression
                        //s.Password = "123"; //Zip压缩文件密码
                        byte[] buffer = new byte[4096]; //缓冲区大小
                        foreach (FileZipInfo fileZipInfo in lstFileZipInfo)
                        {
                            ZipEntry entry = new ZipEntry(Path.GetFileName(fileZipInfo.FileName));
                            entry.DateTime = DateTime.Now;
                            s.PutNextEntry(entry);
                            using (FileStream fs = File.OpenRead(fileZipInfo.FilePath))
                            {
                                int sourceBytes;
                                do
                                {
                                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                    s.Write(buffer, 0, sourceBytes);
                                } while (sourceBytes > 0);
                            }
                        }
                        s.Finish();
                        s.Close();
                    }
                    return zipFileDownloadUrl;
                }
                catch (Exception ex)
                {
                    hasError = true;
                    LogManager.DefaultLogger.Error(LogConvert.ParseWebEx(ex), "CreateZipFile 发生异常！");
                    return ex.Message;
                }
            }
            hasError = true;
            return "";
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="zipFilePath"></param>
        private static string UnZipFile(string zipFilePath, out bool hasError)
        {
            hasError = false;
            if (File.Exists(zipFilePath))
            {
                try
                {
                    using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                    {
                        ZipEntry theEntry;
                        while ((theEntry = s.GetNextEntry()) != null)
                        {
                            Console.WriteLine(theEntry.Name);
                            string directoryName = Path.GetDirectoryName(theEntry.Name);
                            string fileName = Path.GetFileName(theEntry.Name);
                            // create directory
                            if (directoryName.Length > 0)
                            {
                                Directory.CreateDirectory(directoryName);
                            }

                            if (fileName != String.Empty)
                            {
                                using (FileStream streamWriter = File.Create(theEntry.Name))
                                {
                                    int size = 2048;
                                    byte[] data = new byte[2048];
                                    while (true)
                                    {
                                        size = s.Read(data, 0, data.Length);
                                        if (size > 0)
                                        {
                                            streamWriter.Write(data, 0, size);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        return "";
                    }
                }
                catch (Exception ex)
                {
                    hasError = true;
                    LogManager.DefaultLogger.Error(LogConvert.ParseWebEx(ex), "UnZipFile 发生异常！");
                    return ex.Message;
                }
            }
            hasError = true;
            return string.Format("Cannot find file '{0}'", zipFilePath);
        }


    }

    /// <summary>
    /// 文件打包必备信息
    /// </summary>
    [Serializable]
    public class FileZipInfo
    {
        /// <summary>
        /// 最后生成的文件的文件名
        /// </summary>
        public string FileName { set; get; }

        /// <summary>
        /// 文件保存路径
        /// </summary>
        public string FilePath { set; get; }

        /// <summary>
        /// 物料号
        /// </summary>
        public string InvCode { set; get; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string InvName { set; get; }
    }
}
