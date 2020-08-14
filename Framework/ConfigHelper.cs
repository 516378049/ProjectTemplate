using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Common.Extensions;
using System.Reflection;

namespace Framework
{
    public class ConfigHelper
    {

        /// <summary>
        /// 前端跨域地址
        /// </summary>
        public static string CrossDomainUrl = ConfigurationManager.AppSettings["CrossDomainUrl"];
        /// <summary>
        /// AppID
        /// </summary>
        public static string AppID = ConfigurationManager.AppSettings["AppID"];
        /// <summary>
        /// AppSecret
        /// </summary>
        public static string AppSecret = ConfigurationManager.AppSettings["AppSecret"];

        /// <summary>
        /// 微信普通accessToken
        /// </summary>
        public static string GeneralAccessToken = ConfigurationManager.AppSettings["GeneralAccessToken"];
        /// <summary>
        /// jsapiTicket
        /// </summary>
        public static string JsapiTicket = ConfigurationManager.AppSettings["JsapiTicket"];
        /// <summary>
        /// 微信订单查询
        /// </summary>
        public static string wxOrderQuery = ConfigurationManager.AppSettings["wxOrderQuery"];
        /// <summary>
        /// 系统版本
        /// </summary>
        public static string sysVersion = ConfigurationManager.AppSettings["sysVersion"];

        /// <summary>
        /// 系统版本
        /// </summary>
        public static string redisServer = ConfigurationManager.AppSettings["redisServer"];

        /// <summary>
        /// 系统版本
        /// </summary>
        public static string webHost = ConfigurationManager.AppSettings["webHost"];
        

