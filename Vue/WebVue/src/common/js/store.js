import vue from 'vue'
import vuex from 'vuex'
vue.use(vuex)


let store = new vuex.Store({
  state: {
    //当前商家菜单，goods.vue初始化时同步，refereshed when sync Cart from server cache 
    goods: []
  },
  mutations: {
    //add cart should sync server's cache
    addCart(state, item) {
    },
    //reduce cart should sync server's cache
    reduceCart(state, item) {

    },
    //init goods from serve (lunch at goods.vue's method of fetch)
    initGoods(state, goods) {
      state.goods = goods.goods
    },
    //sycn goods from serve, （lunched at app.vue's mouted）
    syncGoods(state, item) {
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
  }
})
export default store
  
