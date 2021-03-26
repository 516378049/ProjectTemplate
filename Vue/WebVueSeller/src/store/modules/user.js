import serverApi  from '@/api/config'
import { login, logout, getInfo } from '@/api/login'
import { getToken, setToken, setUserId, removeToken, getSeller, setSeller} from '@/utils/auth'


const user = {
  state: {
    token: getToken(),
    name: '',
    nick_name:'',
    id:-1,
    avatar: '',
    roles: [],
    seller: getSeller()
  },

  mutations: {
    SET_TOKEN: (state, token) => {
      state.token = token
    },
    SET_SELLER: (state, seller) => {
      state.seller = seller
    },
    SET_NAME: (state, name) => {
      state.name = name
    },
    SET_NICK_NAME: (state, nick_name) => {
      state.nick_name = nick_name
    },
    SET_ID: (state, id) => {
      state.id = id
    },
    SET_AVATAR: (state, avatar) => {
      state.avatar = avatar
    },
    SET_ROLES: (state, roles) => {
      state.roles = roles
    }
  },

  actions: {
    // 登录
    Login({ commit }, userInfo) {
      const username = userInfo.username.trim()
      return new Promise((resolve, reject) => {
        serverApi.login({ username, password:userInfo.password }).then(response => {
        //login(username, userInfo.password).then(response => {
          const data = response
          //const data = response.data
          console.log(data)
          //const tokenStr = data.tokenHead+data.token
          let seller = { seller_avatar: data.Message.seller_avatar, seller_name: data.Message.seller_name }
          const tokenStr = data.Message.access_token
          setToken(tokenStr)
          setSeller(seller)
          setUserId(data.Message.Id)
          commit('SET_ID', data.Message.Id)
          commit('SET_TOKEN', tokenStr)
          commit('SET_NICK_NAME', data.Message.nick_name)
          commit('SET_NAME', data.Message.name)
          commit('SET_AVATAR', data.Message.icon)
          commit('SET_SELLER', seller)
          
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },

    // 获取用户信息
    GetInfo({ commit, state }) {
      return new Promise((resolve, reject) => {
        getInfo().then(response => {
          const data = response.data
          if (data.roles && data.roles.length > 0) { // 验证返回的roles是否是一个非空数组
            commit('SET_ROLES', data.roles)
          } else {
            reject('getInfo: roles must be a non-null array !')
          }
          commit('SET_NAME', data.username)
          commit('SET_AVATAR', data.icon)
          resolve(response)
        }).catch(error => {
          reject(error)
        })
      })
    },

    // 登出
    LogOut({ commit, state }) {
      return new Promise((resolve, reject) => {
        logout(state.token).then(() => {
          commit('SET_TOKEN', '')
          commit('SET_ROLES', [])
          removeToken()
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },

    // 前端 登出
    FedLogOut({ commit }) {
      return new Promise(resolve => {
        commit('SET_TOKEN', '')
        removeToken()
        resolve()
      })
    }
  }
}

export default user
