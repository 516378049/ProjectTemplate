import axios from './axios'
import router from '@/router'

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
      params
    }).then((res) => {
      return res.data.Message
    }).catch((err) => {//如果此内部出错axios.interceptors.response.catch 捕获
      console.log("axios.catch")
      router.push({ path: '/errorPage', query: { data: err } })
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
      router.push({ path: '/errorPage', query: { data: err } })
      return Promise.reject(err);//this phrase will interption  this request.then  just like(eg:) redirection.wxAuthorize.then function
    })
  }
}
