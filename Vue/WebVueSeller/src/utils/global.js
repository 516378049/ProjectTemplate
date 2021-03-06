import vue from 'vue'
import serverApi from '@/api/config'

//jquery扩展方法
$(function () {
  $.fn.extend({
    serializeObject: function () {
      if (this.length > 1) {
        return false;
      }
      var arr = this.serializeArray();
      var obj = new Object;
      $.each(arr, function (k, v) {
        obj[v.name] = v.value;
      });
      return obj;
    }
  });
})

//方法
let Fun = {
  stringFormat: function stringFormat() {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
      var reg = new RegExp("\\{" + i + "\\}", "gm");
      s = s.replace(reg, arguments[i + 1]);
    }
    return s;
  },
  PrefixInteger: function (num, n) {
    return (Array(n).join(0) + num).slice(-n);
  },
  //时区转换(此方法存在问题)
  formatTimeZone: function (time, offset) {
    var d = new Date(time); //创建一个Date对象 time时间 offset 时区  中国为  8
    var localTime = d.getTime();
    var localOffset = d.getTimezoneOffset() * 60000; //获得当地时间偏移的毫秒数
    var utc = localTime + localOffset; //utc即GMT时间
    var wishTime = utc + (3600000 * offset);
    return new Date(wishTime);
  },
  dateFormat: function (val, fmt) {
    if (!fmt) {
      fmt ="yyyy-MM-dd HH:mm:ss"
    }
    var day
    if (val) {
      if (!(val instanceof Date)) {
        val = val.replace(/-/g, "/");
        val=val.replace('T', ' ');
        while (val.indexOf('.') > -1)//如果有小数点作为毫秒，将小数点换成":" ,防止微信内置浏览器无法转换
        {
          val = val.split('.')[0]
        }
        day = new Date(val)
      }
      else {
        day = val
      }
      var o = {
        "M+": day.getMonth() + 1, //月份
        "d+": day.getDate(), //日
        "H+": day.getHours(), //小时
        "m+": day.getMinutes(), //分
        "s+": day.getSeconds(), //秒
        "q+": Math.floor((day.getMonth() + 3) / 3), //季度
        "S": day.getMilliseconds() //毫秒
      };
      if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (day.getFullYear() + "").substr(4 - RegExp.$1.length));
      for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    }
   
    return fmt;
  },
  //转化为时间类型
  toDate: function(val) {
    var newDateTime
    if (val) {
      if (!(val instanceof Date)) {
        val = val.replace(/-/g, "/");
        val = val.replace('T', ' ');
        while (val.indexOf('.') > -1)//如果有小数点作为毫秒，将小数点换成":" ,防止微信内置浏览器无法转换
        {
          val = val.split('.')[0]
        }
        newDateTime = new Date(val)
      }
      else {
        newDateTime = val
      }
    }
    return newDateTime
  },
  dateDiff: function (begintime, endtime, unit) {
    begintime = this.toDate(begintime)
    endtime = this.toDate(endtime)
    unit = unit.toUpperCase()
    var _diff = Math.floor(new Date(begintime) - new Date(endtime))
    if (unit == 'S') {
      _diff= _diff / 1000 
    }
    else if (unit == 'M') {
      _diff =  _diff / 1000 / 60
    }
    else if (unit == 'H') {
      _diff =  _diff / 1000 / 60 / 60
    }
    else if (unit == 'D') {
      _diff =  _diff / 1000 / 60 / 60 / 24
    }
    else {
      _diff =  _diff
    }
    return Math.floor(_diff)
  },
  //订单状态：1、待支付、2、商家待接单；3、商家已接单；4、订单完成；5、待评价；6、已评价；7、取消订单；8、申请退款；9、商家同意退款；10、退款成功
  StatuStr: function (value) {
    switch (value) {
      case 1: return '待支付';
      case 2: return '已支付(商家接单中...)';
      case 3: return '商家已接单';
      case 4: return '订单完成';
      case 5: return '待评价';
      case 6: return '已评价';
      case 7: return '已取消';
      case 8: return '退款中';
      case 9: return '商家同意退款';
      case 10: return '退款成功';
      default: return '未知状态'
    }
  }
}
//常量
const cnst =
{
  
}
//变量
var variable =
{
  fetchLodding: false
}



//过滤器
vue.filter('dateFormat', Fun.dateFormat)
vue.filter('StatuStr', Fun.StatuStr)
vue.filter('PrefixInteger', Fun.PrefixInteger)

//全局变量
window.serverApi = serverApi
vue.prototype.Global = { cnst, Fun, variable}
export { cnst, Fun, variable }

  