        /// <summary>
        /// 是否启用公告
        /// </summary>
        public static bool AnnEnabled
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AnnEnabled"]))
                {
                    return false;
                }
                else
                {
                    bool bl;
                    if (Boolean.TryParse(ConfigurationManager.AppSettings["AnnEnabled"], out bl))
                    {
                        return bl;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static bool IsNewSell
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["IsNewSell"]))
                {
                    return false;
                }
                else
                {
                    bool bl;
                    if (Boolean.TryParse(ConfigurationManager.AppSettings["IsNewSell"], out bl))
                    {
                        return bl;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Solr服务器
        /// </summary>
        public static string SolrServer { get { return ConfigurationManager.AppSettings["SolrServer"]; } }
        /// <summary>
        /// Solr服务器2
        /// </summary>
        public static string SolrServer2 { get { return ConfigurationManager.AppSettings["SolrServer2"]; } }

        public static List<string> NewUploadSapCompanyIDs
        {
            get
            {
                string sapCompanyIds = ConfigurationManager.AppSettings["NewUploadSapCompanyIDs"];
                if (string.IsNullOrWhiteSpace(sapCompanyIds))
                {
                    return new List<string>();
                }
                return sapCompanyIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }

        #region 空军总院订单通知
        public static int KZ_SMSCompanyID { get { return Convert.ToInt32(ConfigurationManager.AppSettings["KZSMSCompanyID"]); } }
        public static string KZ_SMSSign { get { return ConfigurationManager.AppSettings["KZSMSSign"]; } }
        public static string KZ_SMSTemplate { get { return ConfigurationManager.AppSettings["KZSMSTemplate"]; } }
        #endregion

        #region 空总通知
        public static int KZ_NoticeCompanyID { get { return Convert.ToInt32(ConfigurationManager.AppSettings["KZNoticeCompanyID"]); } }
        public static string KZ_NoticeSign { get { return ConfigurationManager.AppSettings["KZNoticeSign"]; } }
        public static string KZ_NoticeTemplate { get { return ConfigurationManager.AppSettings["KZNoticeTemplate"]; } }
        #endregion

        /// <summary>
        /// 未完成订单创建天数
        /// </summary>
        public static string UnfinishedOrderCreateDays { get { return ConfigurationManager.AppSettings["UnfinishedOrderCreateDays"]; } }
        public static string UnfinishedOrderWarnEmails { get { return ConfigurationManager.AppSettings["UnfinishedOrderWarnEmails"]; } }
        public static string UnfinishedOrderCopyTo { get { return ConfigurationManager.AppSettings["UnfinishedOrderCopyTo"]; } }
        public static string SSOUrl { get { return ConfigurationManager.AppSettings["SSOUrl"]; } }

        #region 审核公告邮件通知
        /// <summary>
        /// 审核公告邮件通知
        /// </summary>
        public static List<string> AnnouncementAuditEmail
        {
            get
            {
                string emails = ConfigurationManager.AppSettings["AnnouncementAuditEmail"];
                if (string.IsNullOrWhiteSpace(emails))
                {
                    return null;
                }
                return emails.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }
        #endregion

        #region 收款通知
        /// <summary>
        /// 收款通知
        /// </summary>
        public static Dictionary<string, List<int>> PaymentNotice
        {
            get
            {
                string paymentNoticeStr = ConfigurationManager.AppSettings["PaymentNotice"];
                if (string.IsNullOrWhiteSpace(paymentNoticeStr))
                {
                    return null;
                }
                else
                {
                    Dictionary<string, List<int>> dic = new Dictionary<string, List<int>>();
                    string[] noticeArray = paymentNoticeStr.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string notice in noticeArray)
                    {
                        string[] companyIdGroupIdArray = notice.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (companyIdGroupIdArray.Length == 2)
                        {
                            string companyId = companyIdGroupIdArray[0];
                            string groupId = companyIdGroupIdArray[1];
                            int groupIdInt;
                            if (!string.IsNullOrWhiteSpace(companyId) && !string.IsNullOrWhiteSpace(groupId) && int.TryParse(groupId, out groupIdInt))
                            {
                                if (dic.ContainsKey(companyId))
                                {
                                    List<int> groupIdList = dic[companyId];
                                    if (groupIdList != null)
                                    {
                                        groupIdList.Add(groupIdInt);
                                    }
                                    else
                                    {
                                        groupIdList = new List<int> { groupIdInt };
                                    }
                                    dic[companyId] = groupIdList;
                                }
                                else
                                {
                                    List<int> groupIdList = new List<int> { groupIdInt };
                                    dic.Add(companyId, groupIdList);
                                }
                            }
                        }
                    }
                    return dic;
                }
            }
        }
        #endregion

        #region Redis分布式锁配置
        /// <summary>
        /// Redis分布式锁服务器
        /// </summary>
        public static string DistributedLockRedisServers { get { return ConfigurationManager.AppSettings["DistributedLockRedisServers"]; } }

        /// <summary>
        /// 分布式锁使用的Redis数据库
        /// </summary>
        public static int DistributedLockDefaultDb
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["DistributedLockDefaultDb"]))
                {
                    int result;
                    if (int.TryParse(ConfigurationManager.AppSettings["DistributedLockDefaultDb"], out result))
                    {
                        return result;
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// 获取分布式锁的默认超时时间
        /// 单位：秒，默认30秒
        /// </summary>
        public static int GetLockTimeOut
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["GetLockTimeOut"]))
                {
                    int result;
                    if (int.TryParse(ConfigurationManager.AppSettings["GetLockTimeOut"], out result))
                    {
                        return result;
                    }
                }
                return 30;
            }
        }

