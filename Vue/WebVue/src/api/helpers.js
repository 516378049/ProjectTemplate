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
      if ('/api/Home/wxAuthorize' == httpUrl) {
        if (res.data.RetCode) {
          if (res.data.RetCode != '0') {
            router.push({ path: '/errorPage', query: { errContent: res.data } })
          }
          else {
            return res.data
          }
          return res.data
        }
        
      }
      else {
        console.log('res')
        console.log(res.data)
        const { errno, data } = res.data
        if (errno === ERR_OK) {
          return data
          //return JSON.parse(data)
        }
      }
      }).catch((e) => {
        console.log("axios exception")
        console.log(e)
        router.push({ path: '/errorPage', query: { expContent: e} })
      })
  }
}
