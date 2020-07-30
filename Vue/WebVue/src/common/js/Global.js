
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

  //时区转换
  formatTimeZone: function (time, offset) {
    var d = new Date(time); //创建一个Date对象 time时间 offset 时区  中国为  8
    var localTime = d.getTime();
    var localOffset = d.getTimezoneOffset() * 60000; //获得当地时间偏移的毫秒数
    var utc = localTime + localOffset; //utc即GMT时间
    var wishTime = utc + (3600000 * offset);
    return new Date(wishTime);
  },
  dateFormat: function (val, fmt) {
    var day
    if (!(val instanceof Date)) {
      val = val.replace(/-/g, "/");
      day = new Date(val.replace('T', ' '));
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
    return fmt;
  }
}

import vue from 'vue'
vue.filter('dateFormat', Fun.dateFormat)

const appid= 'wxba3211abca2a188c';

const urlPro = {
  _redirect_uri : 'https://www.changchunamy.com/OrderMeal/wxRedirect/',
  _authorizeUrl : 'https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo#wechat_redirect'
}

const urlDev = {
  _authorizeUrl : 'http://localhost:8080/OrderMeal/wxRedirect?code=00001',
}

const _authorizeUrl = {
  development: urlDev._authorizeUrl,
  production: Fun.stringFormat(urlPro._authorizeUrl, appid, encodeURIComponent(urlPro._redirect_uri)) 
}

const _wxLoginUrl = {
  development: 'http://localhost:8080/OrderMeal/wxRedirect?code=00001',
  production: 'https://www.changchunamy.com/OrderMeal/wxRedirect?code=00001'
}

const cnst =
{
  url: {
    authorizeUrl: _authorizeUrl[process.env.NODE_ENV],
    wxLoginUrl: _wxLoginUrl[process.env.NODE_ENV]
  },
  query: {//默认测试商家
    id: '4',
    deskNumber: '001'
  }
}
export { cnst, Fun }

  
