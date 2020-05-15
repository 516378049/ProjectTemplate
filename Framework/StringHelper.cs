using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Text.RegularExpressions;
//using Runda.B2B.Models;
using Microsoft.International.Converters.PinYinConverter;

namespace Framework
{
    public class StringHelper
    {

        /// <summary>
        /// 获取静态文件地址（用于动静分离）V2
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetStaticFileV2(string file)
        {
            var str = ConfigHelper.StaticFileV2;
            UrlHelper he = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
            return he.Content(str + file);
        }

        /// <summary>
        /// 获取静态文件地址（用于动静分离）
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetStaticFile(string file)
        {
            string str = ConfigHelper.StaticFile;
            UrlHelper he = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
            string versionStr = string.Concat("?v=", ConfigHelper.StaticFileVersion);
            return he.Content(str + file + versionStr);
        }

        #region 验证一个字符串是否为一个数字
        /// <summary>
        /// 验证一个字符串是否为一个数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumberic(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
            return reg.IsMatch(str);
        }
        #endregion

        #region 验证一个字符串是否为数字或字母
        /// <summary>
        /// 验证一个字符串是否为数字或字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// Modify:2016-12-05 Darin -添加-和_
        public static bool IsNumbericOrLetter(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[^0-9a-zA-Z\-_]");
            return !reg.IsMatch(str);
        }
        #endregion

