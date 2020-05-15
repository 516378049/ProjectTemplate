using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
  public  class WaterImageHelper
    {
        /// <summary>
        /// 图片水印
        /// </summary>
        /// <param name="_ImageBytes">服务器图片相对路径</param>
        /// <param name="watermarkFilename">水印文件相对路径</param>
        /// <param name="watermarkStatus">图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下</param>
        /// <param name="quality">附加水印图片质量,0-100</param>
        /// <param name="watermarkTransparency">水印的透明度 1--10 10为不透明</param>
        public static byte[] AddImageSignPic(Byte[] _ImageBytes, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
        {

            //获取上传图片
            Image img = Image.FromStream(new System.IO.MemoryStream(_ImageBytes));
            //Bitmap bitmap = new Bitmap(img);
            //bitmap =  GetSmall(bitmap,1.5);
            //Graphics g = Graphics.FromImage(bitmap);
            Graphics g = Graphics.FromImage(img);

            //设置高质量插值法
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
          
            //获取水印图片
            watermarkFilename = watermarkFilename.Replace("\\","/");         
            HttpWebRequest webreq = WebRequest.CreateHttp(watermarkFilename);
            webreq.Method = "Get";
            var  webres = webreq.GetResponse() as HttpWebResponse;
            Stream stream = webres.GetResponseStream();
            Image watermark = Image.FromStream(stream);
            stream.Close();


            //Image watermark = new Bitmap(watermarkFilename);
            //判断水印图片是否大于上传图片
            //if (watermark.Height >= img.Height || watermark.Width >= img.Width)
            //    return null;

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
                transparency = (watermarkTransparency / 10.0F);


            float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 2:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 3:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 4:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 5:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 6:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 7:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 8:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 9:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
            }

            g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                    ici = codec;
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
                quality = 80;

            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;
            //filename = "H:/B2B/trunk/Runda.B2B.Web.MainWeb/RundaStatic/TemWater/111.jpg";

            //if (ici != null)
            //    img.Save(filename, ici, encoderParams);
            //else
            //    img.Save(filename);

            byte[] newImage = ImageToBytes(img);
            //byte[] newImage = Bitmap2Byte(bitmap);

            g.Dispose();
            img.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();

            return newImage;
        }
        
        /// <summary>
        /// PDF加图片水印
        /// </summary>
        /// <param name="inputfilepath">源文件PDF</param>
        /// <param name="ModelPicName">水印图片</param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static byte[] PDFWatermark(byte[] inputfilepath, string ModelPicName, float top, float left, int waterRemarkType)
        {
            //throw new NotImplementedException();
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            string NewFile = Utils.GetMapPath("/RundaStatic/TemWater/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            try
            {
                pdfReader = new PdfReader(inputfilepath);

                int numberOfPages = pdfReader.NumberOfPages;

                iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);

                float width = psize.Width;

                float height = psize.Height;

                //FileStream fileStream = new FileStream(NewFile, FileMode.Create);

                pdfStamper = new PdfStamper(pdfReader, new FileStream(NewFile, FileMode.Create));

                PdfContentByte waterMarkContent;
                ModelPicName = ModelPicName.Replace("\\", "/");
                HttpWebRequest webreq = WebRequest.CreateHttp(ModelPicName);
                webreq.Method = "Get";
                var webres = webreq.GetResponse() as HttpWebResponse;
                Stream stream = webres.GetResponseStream();
                Image watermark = Image.FromStream(stream);
                stream.Close();
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(ModelPicName);

                image.GrayFill = 20;//透明度，灰色填充
                //image.Rotation//旋转
                //image.RotationDegrees//旋转角度


                //水印的位置 
                if (left < 0)
                {
                    left = width - image.Width + left;   //h////////////
                }

                //image.SetAbsolutePosition(left, (height - image.Height) - top);     //h////////////
          
                switch (waterRemarkType)//10-质检报告,11-注册在证，12-报关单
                {
                    case 10:
                        image.SetAbsolutePosition(430, 740);//质检报告
                        break;
                    case 12:
                        image.SetAbsolutePosition(700, 470);//报关单
                        break;
                    default:
                        image.SetAbsolutePosition(400, 550);//其他
                        break;
                }
                //image.ScaleAbsolute(155, 117);//自定义大小
                //image.ScalePercent(100);  //依照比例缩放
                image.ScaleAbsolute(100, 100);//自定义大小
                //每一页加水印,也可以设置某一页加水印 
                for (int i = 1; i <= numberOfPages; i++)
                {
                    // waterMarkContent = pdfStamper.GetUnderContent(i);
                    waterMarkContent = pdfStamper.GetOverContent(i);
                    waterMarkContent.AddImage(image);
                }
                //strMsg = "success";

                if (pdfStamper != null)
                    pdfStamper.Close();

                if (pdfReader != null)
                    pdfReader.Close();

                FileStream fs = new FileStream(NewFile, FileMode.Open, FileAccess.Read);

                byte[] buffur = new byte[fs.Length];
                fs.Read(buffur, 0, (int)fs.Length);
                fs.Close();

                return buffur;
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.Error(ex, "PDF加图片水印失败");

                return null;
            }
            finally
            {
                try
                {
                    File.Delete(NewFile);
                }
                catch (Exception ex1)
                {
                    LogManager.DefaultLogger.Error(ex1, string.Concat("删除PDF加图片水印临时文件失败.", NewFile));
                }
            }
        }
        
        /// <summary>
        /// Convert Image to Byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image)
            {
                ImageFormat format = image.RawFormat;
                using (MemoryStream ms = new MemoryStream())
                {
                    if (format.Equals(ImageFormat.Jpeg))
                    {
                        image.Save(ms, ImageFormat.Jpeg);
                    }
                    else if (format.Equals(ImageFormat.Png))
                    {
                        image.Save(ms, ImageFormat.Png);
                    }
                    else if (format.Equals(ImageFormat.Bmp))
                    {
                        image.Save(ms, ImageFormat.Bmp);
                    }
                    else if (format.Equals(ImageFormat.Gif))
                    {
                        image.Save(ms, ImageFormat.Gif);
                    }
                    else if (format.Equals(ImageFormat.Icon))
                    {
                        image.Save(ms, ImageFormat.Icon);
                    }
                    byte[] buffer = new byte[ms.Length];
                    //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
            }
        /// <summary>
        /// bitmap转byte
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }
        /// <summary>
        /// 获取缩小后的图片
        /// </summary>
        /// <param name="bm">要缩小的图片</param>
        /// <param name="times">要缩小的倍数</param>
        /// <returns></returns>
        private static Bitmap GetSmall(Bitmap bm, double times)
        {
            int nowWidth = (int)(bm.Width / times);
            int nowHeight = (int)(bm.Height / times);
            Bitmap newbm = new Bitmap(nowWidth, nowHeight);//新建一个放大后大小的图片

            if (times >= 1 && times <= 1.1)
            {
                newbm = bm;
            }
            else
            {
                Graphics g = Graphics.FromImage(newbm);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(bm, new Rectangle(0, 0, nowWidth, nowHeight), new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                g.Dispose();
            }
            return newbm;
        }

    }
}
