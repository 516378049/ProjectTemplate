import vue from 'vue'
import vuex from 'vuex'
import { getSeller, getGoods, getRatings } from 'api'
import { loadLocal } from '@/common/js/storage'
import { Promise } from 'core-js';
vue.use(vuex)


let store = new vuex.Store({
  state: {
    //当前商家菜单，goods.vue初始化时同步，refereshed when sync Cart from server cache 
    goods: [],
    userInfo: null,
    seller: null
  },
  getters: {
    getUserInfo: state => {
      return state.userInfo
    }
  },
  mutations: {
    //add cart should sync server's cache
    addCart(state, item) {
    },
    //reduce cart should sync server's cache
    reduceCart(state, item) {

    },
    //get user info by WChat authorized 
    wxAuthorize(state, obj) {
      state.userInfo = obj.userInfo
    },
    //init goods from serve (lunch at goods.vue's method of fetch)
    initGoods(state) {
      if (state.goods != []) {
        getGoods({
          id: loadLocal('seller').id
        }).then((goods) => {
          state.goods = goods
        })
      }
    },
    //sycn goods from serve, （lunched at app.vue's mouted）
    syncGoods(state) {
      var item = {
        Cart: [
          {
            id: 0,
            name: '扁豆焖面',
            count: 2
          },
          {
            id: 1,
            name: 'VC无限橙果汁',
            count: 3
          }
        ]
      }
      state.goods.forEach((good) => {
        var food = good.foods[0]
        item.Cart.forEach((cart_) => {
          if (cart_.name == food.name) {
            if (!food.count) {
              vue.set(food, 'count', 0)
            }
            food.count = cart_.count
          }
        })
      })
    }
  },
  actions: {
    a_syncGoods({ commit }) {
      setInterval(() => {
        commit('syncGoods')
      }, 3000)
    }
  }
})
export default store
  
