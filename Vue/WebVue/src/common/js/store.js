import vue from 'vue'
import vuex from 'vuex'
vue.use(vuex)


let store = new vuex.Store({
  state: {
    //共享购物车数量，和后台缓存交互
    Cart:{
      id: '',
      count:0
    },
    //当前商家菜单，goods.vue初始化时同步，refereshed when sync Cart from server cache 
    goods: []
  },
  mutations: {
    //add cart should sync server's cache
    addCart(state, item) {
      //if (!state.goods[0].foods[0].count) {
      //  vue.set(state.goods[0].foods[0], 'count', 1)
      //}
      //else {
      //  state.goods[0].foods[0].count++
      //}
    },

    reduceCart(state, item) {

    },
    //init goods from serve (lunch at goods.vue's method of fetch)
    initGoods(state, goods) {
      state.goods = goods.goods
    },
    //sycn goods from serve, 
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
  
