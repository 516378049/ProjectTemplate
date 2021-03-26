import Vue from 'vue'

import Index from './Index.vue'
import router from './router'
import store from './common/js/store'
import { cnst, Fun } from '@/common/js/Global'

import  './cube-ui.js'
import './register'
import Mint from 'mint-ui';

import 'common/stylus/index.styl'
import 'mint-ui/lib/style.css'

import $ from 'jquery'
//window.jquery = window.$ = $

Vue.prototype.Global = { cnst, Fun }

Vue.use(Mint);
Vue.config.productionTip = false




new Vue({
  store,
  router,
  render: h => h(Index)
}).$mount('#app')

