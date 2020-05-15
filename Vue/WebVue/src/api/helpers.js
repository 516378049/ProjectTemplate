import axios from 'ts-axios-new'
import { promise } from 'when';


const urlMap = {
    development: '/',
    production: 'http://vuecli.test/'
}
const baseUrl = urlMap[process.env.NODE_ENV]
const ERR_OK = 0

export function get(url) {

  const appData = require('../../data.json')
  const seller = appData.seller
  const goods = appData.goods
  const ratings = appData.ratings
  
  return function (params = {}) {
    return new Promise(function (reslove, reject) {
      reslove('成功')  //状态由等待变为成功，传的参数作为then函数中成功函数的实参
      //reject('失败')  //状态由等待变为失败，传的参数作为then函数中失败函数的实参
    }).then((res) => {
      var data
      if (url == 'api/seller') {
         data = seller
      }
      else if (url == 'api/goods') {
         data = goods
      }
      else if (url == 'api/ratings') {
        data = ratings
      }
      console.log('testTata')
      console.log(url)
      console.log(data)
      return data
    })
    
    //return axios.get(baseUrl + url, {
    //  params
    //}).then((res) => {
    //  const { errno, data } = res.data
    //  if (errno === ERR_OK) {
    //    if (url == '/api/seller') {
    //      return seller
    //    }
    //    else if (url == '/api/goods') {
    //      return goods
    //    }
    //    return JSON.parse(data)
    //  }
    //}).catch((e) => { })
  }
}
