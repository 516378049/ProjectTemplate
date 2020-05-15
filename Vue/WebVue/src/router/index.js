import Vue from 'vue'
import Router from 'vue-router'
import OrderConfirm from '../components/order-info/order-confirm.vue'
import App from '../App'
Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'App',
      component: App
    }
    ,{
      path: '/OrderConfirm',
      name: 'OrderConfirm',
      component: OrderConfirm
    }
  ]
})
