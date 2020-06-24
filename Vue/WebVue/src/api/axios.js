
import axios from 'ts-axios-new'
//import { loadLocal } from '@/common/js/storage'
import store from '@/common/js/store'
import router from '@/router'

// 请求拦截（配置发送请求的信息）
axios.interceptors.request.use(
  configs => {
    try {
      
      let userinfo = store.state.userInfo
      if (userinfo && userinfo.accesstoken) {
        configs.headers.common.accesstoken = userinfo.accesstoken;
      }
      //console.log(configs.url)
      else if ('/api/Home/wxAuthorize'.indexOf(configs.url) == -1) {
        router.push(
          {
            path: '/errorPage'
            //params: {}
          }
        )
      }
      return configs//正常情况下，不返回会报错
    } catch (e) {
      alert("interceptors.request.use（try） 出错啦")
    }
    
  }, err => {
    // 请求失败的处理
    alert("interceptors.request.use 出错啦")
    return Promise.reject(err);
  });


// 响应拦截（配置请求回来的信息）
axios.interceptors.response.use(
  response => {
    //此处添加将loading去掉的代码
    return response;//正常情况下，不返回会报错
  }, err => {
    alert("interceptors.response.use 出错啦")
    return Promise.reject(err);
  });

export default axios