        /// <summary>
        /// 分布式锁内任务执行的默认超时时间
        /// 单位：秒，默认30秒
        /// </summary>
        public static int TaskRunTimeOut
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["TaskRunTimeOut"]))
                {
                    int result;
                    if (int.TryParse(ConfigurationManager.AppSettings["TaskRunTimeOut"], out result))
                    {
                        return result;
                    }
                }
                return 30;
            }
        }

        #endregion

        #region 罗氏APIGATEWAY配置
        /// <summary>
        /// api访问用户名
        /// </summary>
        public static string RocheApiGatewayUsername { get { return ConfigurationManager.AppSettings["RocheApiGatewayUsername"]; } }
        /// <summary>
        /// api访问密码
        /// </summary>
        public static string RocheApiGatewayPwd { get { return ConfigurationManager.AppSettings["RocheApiGatewayPwd"]; } }
        #endregion

        #region WebAPI配置
        public static string WebAPIAppID { get { return ConfigurationManager.AppSettings["WebAPIAppID"]; } }
        public static string WebAPIEncryptKey { get { return ConfigurationManager.AppSettings["WebAPIEncryptKey"]; } }
        public static string WebAPIUrl { get { return ConfigurationManager.AppSettings["WebAPIUrl"]; } }
        public static string PassportDESLoginWebAPIUrl { get { return ConfigurationManager.AppSettings["PassportDESLoginWebAPIUrl"]; } }
        public static string PassportLoginUrl { get { return ConfigurationManager.AppSettings["PassportLoginUrl"]; } }
        #endregion

        public static string StaticFileV2 { get { return ConfigurationManager.AppSettings["StaticFileV2"]; } }
        public static string StaticFile { get { return ConfigurationManager.AppSettings["StaticFile"]; } }

        public static string StaticFileVersion { get { return ConfigurationManager.AppSettings["StaticFileVersion"]; } }

        /// <summary>
        /// 图片保存路径
        /// </summary>
        public static string UploadMoudleName { get { return ConfigurationManager.AppSettings["UploadMoudleName"]; } }

        /// <summary>
        /// 上传URL
        /// </summary>
        public static string UploadUrl { get { return System.Configuration.ConfigurationManager.AppSettings["UploadUrl"]; } }

        /// <summary>
        /// Ueditor保存路径配置
        /// </summary>
        public static string[] ImageSavePath = new string[] { "upload1", "upload2", "upload3" };

        #region Email相关
        public static string Email_SMTP { get { return ConfigurationManager.AppSettings["Email_SMTP"]; } }
        public static string Email_SendFrom { get { return ConfigurationManager.AppSettings["Email_SendFrom"]; } }
        public static string Email_UserName { get { return ConfigurationManager.AppSettings["Email_UserName"]; } }
        public static string Email_PWD { get { return ConfigurationManager.AppSettings["Email_PWD"]; } }
        #endregion

        public static string CacheKeyPre { get { return ConfigurationManager.AppSettings["CacheKeyPre"]; } }
        public static string CacheSSOKeyPre { get { return ConfigurationManager.AppSettings["CacheSSOKeyPre"]; } }

        /// <summary>
        /// 单位证照证件名称列表
        /// </summary>
        public static Dictionary<int, string> CertifcateReg = new Dictionary<int, string> { { 0, "营业执照" }, { 1, "医疗机构执业许可证" }, { 2, "医疗器械经营许可证" }, { 3, "药品经营许可证" }, { 4, "危险品经营许可证" }, { 5, "质量体系经营许可证" }, { 6, "医疗器械生产许可证" }, { 7, "药品生产许可证" }, { 8, "放射性药品生产许可证" }, { 9, "放射性药品经营许可证" }, { 10, "厂商授权" }, { 11, "药品GSP证书" }, { 12, "药品GMP证书" }, { 13, "医疗器械生产产品登记表" }, { 14, "第二类医疗器械经营备案凭证" }, { 15, "第一类医疗器械生产备案凭证" } };

        public static Dictionary<int, string> MaterialCertificateType = new Dictionary<int, string> { { 0, "产品注册证" }, { 1, "产品授权" } };

        /// <summary>
        /// 个人证照证件名称列表
        /// </summary>
        public static Dictionary<int, string> UserCertifcateReg = new Dictionary<int, string> { { 0, "药品采购人员授权书" } };

        public static string AdminDomain { get { return ConfigurationManager.AppSettings["AdminDomain"]; } }

        public static string MainDomain { get { return ConfigurationManager.AppSettings["MainDomain"]; } }
        /// <summary>
        /// 卖家版主页
        /// </summary>
        public static string SellMainDomain { get { return ConfigurationManager.AppSettings["SellMainDomain"]; } }

        public static string BuyMainDomain { get { return ConfigurationManager.AppSettings["BuyMainDomain"]; } }

        public static string MaterialFilePath { get { return ConfigurationManager.AppSettings["MaterialFilePath"]; } }

        public static string DownloadFile { get { return ConfigurationManager.AppSettings["DownloadFile"]; } }

        public static string WMSCompanyID { get { return ConfigurationManager.AppSettings["WMSCompanyID"]; } }

        public static string LocalUploadPath { get { return ConfigurationManager.AppSettings["LocalUploadPath"]; } }

        public static string LocalZipSavePath { get { return ConfigurationManager.AppSettings["LocalZipSavePath"]; } }

        public static string StaticUrl { get { return ConfigurationManager.AppSettings["StaticUrl"]; } }

        public static string UploadFileUrl { get { return ConfigurationManager.AppSettings["UploadFileUrl"]; } }
        public static string UploadUrlNew { get { return ConfigurationManager.AppSettings["UploadUrlNew"]; } }

        public static Dictionary<int, string> BarterReason = new Dictionary<int, string> { { 0, "打错发货单" }, { 1, "财务科打错发票" }, { 2, "物流部发错货" }, { 3, "客户订错" }, { 4, "效期短" }, { 5, "要求换新批号" }, { 6, "要求换同一批号" }, { 7, "产品质量不好、效果不佳" }, { 8, "重复订货" }, { 9, "医院人员的人为因素" }, { 10, "因项目开展原因，试剂被停用" }, { 11, "销售员或产品专员订错货" }, { 12, "数量订多" }, { 13, "终端客户要求更换包装规格" }, { 14, "经确认为本公司责任的不合格产品（过期、损坏、污染）" }, { 15, "已过期" } };

        public static int MaxExcelProcessNum { get { return int.Parse(string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["MaxExcelProcessNum"]) == true ? "10" : ConfigurationManager.AppSettings["MaxExcelProcessNum"]); } }

        public static string PurReportTemplate { get { return ConfigurationManager.AppSettings["PurReportTemplate"]; } }
        /// <summary>
        /// 产品线采购指标的模板
        /// </summary>
        public static string PurProductBuyTargetTemplate { get { return ConfigurationManager.AppSettings["PurProductBuyTargetTemplate"]; } }

        public static string RundaFactoryID { get { return ConfigurationManager.AppSettings["factoryId"]; } }

        /// <summary>
        /// 微信队列QuenID
        /// </summary>
        public static string MessageQuenID { get { return ConfigurationManager.AppSettings["MessageQuenID"]; } }

        public static int DeliveryCheckDaySpan { get { return int.Parse(ConfigurationManager.AppSettings["DeliveryCheckDaySpan"]); } }

        public static int SPSCompanyID { get { return int.Parse(ConfigurationManager.AppSettings["SPSCompanyID"]); } }

        public static string SPSStockCode { get { return ConfigurationManager.AppSettings["SPSStockCode"]; } }

        public static int MaxUploadFileSize { get { return int.Parse(ConfigurationManager.AppSettings["MaxUploadFileSize"]); } }

        public static string SPSSAPCompanyIDs { get { return ConfigurationManager.AppSettings["SPSSAPCompanyIDs"]; } }

        public static string SHPharmSendCompanyID { get { return ConfigurationManager.AppSettings["SHPharmSendCompanyID"]; } }
        public static string SHPharmBuyCompanyID { get { return ConfigurationManager.AppSettings["SHPharmBuyCompanyID"]; } }
        public static string SHPharmSellCompanyID { get { return ConfigurationManager.AppSettings["SHPharmSellCompanyID"]; } }

        #region OA API BusinessConfigInfo 相关配置
        /// <summary>
        /// OA 创建采购订单业务编号
        /// </summary>
        public static string OA_CreateOrderBusinessCode { get { return ConfigurationManager.AppSettings["OA_CreateOrderBusinessCode"]; } }
        public static string OA_CreateOrderBusinessCode90 { get { return ConfigurationManager.AppSettings["OA_CreateOrderBusinessCode90"]; } }
        #endregion

        /// <summary>
        /// 外部订单对接队列
        /// </summary>
        public static string ExternalOrderQueneID { get { return CacheKeyPre + ConfigurationManager.AppSettings["ExternalOrderQueneID"]; } }
        /// <summary>
        /// life创建订单的url
        /// </summary>
        public static string LifeCreateOrderLink { get { return ConfigurationManager.AppSettings["LifeCreateOrderLink"]; } }
        /// <summary>
        /// life的identity
        /// </summary>
        public static string LifeToIdentity { get { return ConfigurationManager.AppSettings["LifeToIdentity"]; } }
        /// <summary>
        /// LifeSharedSecret
        /// </summary>
        public static string LifeSharedSecret { get { return ConfigurationManager.AppSettings["LifeSharedSecret"]; } }

        /// <summary>
        /// 
        /// </summary>
        public static string CertificateAlarmForUserCompanyList { get { return ConfigurationManager.AppSettings["CertificateAlarmForUserCompanyList"]; } }
        /// <summary>
        /// 需要统计告警数量（代理商资质告警和授权资质告警）的公司
        /// </summary>
        public static string SupplierAndAuth { get { return ConfigurationManager.AppSettings["SupplierAndAuth"]; } }


        /// <summary>
        /// 证照更新-物料品牌数组
        /// </summary>
        public static string[] BrandNameArray
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["BrandNameArray"]))
                    return new string[] { "" };
                else
                    return ConfigurationManager.AppSettings["BrandNameArray"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        /// <summary>
        /// 物料更新-物料卖方
        /// </summary>
        public static int[] SellCompanyIdArray
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["SellCompanyIdArray"]))
                    return new int[] { -1 };
                else
                {
                    return
                        ConfigurationManager.AppSettings["SellCompanyIdArray"].Split(new string[] { "," },
                            StringSplitOptions.RemoveEmptyEntries)
                            .ConvertAll(int.Parse).ToArray();
                }
            }
        }

        /// <summary>
        /// 是否为DEBUG环境
        /// 如未配置则视为线上环境
        /// </summary>
        public static bool IsDebug
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["IsDebug"]))
                {
                    return false;
                }
                else
                {
                    bool bl;
                    if (Boolean.TryParse(ConfigurationManager.AppSettings["IsDebug"], out bl))
                    {
                        return bl;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 采购SAP对接适用的SAP编号
        /// </summary>
        public static string PurchaseSAP
        {
            get { return ConfigurationManager.AppSettings["PurchaseSAP"]; }
        }

        /// <summary>
        /// 润达仓库地址
        /// </summary>
        public static int RundaWarehouseAddressID
        {
            get
            {
                int id = 0;
                string strID = ConfigurationManager.AppSettings["RundaWarehouseAddressID"];

                if (!string.IsNullOrEmpty(strID))
                {
                    int.TryParse(strID, out id);
                }

                return id;
            }
        }

        public static string SSOFrameLoginCustom { get { return ConfigurationManager.AppSettings["SSOFrameLoginCustom"]; } }

        /// <summary>
        /// 指定送达方即将到期的供应商证照及物料资质告警 SAP编号
        /// </summary>
        public static string CertificateAlarmForSendSAP { get { return ConfigurationManager.AppSettings["CertificateAlarmForSendSAP"]; } }

        /// <summary>
        /// 退货测试
        /// </summary>
        public static string TestReturn { get { return ConfigurationManager.AppSettings["TestReturn"]; } }

        /// <summary>
        /// 测试回单
        /// </summary>
        public static string TestReceipt { get { return ConfigurationManager.AppSettings["TestReceipt"]; } }

        /// <summary>
        /// 测试预留单
        /// </summary>
        public static string TestReserved { get { return ConfigurationManager.AppSettings["TestReserved"]; } }
        /// <summary>
        /// 采购非最低价业务员审批测试
        /// </summary>
        public static string SalesPurchaseApprovalTest { get { return ConfigurationManager.AppSettings["SalesPurchaseApprovalTest"]; } }

        /// <summary>
        /// 随货同行单，SAP客户编号
        /// </summary>
        public static string SHTXD { get { return ConfigurationManager.AppSettings["SHTXD"]; } }
        /// <summary>
        /// 冷链交接单
        /// </summary>
        public static string LLJJD { get { return ConfigurationManager.AppSettings["LLJJD"]; } }
        public static string SHTXD_TestUserID { get { return ConfigurationManager.AppSettings["SHTXD_TestUserID"]; } }
        /// <summary>
        /// 购销合同打印
        /// </summary>
        public static string GXHT { get { return ConfigurationManager.AppSettings["GXHT"]; } }
        /// <summary>
        /// 验收报告打印
        /// </summary>
        public static string YSBG { get { return ConfigurationManager.AppSettings["YSBG"]; } }
        /// <summary>
        /// 质检报告存放路径
        /// </summary>
        public static string QualityInspectionReportPath { get { return ConfigurationManager.AppSettings["QualityInspectionReport"]; } }

        /// <summary>
        /// 质检报告文件检索开始时间
        /// </summary>
        public static DateTime QualityInspectionReportBegin
        {
            get
            {
                string qualityInspectionReportBegin = ConfigurationManager.AppSettings["QualityInspectionReportBegin"];

                if (!string.IsNullOrEmpty(qualityInspectionReportBegin))
                {
                    return Convert.ToDateTime(qualityInspectionReportBegin);
                }
                else
                {
                    return Convert.ToDateTime("1900-1-1");
                }
            }
        }

        /// <summary>
        /// 设置Config内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetConfigValue(string key, string value)
        {
            try
            {
                string assemblyConfigFile = Assembly.GetEntryAssembly().Location;

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (config.AppSettings.Settings[key] != null)
                {
                    config.AppSettings.Settings[key].Value = value;
                }
                else
                {
                    config.AppSettings.Settings.Add(key, value);
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 上海润达B2B公司编号
        /// </summary>
        public static string RundaCompanyID { get { return ConfigurationManager.AppSettings["RundaCompanyID"]; } }

        /// <summary>
        /// 正规化信息方法编号
        /// </summary>
        public static string NormalizeFunctionID { get { return ConfigurationManager.AppSettings["NormalizeFunctionID"]; } }

        /// <summary>
        /// 正规化AppID
        /// </summary>
        public static string NormalizeAppID { get { return ConfigurationManager.AppSettings["NormalizeAppID"]; } }

        /// <summary>
        /// 上传润医T+订单地址
        /// </summary>
        public static string RY_WebServiceURL { get { return ConfigurationManager.AppSettings["RY_WebServiceURL"]; } }

        /// <summary>
        /// 上传柳润订单地址
        /// </summary>
        public static string LR_WebServiceURL { get { return ConfigurationManager.AppSettings["LR_WebServiceURL"]; } }

        public static string SPSNormalizeAPPID { get { return ConfigurationManager.AppSettings["SPSNormalizeAPPID"]; } }

        /// <summary>
        /// 接收客户投诉钉钉通知用户编号
        /// </summary>
        public static string ComplainDTMsgUserID
        {
            get { return ConfigurationManager.AppSettings["ComplainDTMsgUserID"]; }
        }

        #region 论坛参数
        /// <summary>
        /// BBSHash
        /// </summary>
        public static string BBSHash { get { return ConfigurationManager.AppSettings["BBSHash"]; } }
        /// <summary>
        /// BBSHash
        /// </summary>
        public static string BBSCookiePre { get { return ConfigurationManager.AppSettings["BBSCookiePre"]; } }
        /// <summary>
        /// BBSHash
        /// </summary>
        public static string BBSCompanyList { get { return ConfigurationManager.AppSettings["BBSCompanyList"]; } }
        #endregion

        #region 添加用户和批量添加用户发送密码短信提醒时用到的参数
        public static int CompanyID { get { return Convert.ToInt32(ConfigurationManager.AppSettings["CompanyID"]); } }
        public static string Sign { get { return ConfigurationManager.AppSettings["Sign"]; } }
        public static string SMSTemplate { get { return ConfigurationManager.AppSettings["SMSTemplate"]; } }
        #endregion

        /// <summary>
        /// 获取SAP交货单开始时间
        /// </summary>
        public static DateTime SAPDeliveryBeginTime
        {
            get
            {
                string sapDeliveryBeginTime = ConfigurationManager.AppSettings["SAPDeliveryBeginTime"];

                if (!string.IsNullOrEmpty(sapDeliveryBeginTime))
                {
                    return Convert.ToDateTime(sapDeliveryBeginTime);
                }
                else
                {
                    return Convert.ToDateTime("1900-1-1");
                }
            }
        }

        /// <summary>
        /// 限制修改收货地址客户编号
        /// </summary>
        public static string LimitEditAddressSAPCustomID { get { return ConfigurationManager.AppSettings["LimitEditAddressSAPCustomID"]; } }

        /// <summary>
        /// webapi 缓存前缀
        /// </summary>
        public static string WebAPICacheKeyPre { get { return ConfigurationManager.AppSettings["WebAPICacheKeyPre"]; } }

        //public static string EntrustSendAuditEmail { get{ return ConfigurationManager.AppSettings["EntrustSendAuditEmail"]; } }

        //public static string ConfigAdddrLimit { get { return ConfigurationManager.AppSettings["ConfigAdddrLimit"]; } }

        public static bool ConfigAdddrLimit
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ConfigAdddrLimit"]))
                {
                    return false;
                }
                else
                {
                    bool bl;
                    if (Boolean.TryParse(ConfigurationManager.AppSettings["ConfigAdddrLimit"], out bl))
                    {
                        return bl;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 通知CMS预到货地址
        /// </summary>
        public static string CMSASNURL { get { return ConfigurationManager.AppSettings["CMSASNURL"]; } }

        /// <summary>
        /// 手动创建交货单测试
        /// </summary>
        public static string CreateDeliveryTest { get { return ConfigurationManager.AppSettings["CreateDeliveryTest"]; } }

        /// <summary>
        /// 限制物料注册证是否过期的客户
        /// </summary>
        public static string LimitMaterialCertificateTimeCompanyID { get { return ConfigurationManager.AppSettings["LimitMaterialCertificateTimeCompanyID"]; } }
        /// <summary>
        /// B2B-WMS地址推送地址
        /// </summary>
        public static string WMS_B2BAddressSynchroUrl { get { return ConfigurationManager.AppSettings["WMS_B2BAddressSynchroUrl"]; } }

        public static string ChangeDelegatedCompanyIDNew { get { return ConfigurationManager.AppSettings["ChangeDelegatedCompanyIDNew"]; } }
        /// <summary>
        /// 定制打印交货单
        /// </summary>
        public static string DeliveryPrintCompanyID { get { return ConfigurationManager.AppSettings["DeliveryPrintCompanyID"]; } }
        /// <summary>
        /// 定制化打印交货单模板（大）
        /// </summary>
        public static string DeliveryPrintBigCompanyID { get { return ConfigurationManager.AppSettings["DeliveryPrintBigCompanyID"]; } }
        /// <summary>
        /// 是否开启物料初次订购提醒
        /// </summary>
        public static string MaterialConfirm
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaterialConfirm"]))
                {
                    return "false";
                }
                else
                {
                    return ConfigurationManager.AppSettings["MaterialConfirm"];
                }
            }
        }

        /// <summary>
        /// 队列运行环境
        /// </summary>
        public static string HostNameMQ { get { return ConfigurationManager.AppSettings["HostNameMQ"]; } }

        public static string VirtualHostMQ { get { return ConfigurationManager.AppSettings["VirtualHostMQ"]; } }
        /// <summary>
        /// 队列用户名
        /// </summary>
        public static string UserNameMQ { get { return ConfigurationManager.AppSettings["UserNameMQ"]; } }
        /// <summary>
        /// 对列密码
        /// </summary>
        public static string PasswordMQ { get { return ConfigurationManager.AppSettings["PasswordMQ"]; } }

        public static string FilePath { get { return ConfigurationManager.AppSettings["FilePath"]; } }

        /// <summary>
        /// 邮件发送排除的邮箱，减少退信(多个值用英文逗号隔开)
        /// </summary>
        public static string ExcludeMailTo { get { return ConfigurationManager.AppSettings["ExcludeMailTo"]; } }
        /// <summary>
        /// 处理订单页显示价格的供应商
        /// </summary>
        public static string HandlerShowPriceSellCompanyID { get { return ConfigurationManager.AppSettings["HandlerShowPriceSellCompanyID"]; } }
        #region 发送短信验证码的配置
        public static string CompanyId { get { return ConfigurationManager.AppSettings["CompanyId"]; } }
        public static string SignName { get { return ConfigurationManager.AppSettings["SignName"]; } }
        public static string TemplateCode { get { return ConfigurationManager.AppSettings["TemplateCode"]; } }
        #endregion

        #region 发货短信提醒配置
        public static int NoticeCompanyID { get { return Convert.ToInt32(ConfigurationManager.AppSettings["NoticeCompanyID"]); } }
        public static string NoticeSign { get { return ConfigurationManager.AppSettings["NoticeSign"]; } }
        public static string NoticeTemplate { get { return ConfigurationManager.AppSettings["NoticeTemplate"]; } }
        #endregion
        #region 手动穿件采购审批审核配置
        public static int CreateSAPPurchaseApplyCompanyID { get { return Convert.ToInt32(ConfigurationManager.AppSettings["CreateSAPPurchaseApplyCompanyID"]); } }
        #endregion

        #region 程序主页
        public static string WebMainUrl { get { return ConfigurationManager.AppSettings["WebMainUrl"]; } }
        #endregion
        /// <summary>
        /// wms移库接口地址
        /// </summary>
        public static string WMSMoveStockUrl { get { return ConfigurationManager.AppSettings["WMSMoveStockUrl"]; } }

        public static string WMSEnterStockUrl { get { return ConfigurationManager.AppSettings["WMSEnterStockUrl"]; } }
        #region B2B外链CCLS，下载温度记录zip包请求地址
        public static string DownLoadPdfZip { get { return ConfigurationManager.AppSettings["DownLoadPdfZip"]; } }
        #endregion


        //库存异动同步 时间范围
        public static string TransStockStartTime { get { return ConfigurationManager.AppSettings["TransStockStartTime"]; } }

        public static string TransStockEndTime { get { return ConfigurationManager.AppSettings["TransStockEndTime"]; } }

        public static string BuyMai47com { get { return ConfigurationManager.AppSettings["BuyMai47com"]; } }

        /// <summary>
        /// 国润库存查询接口地址
        /// </summary>
        public static string GrStockInfoUrl { get { return ConfigurationManager.AppSettings["GrStockInfoUrl"]; } }
        /// <summary>
        /// 国润接口appid
        /// </summary>
        public static string GrAppID { get { return ConfigurationManager.AppSettings["GrAppID"]; } }
        /// <summary>
        /// 国润接口accesstoken
        /// </summary>
        public static string GrAccesstToken { get { return ConfigurationManager.AppSettings["GrAccesstToken"]; } }
        /// <summary>
        /// 是否直接生成采购订单
        /// </summary>
        public static string AutoPurchase { get { return ConfigurationManager.AppSettings["AutoPurchase"]; } }
    }
}