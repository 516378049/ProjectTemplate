import Vue from 'vue'
import Router from 'vue-router'
import OrderConfirm from '../components/order-info/order-confirm.vue'
import OrderPay from '../components/order-info/order-pay.vue'
import PayResult from '../components/order-info/pay-result.vue'

import Home from '../components/home/home.vue'

import Index from '../Index.vue'
import App from '../App'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      redirect: '/App'
    },

    {
      path: '/Index',
      name: 'Index',
      component: Index
    },
    {
      path: '/App',
      name: 'App',
      component: App
    }
    ,{
      path: '/OrderConfirm',
      name: 'OrderConfirm',
      component: OrderConfirm
    }
    , {
      path: '/OrderPay',
      name: 'OrderPay',
      component: OrderPay
    }
    , {
      path: '/PayResult',
      name: 'PayResult',
      component: PayResult
    }

    , {
      path: '/Home',
      name: 'Home',
      component: Home
    }
  ]
})
