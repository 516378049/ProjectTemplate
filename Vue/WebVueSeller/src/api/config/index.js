
import request from '@/utils/request'
//const request = require('../../utils/request').default

function requestGet(url) {
  return function (params = {}) {
    return request({
      url: url,
      method: 'GET',
      params
    })
  }
}
function requestPost(url) {
  return function (params = {}) {
    return request({
      url: url,
      method: 'POST',
      data: params
    })
  }
}


const serverApi = {
  login: requestPost('Authorize/Home/webAuthorize'),//登录
  getSeller: requestGet('selleradmin/api.selleradmin.getSellerList/'),//获取商家信息
  saveSeller: requestPost('selleradmin/api.selleradmin.saveSeller/'),//保存商家信息
  uploadFile: requestPost('selleradmin/api.selleradmin.uploadFile/')//上传图片
}

export default serverApi 








//export function serverApi() { return  serverApi}
//exports.serverApi = function (options) { return serverApi }
//module.exports = { serverApi: serverApi}

