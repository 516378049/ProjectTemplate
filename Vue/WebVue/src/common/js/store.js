import vue from 'vue'
import vuex from 'vuex'
import { cnst, Fun } from '@/common/js/Global'
import { getSeller, getGoods, getRatings, setCartCount, getCartCount, getOrderInfoList } from 'api'
import { loadLocal, saveLocal } from '@/common/js/storage'
import { from } from 'array-flatten';
vue.use(vuex)

let store = new vuex.Store({
  state: {
    //当前商家菜单，goods.vue初始化时同步，refereshed when sync Cart from server cache 
    goods: [],
    userInfo: null,
    OrderInfoList:[]
    //seller: null
  },
  getters: {
    getUserInfo: state => {
      return state.userInfo
    },
    getSelMenuList: state => {
      var foods = []
      state.goods.forEach((good) => {
        good.foods.forEach((food) => {
          if (food.count && food.count > 0) {
            foods.push(food)
          }
        })
      })
      return foods
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
    //拉取订单，每次拉取5条，1、向下拉取查询当前时间以前订单，向上拉取订单，查询当前时间以后订单
    getOrderInfoList(state, obj) {

      getOrderInfoList({
        userId: state.userInfo.Id,
        sellerId: loadLocal('seller').id,
        startTime: obj.startTime,
        slipAction: obj.slipAction,
        count:5
      }).then((items) => {
        console.log(items)
        items.forEach((item) => {
          state.OrderInfoList.push(item)
        })
        //getOrderInfoList(state, obj)
      })
    },
    //更新所有显示的订单状态
    getOrderInfoListStatus(state, obj) {
      var _OrderInfoList = [];
      state.OrderInfoList.forEach(item => {
        _OrderInfoList.push({ Id: item.Id})
      })

      getOrderInfoListStatus(_OrderInfoList).then(items => {
        state.OrderInfoList.forEach(item => {
          items.forEach(_item => {
            if (item.Id == _item.Id) {
              item.Status = _item.Status
              return;
            }
          })
        })
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
            good.foods.forEach((food) => {
              item.forEach((cart_) => {
                if (cart_.menuId == food.Id) {
                  if (!food.count) {
                    vue.set(food, 'count', 0)
                  }
                  food.count = cart_.count
                }
              })
            })
          })
        }
        else {
          state.goods.forEach((good) => {
            good.foods.forEach((food) => {
              if (!food.count) {
                vue.set(food, 'count', 0)
              }
              food.count = 0
            })
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
      }, 1000000)
    },
    a_setCartCount({ commit }, item) {
      setTimeout(() => {
        commit('setCartCount', item)
      }, 1)
    }
  }
})
export default store
  
