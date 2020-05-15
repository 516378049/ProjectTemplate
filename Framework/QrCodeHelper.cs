using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.IO;
using System.Drawing;
using System.Web;

namespace Framework
{
    public class QrCodeHelper
    {
        /// <summary>
        /// 根据内容生成png格式的二维码，并保存到指定的物理路径中；
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="moduleSize">控制图片的大小：1，2，3，4，6，7，8</param>
        /// <param name="physicalpath">图片存放的物理路径</param>
        /// <returns>返回生成的图片名</returns>
        public static string CreateQrCode(string content, int moduleSize, string physicalpath)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }
            QrEncoder qrencoder = new QrEncoder(ErrorCorrectionLevel.L);
            Gma.QrCodeNet.Encoding.QrCode qrcode = new Gma.QrCodeNet.Encoding.QrCode();
            qrencoder.TryEncode(content, out qrcode);
            GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(moduleSize, QuietZoneModules.Zero), Brushes.Black, Brushes.White);
            using (MemoryStream ms = new MemoryStream())
            {
                render.WriteToStream(qrcode.Matrix, System.Drawing.Imaging.ImageFormat.Png, ms);
                Image image = Image.FromStream(ms);
                string filename = "JHD" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".png";
                image.Save(physicalpath + filename);
                return filename;
            }
        }
    }
}
