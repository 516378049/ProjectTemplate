using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wxPayApiMVC.lib
{
    public class PayParams
    {
        public string OrderId { set; get; }
        public int TotalFee { set; get; }
        public string OpenId { set; get; }
        public string TradeType {  set; get; }
        public string SpbillCreateIp { set; get; }
    }
    
}