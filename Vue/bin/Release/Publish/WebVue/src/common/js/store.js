import vue from 'vue'
import vuex from 'vuex'
import { getSeller, getGoods, getRatings, setCartCount, getCartCount } from 'api'
import { loadLocal, saveLocal } from '@/common/js/storage'
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
    //add or reduce cart to redis server 
    setCartCount(state,item) {
      var _seller = loadLocal('seller')
      setCartCount({
        id: _seller.id,
        deskNumber: _seller.deskNumber,
        menuId: item.menuId,
        count: item.count
      })
    },
    //init goods from serve (lunch at goods.vue's method of fetch)
    initGoods(state) {
      var _goods = loadLocal("goods_" + loadLocal('seller').id)
      if (_goods) {
        state.goods = _goods
        return
      }
      getGoods({
        id: loadLocal('seller').id
      }).then((goods) => {
        state.goods = goods
        //save the goods to local
        saveLocal("goods_" + loadLocal('seller').id, goods)
      })
    },
    //sycn goods from serve, （lunched at app.vue's mouted）
    syncGoods(state) {
      var _seller = loadLocal('seller')
      getCartCount({
        id: _seller.id,
        deskNumber: _seller.deskNumber
      }).then((item) => {
        if (item != "") {
          state.goods.forEach((good) => {
            var food = good.foods[0]
            item.forEach((cart_) => {
              if (cart_.menuId == food.Id) {
                if (!food.count) {
                  vue.set(food, 'count', 0)
                }
                food.count = cart_.count
              }
            })
          })
        }
        else {
          state.goods.forEach((good) => {
            var food = good.foods[0]
            if (!food.count) {
              vue.set(food, 'count', 0)
            }
            food.count = 0
          })
        }
      })
    }
  },
  actions: {
    a_syncGoods({ commit }) {
      commit('syncGoods')
      setInterval(() => {
        commit('syncGoods')
      }, 5000)
    },
    a_setCartCount({ commit }, item) {
      setTimeout(() => {
        commit('setCartCount', item)
      }, 1)
    }
  }
})
export default store
  
