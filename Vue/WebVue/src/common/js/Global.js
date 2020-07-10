import Vue from 'vue'


Vue.prototype.$stringFormat = function stringFormat() {
  var s = arguments[0];
  for (var i = 0; i < arguments.length - 1; i++) {
    var reg = new RegExp("\\{" + i + "\\}", "gm");
    s = s.replace(reg, arguments[i + 1]);
  }
  return s;
}

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
  production: Vue.prototype.$stringFormat(urlPro._authorizeUrl, appid, encodeURIComponent(urlPro._redirect_uri)) 
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
    deskNumber: '888'
  }
}
export default cnst

  
