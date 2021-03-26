import axios from 'axios'
import { Message, MessageBox } from 'element-ui'
import store from '../store'
import { getToken, getUserId } from '@/utils/auth'
// 创建axios实例
const service = axios.create({
  baseURL: process.env.BASE_API, // api的base_url
  timeout: 30 * 1000 // 请求超时时间
})


// request拦截器
service.interceptors.request.use(config => {
  try {
    if (store.getters.token) {
      config.headers['accesstoken'] = getToken() // 让每个请求携带自定义token 请根据实际情况自行修改
      config.headers['UserId'] = getUserId()
      config.headers['App'] = "web"
    }
  }
  catch (e) {
    return Promise.reject(e)//加了return  返回就不会往下执行，发送请求
    console.log('request.use.catch' + e.message)
  }
  return config
}, error => {
  console.log('request.use.error')
  console.log(error) // for debug
  Promise.reject(error)
})

// respone拦截器
service.interceptors.response.use(
  response => {
    /**
    * code为非0是抛错 可结合自己业务进行修改
    */
    console.log(response.data)
    const res = response.data
    if (res.RetCode !== '0') {
      // 401:未登录;
      if (res.RetCode === '401') {
        MessageBox.confirm('您已被登出，可以取消继续留在该页面，或者重新登录', '确定登出', {
          confirmButtonText: '重新登录',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          store.dispatch('FedLogOut').then(() => {
            location.reload()// 为了重新实例化vue-router对象 避免bug
          })
        })
      }
      //如果返回-1，则直接统一提示
      else //if (res.RetCode === '-1')
      {
        Message({
          message: res.RetMsg,
          type: 'warning',
          duration: 0,
          showClose: true
        })
      }

      //其他<0的RetCode,返回由用户统一处理 需要增加return ，否则默认返回 resolve 函数
      return Promise.reject(response.data)
    }
    return response.data  //返回resolve 参数为 response.data
  },
  error => {
    console.log('response.use')
    console.log('err' + error)// for debug
    Message({
      message: error.message,
      type: 'error',
      duration: 0,
      showClose: true
    })
    return Promise.reject(error)
  }
)

export default service
