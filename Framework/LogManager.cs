using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkDev.Logging;

namespace Framework
{
    public class LogManager
    {
        /// <summary>
        /// 默认日志器
        /// </summary>
        public static Logger DefaultLogger = LoggerFactory.GetLogger("DefaultLogger");
        public static Logger StorageLogger = LoggerFactory.GetLogger("StorageLogger");

        public static Logger OrderMsgLogger { get { return LoggerFactory.GetLogger("OrderMsgLogger"); } }

        public static Logger SetDelegateLogger { get { return LoggerFactory.GetLogger("SetDelegateLogger"); } }
        public static Logger DownStockLogger { get { return LoggerFactory.GetLogger("DownStockLogger"); } }
        public static Logger DownMaterialLogger { get { return LoggerFactory.GetLogger("DownMaterialLogger"); } }
        public static Logger DownCustomerLogger { get { return LoggerFactory.GetLogger("DownCustomerLogger"); } }
        public static Logger DownContractLogger { get { return LoggerFactory.GetLogger("DownContractLogger"); } }
        public static Logger DownVendorLogger { get { return LoggerFactory.GetLogger("DownVendorLogger"); } }
        public static Logger DeliverDownLogger { get { return LoggerFactory.GetLogger("DeliverDownLogger"); } }
        public static Logger DeliverCheckLogger { get { return LoggerFactory.GetLogger("DeliverCheckLogger"); } }
        public static Logger OrderSapLogger { get { return LoggerFactory.GetLogger("OrderSapLogger"); } }
        public static Logger OrderLogger { get { return LoggerFactory.GetLogger("OrderLogger"); } }

        public static Logger OrderCommitLogger { get { return LoggerFactory.GetLogger("OrderCommitLogger"); } }
        public static Logger MaterialRegDownloadLogger { get { return LoggerFactory.GetLogger("MaterialRegDownloadLogger"); } }

        public static Logger MaterialUploadToSAPLogger { get { return LoggerFactory.GetLogger("MaterialUploadToSAPLogger"); } }

        public static Logger ExternalLogger { get { return LoggerFactory.GetLogger("ExternalLogger"); } }
        public static Logger WebChatLogger { get { return LoggerFactory.GetLogger("WebChatLogger"); } }

        public static Logger WebApiLogger = LoggerFactory.GetLogger("WebApiLogger");

        /// <summary>
        /// 修改地址日志
        /// </summary>
        public static Logger AddressLogger { get { return LoggerFactory.GetLogger("AddressLogger"); } }

        //上药WebSerivce日志
        public static Logger SHPharmWebSerivceLogger { get { return LoggerFactory.GetLogger("SHPharmWebSerivceLogger"); } }

        /// <summary>
        /// 上传外部订单的日志
        /// </summary>
        public static Logger ExternalOrderHandlerLogger { get { return LoggerFactory.GetLogger("ExternalOrderHandlerLogger"); } }

        /// <summary>
        /// 采购相关日志对象
        /// </summary>
        public static Logger PurchaseLogger { get { return LoggerFactory.GetLogger("PurchaseLogger"); } }

        /// <summary>
        /// 支付相关日志
        /// </summary>
        public static Logger PaymentLogger { get { return LoggerFactory.GetLogger("PaymentLogger"); } }
        /// <summary>
        /// 提醒相关日志
        /// </summary>
        public static Logger NoticeLogger { get { return LoggerFactory.GetLogger("NoticeLogger"); } }

        /// <summary>
        /// B2B-CMS对接相关日志
        /// </summary>
        public static Logger B2BCMSLogger = LoggerFactory.GetLogger("B2BCMSLogger");
        
         /// <summary>
        /// B2B-润医T+对接相关日志
        /// </summary>
        public static Logger B2BRYTLogger = LoggerFactory.GetLogger("B2BRYTLogger");

        /// <summary>
        /// B2B-柳润对接相关日志
        /// </summary>
        public static Logger B2BLRLogger = LoggerFactory.GetLogger("B2BLRLogger");

        /// <summary>
        /// B2B-祥闰对接相关日志
        /// </summary>
        public static Logger B2BXRLogger = LoggerFactory.GetLogger("B2BXRLogger");

        /// <summary>
        /// B2B-润澜对接相关日志
        /// </summary>
        public static Logger B2BRLLogger = LoggerFactory.GetLogger("B2BRLLogger");

        /// <summary>
        /// B2B-伟康ERP对接相关日志
        /// </summary>
        public static Logger B2BWKLogger = LoggerFactory.GetLogger("B2BWKLogger");

        /// <summary>
        /// 手动入库
        /// </summary>
        public static Logger CustomMIGO { get { return LoggerFactory.GetLogger("CustomMIGO"); } }
    }
}
