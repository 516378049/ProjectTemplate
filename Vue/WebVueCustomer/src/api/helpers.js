import axios from './axios'
import router from '@/router'
import { Fun } from '@/common/js/Global'

import { Toast } from 'cube-ui'
import qs from 'qs';


const urlMap = {
  development: '/',
  production: 'https://www.changchunamy.com/'
}
const baseUrl = urlMap[process.env.NODE_ENV]
const ERR_OK = 0

export function get(url) {

  return function (params = {}) {
    var httpUrl = baseUrl + url
    return axios.get(httpUrl, {
      params: params,
      paramsSerializer: function (params) {
        return qs.stringify(params)
      },
    }).then((res) => {
      return res.data.Message
    }).catch((err) => {//如果此内部出错axios.interceptors.response.catch 捕获
      console.log("axios.catch")
      if (err.RetCode == '-3') {
        let toast = Toast.$create({
          time: 2000,
          txt: '服务器加载异常!'
        })
        toast.show()
      }
      else {
        router.push({ path: '/errorPage', query: { data: err } })
      }
      return Promise.reject(err);//this phrase will interption  this request.then  just like(eg:) redirection.wxAuthorize.then function
    })
  }
}

export function post(url) {
  return function (params = {}) {
    var httpUrl = baseUrl + url
    return axios.post(httpUrl,
      params
    ).then((res) => {
      return res.data.Message
    }).catch((err) => {//如果此内部出错axios.interceptors.response.catch 捕获
      console.log("axios.catch")

      if (err.RetCode == '-3') {
        let toast = Toast.$create({
          time: 2000,
          txt: '服务器加载异常!'
        })
        toast.show()
      }
      else {
        router.push({ path: '/errorPage', query: { data: err } })
      }
      return Promise.reject(err);//this phrase will interption  this request.then  just like(eg:) redirection.wxAuthorize.then function
    })
  }
}