        #region 判断一个字符串是否为合法整数(不限制长度)
        /// <summary>
        /// 判断一个字符串是否为合法整数(不限制长度)
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool IsInteger(string s)
        {
            decimal d = 0m;
            if (decimal.TryParse(s, out d))
            {
                if (d % 1 == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
            //string pattern = @"^\d*$";
            //return Regex.IsMatch(s, pattern);
        }
        #endregion

        #region 格式化显示数字
        public static string FormatNumber(object num)
        {
            string n = num.ToString();
            if (IsInteger(n))
            {
                decimal dTmp = decimal.Parse(n);
                return string.Format("{0:d}", (int)dTmp);
            }
            else if (IsNumberic(n))
            {
                decimal dTmp = decimal.Parse(n);
                return string.Format("{0:f2}", dTmp);
            }
            else
            {
                return n;
            }
        }
        #endregion

        /// <summary>
        /// 格式化Decimal(默认保留两位小数)
        /// </summary>
        /// <param name="d"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string FormatDecimal(decimal? d, string format = "0.00")
        {
            if (d != null)
            {
                return d.Value.ToString(format);
            }
            else
            {
                return "0.00";
            }
        }

        #region 验证输入是否为手机号码
        /// <summary>
        /// 验证输入是否为手机号码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobileNumber(string mobile)
        {
            if (mobile.Length != 11)
            {
                return false;
            }

            if (!IsNumberic(mobile))
            {
                return false;
            }

            long tmp = long.Parse(mobile);
            if (tmp < 13000000000 || tmp > 19000000000)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 验证输入是否为邮编
        public static bool IsPostCode(string postcode)
        {
            return Regex.IsMatch(postcode, @"\d{6}");
        }
        #endregion

        #region 验证输入是否为Email
        /// <summary>
        /// 验证输入是否为Email
        /// </summary>
        /// <param name="str_Email"></param>
        /// <returns></returns>
        public static bool IsEmail(string str_Email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_Email, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }
        #endregion

        #region 为用户帐号添加掩码
        /// <summary>
        /// 为用户帐号添加掩码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static string PassAccountDisplay(string account)
        {
            string ret = account;
            if (IsEmail(account))
            {
                string preStr = account.Substring(0, account.IndexOf('@'));
                string suffStr = account.Substring(account.IndexOf('@') + 1);
                if (preStr.Length <= 3)
                {
                    preStr = preStr.Substring(0, 1) + "***" + preStr.Substring(preStr.Length - 1);
                }
                else
                {
                    preStr = preStr.Substring(0, 3) + "***" + preStr.Substring(preStr.Length - 1);
                }
                ret = preStr + "@" + suffStr;
            }
            else if (IsMobileNumber(account))
            {
                ret = account.Substring(0, 3) + "***" + account.Substring(account.Length - 4);
            }
            else
            {
                if (account.Length <= 3)
                {
                    ret = ret.Substring(0, 1) + "***" + ret.Substring(ret.Length - 1);
                }
                else
                {
                    ret = ret.Substring(0, 3) + "***" + ret.Substring(ret.Length - 1);
                }
            }
            return ret;
        }
        #endregion

        #region 转换用户类型
        /// <summary>
        /// 转换用户类型
        /// </summary>
        /// <param name="userClass"></param>
        /// <returns></returns>
        public static string ConvertUserClass(int userClass)
        {
            switch (userClass)
            {
                case 1: return "管理员";
                case 2: return "普通成员";
                default: return "";
            }
        }
        #endregion

        #region 转换合约类型
        /// <summary>
        /// 转换合约类型
        /// </summary>
        /// <param name="userClass"></param>
        /// <returns></returns>
        public static string ConvertContactStatus(int contactStatus)
        {
            switch (contactStatus)
            {
                case 0: return "未生效";
                case 1: return "已上架";
                case 2: return "已下架";
                default: return "";
            }
        }
        #endregion

        #region 转换公司证件
        public static string ConvertCertificateName(int id)
        {
            Dictionary<int, string> templates = ConfigHelper.CertifcateReg;
            return templates[id];
        }

        public static string ConvertCertificateStatus(int cipStatus)
        {
            switch (cipStatus)
            {
                case 0: return "未审核";
                case 1: return "已通过审核";
                case 2: return "已过期";
                case 3: return "审核拒绝";
                default: return "";
            }
        }
        #endregion

        #region 转换是否公开售卖
        public static string ConvertMaterialIsOpen(int isOpen)
        {
            switch (isOpen)
            {
                case 0: return "否";
                case 1: return "是";
                case 2: return "下架";
                default: return "";
            }
        }
        #endregion

        #region 转换公司类型
        public static string ConvertCompanyType(int companytype)
        {
            switch (companytype)
            {
                case 0: return "无";
                case 1: return "医院实验室";
                case 2: return "厂商";
                case 4: return "经销商";
                default: return "";
            }
        }
        #endregion

        #region 转换公司状态
        public static string ConvertCompanyStatus(int status)
        {
            switch (status)
            {
                case 0: return "未审核";
                case 1: return "正常";
                case 2: return "被屏蔽";
                default: return "";
            }
        }
        #endregion

        #region 转换供应商物料库存状态
        public static string ConvertMaterialCertificateName(int id)
        {
            Dictionary<int, string> templates = ConfigHelper.MaterialCertificateType;
            return templates[id];
        }
        /// <summary>
        /// 转换供应商物料库存状态：从数字到字符
        /// </summary>
        /// <param name="materialStatus"></param>
        /// <returns></returns>
        public static string ConvertMaterialStatus(int materialStatus)
        {
            //供应商物料库存状态：0-未添加物料；1-未上传证书；2-证书审核未通过；3-证书审核中；4-证书审核已通过\n未设置批号效期；5-证书审核已通过\n已设置批号效期\n未设置库存；6-证书审核已通过\n已设置批号效期\n已设置库存；
            switch (materialStatus)
            {
                case 0: return "未添加物料";
                case 1: return "未上传证书";
                case 2: return "证书审核未通过";
                case 3: return "证书审核中";
                case 4: return "证书审核已通过";
                default: return "";
            }
        }
        public static string ConvertMaterialCertificateStatus(int mcStatus)
        {
            switch (mcStatus)
            {
                case 0: return "未审核";
                case 1: return "审核已通过";
                case 2: return "审核未通过";
                default: return "";
            }
        }
        #endregion

        #region 转换送货时间
        /// <summary>
        /// 转换送货时间
        /// </summary>
        /// <param name="sendTime"></param>
        /// <returns></returns>
        public static string ConvertSendTimeType(int sendTime)
        {
            switch (sendTime)
            {
                case 1: return "工作日送货";
                case 2: return "双休日送货";
                case 3: return "全天送货";
            }
            return "";
        }
        #endregion

        #region 站内信类型
        public static string ConvertMessageType(int messagetype)
        {
            switch (messagetype)
            {
                case 0: return "普通消息";
                case 1: return "系统消息";
                default: return "";
            }
        }
        #endregion

        #region 转换物料库存操作类型
        public static string ConvertStockLogOperationType(int operationType)
        {
            switch (operationType)
            {
                case 1: return "出库";
                case 2: return "入库";
                case 3: return "追加";
                default: return "";
            }
        }
        #endregion

        #region 转换菜单类型
        public static string ConvertPermissionType(int type)
        {
            switch (type)
            {
                case 0: return "菜单";
                case 1: return "按钮";
                default: return "";
            }
        }
        #endregion

        #region 转换PR状态
        public static string ConvertOrderPurchaseStatus(int status)
        {
            switch (status)
            {
                case 0: return "未处理";
                case 1: return "已处理";
                case 2: return "已隐藏";
                case 3: return "已退回";
                default:
                    return string.Empty;
            }
        }
        #endregion

        #region 转换交货单状态 弃用2018/12/20
        ///// <summary>
        ///// 转换交货单状态
        ///// </summary>
        ///// <param name="status">出库单状态：0-未处理；1-预出库；2-配送中；3-已出库；4-已签收</param>
        ///// <param name="ConfirmStatus">交货单确认状态：0-未确认；1-已确认</param>
        ///// <returns></returns>
        //public static string ConvertDeliveryStatus(OrderDeliveryInfo orderDeliveryInfo)
        //{
        //    string b2bStatus = "";
        //    int status = orderDeliveryInfo.Status;
        //    int confirmStatus = orderDeliveryInfo.ConfirmStatus;
        //    switch (status)
        //    {
        //        case 0:
        //            switch (confirmStatus)
        //            {
        //                case 0: b2bStatus = "等待反馈确认"; break;
        //                case 1: b2bStatus = "已确认未出库"; break;
        //                case 2: b2bStatus = "反馈确认拒绝"; break;
        //                default: b2bStatus = "反馈确认异常"; break;
        //            }
        //            break;
        //        case 1: b2bStatus = "预出库"; break;
        //        case 2: b2bStatus = "配送中"; break;
        //        case 3: b2bStatus = "已出库"; break;
        //        case 4: b2bStatus = "已签收"; break;
        //    }

        //    //注意：修改相关代码参数，请同步修改存储过程：SP_OrderDeliveryInfo_QueryList
        //    if (!string.IsNullOrEmpty(orderDeliveryInfo.SapDeliveryID))
        //    {
        //        //如果有SAP交货单号，则判断是否使用WMS发货
        //        if (ConfigHelper.WMSCompanyID.Contains("," + orderDeliveryInfo.SAPSellCompanyID + ","))
        //        {
        //            //是否是非代理运营的订单
        //            if (orderDeliveryInfo.OperationCompanyID == -1 || orderDeliveryInfo.SellCompanyID == orderDeliveryInfo.OperationCompanyID)
        //            {
        //                //直接取WMS状态
        //                return orderDeliveryInfo.SOStatusString;
        //            }
        //        }
        //    }
        //    return b2bStatus;
        //}
        #endregion

        #region 转换物料照片状态
        public static string ConvertMaterialPictureStatus(int status)
        {
            switch (status)
            {
                case 0: return "未审核";
                case 1: return "已审核";
                case 2: return "已拒绝";
                default: return "";
            }
        }
        #endregion

        #region 格式化物料名称
        public static string ConvertInvName(string invName)
        {
            string outName = invName;
            if (!String.IsNullOrWhiteSpace(invName))
            {
                if (invName.IndexOf("\\") == -1)
                {
                    if (invName.IndexOf("/") > -1)
                    {
                        if (invName.IndexOf("/") != invName.LastIndexOf("/"))
                        {
                            outName = invName.Substring(0, invName.IndexOf("/"));
                        }
                    }
                }
                else
                {
                    if (invName.IndexOf("\\") != invName.LastIndexOf("\\"))
                    {
                        outName = invName.Substring(0, invName.IndexOf("\\"));
                    }
                }
            }
            return outName;
        }
        #endregion

        #region 转换SIMS供应商
        public static string ConvertVendor(string inVendor)
        {
            string outVendor = inVendor == null ? "" : inVendor.Trim();
            if (!String.IsNullOrWhiteSpace(inVendor))
            {
                if (inVendor.Equals("SG01"))
                {
                    outVendor = "1010";
                }
                else if (inVendor.Equals("SG02"))
                {
                    outVendor = "1020";
                }
                //else
                //{
                //    outVendor = "1010";
                //}
            }
            return outVendor;
        }
        #endregion

        #region 交货单下载状态
        public static string ConvertOrderDeliveryDownloadStatus(int isdownload)
        {
            switch (isdownload)
            {
                case 0: return "未下载";
                case 1: return "已下载";
                default: return "";
            }
        }
        #endregion

        #region SAP销售订单上传状态
        public static string ConvertOrderSapUploadFlag(int uploadFlag)
        {
            switch (uploadFlag)
            {
                case 0: return "未上传";
                case 1: return "未上传";
                case 2: return "上传成功";
                case 3: return "上传失败";
                default: return "";
            }
        }
        #endregion

        #region SIMS 储位十进制转汉字
        public static string Convertsims(string str)
        {
            string result = string.Empty;
            string[] strArray = str.Split(new string[] { @"u" }, StringSplitOptions.None);
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].Trim() == "" || strArray[i].Length < 2 || strArray.Length <= 1)
                {
                    result += i == 0 ? strArray[i] : @"u" + strArray[i]; continue;
                }
                for (int j = strArray[i].Length > 4 ? 4 : strArray[i].Length; j >= 2; j--)
                {
                    try
                    {
                        result += char.ConvertFromUtf32(Convert.ToInt32(strArray[i].Substring(0, j), 16)) + strArray[i].Substring(j);
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return result;
        }
        #endregion

        #region SIMS 储位十进制转汉字
        public static string ConvertsimscwToGB(string str0)
        {
            if (string.IsNullOrEmpty(str0))
            {
                return "";
            }
            string result = string.Empty;
            string[] array = str0.Split(',');
            for (int m = 0; m < array.Length; m++)
            {
                int i0 = Convert.ToInt32(array[m]);
                string str = @"u" + Convert.ToString(Convert.ToInt32(array[m]), 16);
                if (i0 < 0)
                {
                    str = str.Replace("ffff", "");
                }
                string[] strArray = str.Split(new string[] { @"u" }, StringSplitOptions.None);
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].Trim() == "" || strArray[i].Length < 2 || strArray.Length <= 1)
                    {
                        result += i == 0 ? strArray[i] : @"u" + strArray[i]; continue;
                    }
                    for (int j = strArray[i].Length > 4 ? 4 : strArray[i].Length; j >= 2; j--)
                    {
                        try
                        {
                            result += char.ConvertFromUtf32(Convert.ToInt32(strArray[i].Substring(0, j), 16)) + strArray[i].Substring(j);
                            break;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        #region 转换打打印文档的类型
        public static int ConvertCloudPrintDocType(string type)
        {
            switch (type)
            {
                case null:
                    return 0;
                case "交货单":
                    return 1;
                case "出库标签":
                    return 2;
                case "注册证":
                    return 3;
                case "订单":
                    return 4;
                case "五联单":
                    return 5;
                default:
                    return -1;
            }
        }
        #endregion

        #region 转换退换货状态
        public static string ConvertBarterStatus(int status)
        {
            switch (status)
            {
                case 0: return "未处理";
                case 1: return "已处理";
                case 2: return "用户撤销";
                default: return "";
            }
        }
        #endregion

        #region 按照指定的分隔符和最大长队拆分字符串
        /// <summary>
        /// 按照指定的分隔符和最大长队拆分字符串
        /// </summary>
        /// <param name="strOld"></param>
        /// <param name="split"></param>
        /// <param name="maxlength"></param>
        /// <returns></returns>
        public static List<string> GetStringList(string strOld = "", char split = '|', int maxlength = 4000)
        {
            List<string> lstString = new List<string>();

            if (strOld.Length <= maxlength)
            {
                lstString.Add(strOld);
            }
            else
            {
                string strFirst = strOld.Substring(0, maxlength);
                int spiltIndex = strFirst.LastIndexOf(split);
                if (spiltIndex > 0)
                {
                    strFirst = strFirst.Substring(0, spiltIndex);
                    lstString.Add(strFirst.TrimEnd(split));
                    strOld = strOld.Substring(spiltIndex + 1);
                    lstString.AddRange(GetStringList(strOld, split, maxlength));
                }
                else
                {
                    lstString.Add(strFirst.TrimEnd(split));
                    strOld = strOld.Substring(maxlength + 1);
                    lstString.AddRange(GetStringList(strOld, split, maxlength));
                }
            }
            return lstString;
        }
        #endregion

        #region 获取文件名字符串内的扩展名
        /// <summary>
        /// 获取文件名字符串内的扩展名
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetFileExtName(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return "";
            string fileExtName = filename.Substring(filename.LastIndexOf(".") + 1, (filename.Length - filename.LastIndexOf(".") - 1));
            return fileExtName;
        }
        #endregion

        #region 将字符串转换为拼音（只取首字母）
        public static string ConvertToPY(string source)
        {
            char[] arrString = source.ToCharArray();
            string pyFinal = "";
            if (arrString.Length > 0)
            {
                for (int i = 0; i < arrString.Length; i++)
                {
                    try
                    {
                        //获取某个中文字符的拼音
                        //先判断是否是英文字母或者数字
                        System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
                        if (reg1.IsMatch(arrString[i].ToString()))
                        {
                            pyFinal += arrString[i].ToString().ToLower();
                            continue;
                        }
                        ChineseChar chn = new ChineseChar(arrString[i]);
                        //取开头字母
                        string py = chn.Pinyins[0].Substring(0, 1);
                        pyFinal += py;
                    }
                    catch (Exception ex)
                    {
                        //如果不是简体中文，则忽略
                    }
                }
            }
            return pyFinal;
        }
        #endregion

        #region 移除脚本代码（Script标签、on事件属性、href属性）
        /// <summary>
        /// 移除脚本代码（Script标签和on事件属性）
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveScript(string html)
        {
            html = Regex.Replace(html, @"(<script\s*.*>\s*.+?\/script\s*>|<\s*script\s*.+?/>)", "");//过滤script标签
            html = Regex.Replace(html, "(\\son\\w+\\s*=\\s*\\\".+?[^\\\\]\\\")|(\\son\\w+\\s*=\\s*'.+?[^\\\\]')", "");//过滤on事件属性           
            html = Regex.Replace(html, "(\\shref\\s*=\\s*\\\".+?[^\\\\]\\\")|(\\shref\\s*=\\s*'.+?[^\\\\]')", "");//过滤href属性           
            return html;
        }
        #endregion

        #region HTML常用字符转义处理
        /// <summary>
        /// HTML常用字符转义处理
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlFilter(string html)
        {
            html = html.Replace("&", "&amp;");
            html = html.Replace("<", "&lt;");
            html = html.Replace(">", "&gt;");
            html = html.Replace("\"", "&quot;");
            html = html.Replace("'", "&apos;");
            return html;
        }
        #endregion

        #region HTML常用字符反转义处理
        /// <summary>
        /// HTML常用字符反转义处理
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlRecover(string html)
        {
            html = html.Replace("&lt;", "<");
            html = html.Replace("&gt;", ">");
            html = html.Replace("&quot;", "\"");
            html = html.Replace("&apos;", "'");
            html = html.Replace("&amp;", "&");
            return html;
        }
        #endregion

        #region 移除HTML标签
        /// <summary>
        /// 移除HTML标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveHtml(string html)
        {
            return Regex.Replace(html, @"<.+?>", "").Replace("&nbsp;", "");
        }
        #endregion

        #region 获取内容摘要
        /// <summary>
        /// 获取内容摘要
        /// </summary>
        /// <param name="text"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GetSummary(string text, int len)
        {
            text = text.Replace("<br>", "\n");
            len -= 1;
            if (text.Length <= len)
            {
                return text + "...";
            }
            else
            {
                return text.Substring(0, len) + "...";
            }
        }
        #endregion

        #region 转换换行符
        /// <summary>
        /// 转换换行符
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ConvertNewLineToHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            return text.Replace("\n", "<br>").Replace("\r\n", "<br>");
        }
        #endregion

        #region 根据SAP MWSKZ值返回税率
        /// <summary>
        /// 根据SAP MWSKZ值返回税率
        /// </summary>
        /// <param name="sapMWSKZ"></param>
        /// <returns></returns>
        public static decimal ConvertSapTaxRate(string sapMWSKZ)
        {
            switch (sapMWSKZ)
            {
                case "J0":
                    return 0;
                case "J1":
                    return 0.17M;
                case "J2":
                    return 0.13M;
                case "J3":
                    return 0.11M;
                case "J4":
                    return 0.07M;
                case "J5":
                    return 0.06M;
                case "J6":
                    return 0.03M;
                case "J7":
                    return 0.05M;
                case "J8":
                    return 0.015M;
                case "J9":
                    return 0.16M;
                case "JA":
                    return 0.10M;
                case "L1":
                    return 0.05M;
                case "X0":
                    return 0;
                case "X1":
                    return 0.17M;
                case "X2":
                    return 0.06M;
                case "X3":
                    return 0.03M;
                case "X5":
                    return 0.11M;
                case "X9":
                    return 0.16M;
                case "XA":
                    return 0.10M;
                default:
                    return 0;
            }
        }
        #endregion

        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string FormatDateTime(DateTime? dt, string format = "yyyy-MM-dd")
        {
            if (dt == null)
            {
                return string.Empty;
            }
            else
            {
                if (dt.Value == DateTime.MinValue)  //最小值时返回空
                {
                    return string.Empty;
                }

                return dt.Value.ToString(format);
            }
        }

        /// <summary>
        /// 指定的SAPCompanyID是否适用SAP采购对接功能
        /// </summary>
        /// <param name="sapCompanyID"></param>
        /// <returns></returns>
        public static bool IsPurchaseSAP(string sapCompanyID)
        {
            bool flag = false;
            string purchaseSAP = ConfigHelper.PurchaseSAP;

            if (!string.IsNullOrEmpty(purchaseSAP))
            {
                string[] arr = purchaseSAP.Split(',');

                if (arr != null && arr.Length > 0)
                {
                    foreach (string str in arr)
                    {
                        if (string.IsNullOrEmpty(str))
                        {
                            continue;
                        }

                        if (str == sapCompanyID)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// 转换人民币大小金额
        /// </summary>
        /// <param name="num">金额</param>
        /// <returns>返回大写形式</returns>
        public static string CmycurD(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字
            string str3 = "";    //从原num值中取出的值
            string str4 = "";    //数字的字符串形式
            string str5 = "";  //人民币大写金额形式
            int i;    //循环变量
            int j;    //num的值乘以100的字符串长度
            string ch1 = "";    //数字的汉语读法
            string ch2 = "";    //数字位的汉字读法
            int nzero = 0;  //用来计算连续的零值是几个
            int temp;            //从原num值中取出的值

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式
            j = str4.Length;      //找出最高位
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分

            //循环取出每一位需要转换的值
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值
                temp = Convert.ToInt32(str3);      //转换为数字
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整”
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }

        /// <summary>
        /// 获取16位唯一字符串
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueNo()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 物料注册证文件外网地址
        /// </summary>
        /// <param name="certificatePath"></param>
        /// <returns></returns>
        public static string FormatMaterialCertificatePath(string certificatePath)
        {
            if (string.IsNullOrEmpty(certificatePath))
            {
                return certificatePath;
            }

            if (certificatePath.StartsWith("^", StringComparison.OrdinalIgnoreCase))
            {
                return certificatePath;
            }
            if (!certificatePath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                if (certificatePath.StartsWith("UploadImages", StringComparison.OrdinalIgnoreCase) ||
                    certificatePath.StartsWith("UploadFiles", StringComparison.OrdinalIgnoreCase) ||
                    certificatePath.StartsWith("/UploadImages", StringComparison.OrdinalIgnoreCase) ||
                    certificatePath.StartsWith("/UploadFiles", StringComparison.OrdinalIgnoreCase))
                {
                    certificatePath = ConfigHelper.StaticUrl + certificatePath.Replace("//", "/");
                }
                else
                {
                    certificatePath = ConfigHelper.StaticUrl + "UploadFiles/" + certificatePath;
                }
                //certificatePath = certificatePath.Replace("//", "/");     xuqing  2017.12.4 注释
            }


            return certificatePath;
        }

        /// <summary>
        /// 物料注册证文件本地地址
        /// </summary>
        /// <param name="certificatePath"></param>
        /// <returns></returns>
        public static string FormatMaterialCertificateLocalPath(string certificatePath)
        {
            if (string.IsNullOrEmpty(certificatePath))
            {
                return certificatePath;
            }

            certificatePath = certificatePath.Replace(ConfigHelper.StaticUrl, "");

            if (certificatePath.IndexOf("webupload", StringComparison.OrdinalIgnoreCase) > -1)
            {
                certificatePath = certificatePath.Substring(certificatePath.IndexOf("webupload",
                    StringComparison.OrdinalIgnoreCase));
            }

            if (certificatePath.StartsWith("UploadImages", StringComparison.OrdinalIgnoreCase) ||
                certificatePath.StartsWith("UploadFiles", StringComparison.OrdinalIgnoreCase))
            {
                certificatePath = ConfigHelper.LocalUploadPath + certificatePath;
            }
            else if (certificatePath.IndexOf("webupload", StringComparison.OrdinalIgnoreCase) > -1)
            {
                certificatePath = ConfigHelper.LocalUploadPath + certificatePath;
            }
            else
            {
                certificatePath = ConfigHelper.LocalUploadPath + "UploadFiles/" + certificatePath;
            }

            return certificatePath.Replace("/", "\\");
        }

        /// <summary>
        /// 格式化长文本
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static string StringSubstrLen(string str, int len)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(str);
            int n = 0;  //  表示当前的字节数
            int i = 0;  //  要截取的字节数
            for (; i < bytes.GetLength(0) && n < len; i++)
            {
                //  偶数位置，如0、2、4等，为UCS2编码中两个字节的第一个字节

                if (i % 2 == 0)
                {
                    n++;      //  在UCS2第一个字节时n加1
                }
                else
                {
                    //  当UCS2编码的第二个字节大于0时，该UCS2字符为汉字，一个汉字算两个字节
                    if (bytes[i] > 0)
                    {
                        n++;
                    }
                }
            }

            //  如果i为奇数时，处理成偶数
            if (i % 2 == 1)
            {
                //  该UCS2字符是汉字时，去掉这个截一半的汉字
                if (bytes[i] > 0)
                {
                    i = i - 1;
                }
                //  该UCS2字符是字母或数字，则保留该字符
                else
                {
                    i = i + 1;
                }
            }

            string result = System.Text.Encoding.Unicode.GetString(bytes, 0, i);

            if (result.Length < str.Length)
            {
                return result + "...";
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 指定的SAP公司是否使用WMS
        /// </summary>
        /// <param name="sapCompanyID"></param>
        /// <returns></returns>
        public static bool IsWMSCompanyID(string sapCompanyID)
        {
            bool flag = false;
            string[] arrWMSCompanyID = ConfigHelper.WMSCompanyID.Split(',');

            foreach (string wmsCompanyID in arrWMSCompanyID)
            {
                if (string.IsNullOrEmpty(wmsCompanyID))
                {
                    continue;
                }

                if (wmsCompanyID == sapCompanyID)
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <param name="encode">加密采用的编码方式</param>
        /// <returns></returns>
        public static string EncodeBase64(string source, Encoding encode)
        {
            string result = string.Empty;
            byte[] bytes = encode.GetBytes(source);
            try
            {
                result = Convert.ToBase64String(bytes);
            }
            catch
            {
                result = source;
            }
            return result;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(source, Encoding.UTF8);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <param name="encode">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result, Encoding encode)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(result, Encoding.UTF8);
        }

        #region 数字与字母转换
        /// <summary>
        /// 转换两位年月为字母：09->9；10->A；35->Z
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertYearOrMonthToLetter(string str)
        {
            //如果不是字母或数字，返回00
            if (!IsNumberic(str))
            {
                return "0";
            }

            int num = Convert.ToInt16(str);

            if (num < 10)
            {
                str = num.ToString();
            }
            else
            {
                if (num > 35)
                {
                    return "0";
                }

                byte[] array = new byte[1];
                array[0] = (byte)(Convert.ToInt32(num + 55)); //ASCII码强制转换二进制
                str = Convert.ToString(System.Text.Encoding.ASCII.GetString(array));
            }
            return str;
        }

        /// <summary>
        /// 转换字母为两位年月：9->09；A->10；Z->35
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertLetterToYearOrMonth(string str)
        {
            //如果不是字母或数字，返回00
            if (!IsNumbericOrLetter(str))
            {
                return "00";
            }

            if (IsNumberic(str))
            {
                str = "0" + str;
            }
            else
            {
                byte[] array = new byte[1];
                array = System.Text.Encoding.ASCII.GetBytes(str);
                int asciicode = (short)(array[0]);
                str = Convert.ToString(asciicode - 55);
            }
            return str;
        }
        #endregion

        #region 生成随机字符串
        ///<summary>
        ///生成随机字符串
        ///</summary>
        ///<param name="length">目标字符串的长度，默认8位</param>
        ///<param name="removeChar">要移除的字符</param>
        ///<param name="useNum">是否包含数字，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRnd(int length = 8, string removeChar = "", bool useNum = true, bool useLow = true, bool useUpp = true, bool useSpe = false, string custom = "")
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }

            //移除字符
            if (!string.IsNullOrWhiteSpace(removeChar))
            {
                str = Regex.Replace(str, "[" + removeChar + "]", "");
            }

            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        #endregion
    }
}
