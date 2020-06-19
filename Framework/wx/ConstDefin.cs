using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wxPayApiMVC.lib
{
    public class ConstDefin
    {
        //说明：JSAPI--JSAPI支付（或小程序支付）、NATIVE--Native支付、APP--app支付，MWEB--H5支付，不同trade_type决定了调起支付的方式
        //MICROPAY--付款码支付，付款码支付有单独的支付接口，所以接口不需要上传，该字段在对账单中会出现
        public const string TRADE_TYPE_H5 = "MWEB";
        public const string TRADE_TYPE_JSAPI = "JSAPI";
        public const string TRADE_TYPE_NATIVE = "NATIVE"; 


    }
}