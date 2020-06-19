import axios from 'ts-axios-new'
import { promise } from 'when';


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
      const { errno, data } = res.data
      if (errno === ERR_OK) {
        return JSON.parse(data)
      }
    }).catch((e) => { })
  }
}
