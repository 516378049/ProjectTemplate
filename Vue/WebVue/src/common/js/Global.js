import Vue from 'vue'


Vue.prototype.$stringFormat = function stringFormat() {
  var s = arguments[0];
  for (var i = 0; i < arguments.length - 1; i++) {
    var reg = new RegExp("\\{" + i + "\\}", "gm");
    s = s.replace(reg, arguments[i + 1]);
  }
  return s;
}

const cnst =
{
  appid: 'wxba3211abca2a188c',
  url: {
    redirect_uri:'http://www.changchunamy.com/OrderMeal/#/App',
    authorizeUrl: "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo#wechat_redirect"
  }
}
export default cnst

  
