//import Vue from 'vue'
//let global_fun = function(){
//  Vue.prototype.$stringFormat = function stringFormat() {
//    var s = arguments[0];
//    for (var i = 0; i < arguments.length - 1; i++) {
//      var reg = new RegExp("\\{" + i + "\\}", "gm");
//      s = s.replace(reg, arguments[i + 1]);
//    }
//    return s;
//  }
//  Vue.prototype.PrefixInteger = function (num, n) {
//    return (Array(n).join(0) + num).slice(-n);
//  }

//  //时区转换
//  Vue.prototype.formatTimeZone = function (time, offset) {
//    var d = new Date(time); //创建一个Date对象 time时间 offset 时区  中国为  8
//    var localTime = d.getTime();
//    var localOffset = d.getTimezoneOffset() * 60000; //获得当地时间偏移的毫秒数
//    var utc = localTime + localOffset; //utc即GMT时间
//    var wishTime = utc + (3600000 * offset);
//    return new Date(wishTime);
//  }
//}
let _fun = {
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
  }
}

export default {
  _fun
}


  
