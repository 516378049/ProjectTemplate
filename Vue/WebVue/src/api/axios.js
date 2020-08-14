
import axios from 'ts-axios-new'
//import { loadLocal } from '@/common/js/storage'
import store from '@/common/js/store'
import router from '@/router'

// 请求拦截（配置发送请求的信息）
axios.interceptors.request.use(
  configs => {
    try {
      let userinfo = store.state.userInfo

      if (userinfo && userinfo.access_token) {
        configs.headers.common.accesstoken = userinfo.access_token;
        configs.headers.common.UserId = userinfo.Id;
        //'content-type multipart/form-data'
      }
      else if (configs.url.indexOf('wxAuthorize') == -1) {
        //add  this phrase will catched by axios.interceptors.reponse.use.err and will bring err massage , if there is not return both of "return Promise.reject("")" and "return config" it will also catched by axios.interceptors.reponse.use.err
        return Promise.reject(
          { RetCode: '-1', RetMsg: '访问业务数据没有携带tken', errLocation: 'axios.interceptors.request.try' }
        );
      }
    } catch (e) {
      return Promise.reject({ RetCode: e.name, RetMsg: e.message, errLocation: 'axios.interceptors.request.catch' })
    }
    return configs//正常情况下，不返回会报错
  }, err => {
    // 请求失败的处理
    //alert("interceptors.request.use 出错啦")
    return Promise.reject({ RetCode: "-1", RetMsg: err, errLocation: 'axios.interceptors.request.use.err' });
  });


// 响应拦截（配置请求回来的信息）
axios.interceptors.response.use(
  response => {
    try {
      if (response.data.RetCode != '0') {
        return Promise.reject({ RetCode: response.data.RetCode, RetMsg: response.data.RetMsg, errLocation: 'axios.interceptors.response.use.try' }); //如果加了这句，则会进入axios 的.catch方法，并且会将参数err传入catch中 , 如果不加则会进入 axios的then方法中，注意return 只会执行一次
      }
    }
    catch (e) {
      return Promise.reject({ RetCode: e.name, RetMsg:e.message, errLocation: 'axios.interceptors.response.use.catch' }); //如果加了这句，则会进入axios 的catch方法中，并且会将参数e传入catch中
    }
    return response;//正常情况下，不返回response也会进入axios请求的then方法中，但是结果没有http请求信息，加了这句会进如axios的then方法，并且传入http的请求信息
  }, err => {
    //alert("interceptors.response.use 出错啦,请管理员处理")
    return Promise.reject({ RetCode: "-1", RetMsg: err, errLocation: 'axios.interceptors.response.use.err' });//add  this phrase will catched by axios.catch
  });

export default axios
