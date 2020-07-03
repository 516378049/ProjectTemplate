import Vue from 'vue'

import Index from './Index.vue'
import router from './router'
import store from './common/js/store'
import { loadLocal } from './common/js/storage'

import './cube-ui'
import './register'

import 'common/stylus/index.styl'

//customize
// 引入全部mint-ui组件
import Mint from 'mint-ui';
import 'mint-ui/lib/style.css'


import $ from 'jquery'
window.jquery = window.$ = $

Vue.use(Mint);
Vue.config.productionTip = false


new Vue({
  store,
  router,
  render: h => h(Index)
}).$mount('#app')
