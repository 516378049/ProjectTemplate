import Vue from 'vue'
import Router from 'vue-router'
import OrderConfirm from '../components/order-info/order-confirm.vue'
import OrderDetail from '../components/order-info/order-detail.vue'
import OrderPay from '../components/order-info/order-pay.vue'
import PayResult from '../components/order-info/pay-result.vue'
import Home from '../components/home/home.vue'
import Me from '../components/home/Me.vue'
import OrderList from '../components/home/OrderList.vue'
import wxLogin from '@/components/utility/wxLogin/wxLogin'
import wxRedirect from '@/components/utility/wxRedirect/wxRedirect'
import Index from '../Index.vue'
import App from '../App'
import Test from '@/components/testTemplate/test.vue'
import errorPage from '@/components/utility/errorPage/errorPage'

import { loadLocal } from '@/common/js/storage'
import store from '@/common/js/store'
import { cnst } from '@/common/js/Global'
import qs from 'query-string'
Vue.use(Router)


let router = new Router({
  mode: 'history',
  base: '/OrderMeal/', //如果项目根目录不为域名，则添加设置域名后级目录 eg. OrderMeal
  routes: [
    {
      path: '/Test',
      name: 'Test',
      component: Test
    },

    {
      path: '/wxLogin',
      name: 'wxLogin',
      component: wxLogin
    },
    {
      path: '/wxRedirect',
      name: 'wxRedirect',
      component: wxRedirect
    },
    {
      path: '/errorPage',
      name: 'errorPage',
      component: errorPage
    },

    {
      path: '/',
      redirect: '/App'
      //redirect: '/Test'
    },

    {
      path: '/Index',
      name: 'Index',
      meta: {
        requireAuth: true // 添加该字段，表示进入这个路由是需要登录的
      },
      component: Index
    },
    {
      path: '/App',
      name: 'App',
      //必须要放到component前面，才能在beforeEach获取到
      meta: {
        requireAuth: true // 添加该字段，表示进入这个路由是需要登录的
      },
      component: App
    }
    , {
      path: '/OrderConfirm',
      name: 'OrderConfirm',
      meta: {
        requireAuth: true // 添加该字段，表示进入这个路由是需要登录的
      },
      component: OrderConfirm
    }
    , {
      path: '/OrderPay',
      name: 'OrderPay',
      meta: {
        requireAuth: true // 添加该字段，表示进入这个路由是需要登录的
      },
      component: OrderPay
    },
    {
      path: '/OrderDetail',
      name: 'OrderDetail',
      meta: {
        requireAuth: true // 添加该字段，表示进入这个路由是需要登录的
      },
      component: OrderDetail
    }
    , {
      path: '/PayResult',
      name: 'PayResult',
      meta: {
        requireAuth: true // 添加该字段，表示进入这个路由是需要登录的
      },
      component: PayResult
    }
    , {
      path: '/Home',
      name: 'Home',
      redirect: '/Home/Me',
      children: [
        {
          path: '/Home/Me',
          name: 'Me',
          meta: {
            requireAuth: true // 添加该字段，表示进入这个路由是需要登录的
          },
          component: Me
        },
        {
          path: '/Home/OrderList',
          name: 'OrderList',
          meta: {
            requireAuth: true // 添加该字段，表示进入这个路由是需要登录的
          },
          component: OrderList
        }
      ],
      component: Home
    }
   
   
  ]
})

//登录验证
router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requireAuth)) { // 判断该路由是否需要登录权限
    // 必须要有商家id和userInfo才能进入进入点餐，商家id保存在storage,userInfo 保存在vuex
    if (store.state.userInfo && loadLocal('seller')) {
      next();
    }
    else {
      next({
        path: '/wxLogin',
        query: cnst.query
      })
    }
  }
  else {
    next();
  }
})
export default router
