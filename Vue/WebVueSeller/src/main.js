import Vue from 'vue'


import 'normalize.css/normalize.css'// A modern alternative to CSS resets

import ElementUI from 'element-ui'
import locale from 'element-ui/lib/locale/lang/zh-CN' // lang i18n
import VCharts from 'v-charts'

import '@/styles/index.scss' // global css

import App from './App'
import router from './router'
import store from './store'

import '@/icons' // icon
import '@/permission' // permission control
import '@/utils/global' 

Vue.use(ElementUI, { locale })
Vue.use(VCharts)

Vue.config.productionTip = false


new Vue({
  el: '#app',
  router,
  store,
  template: '<App/>',
  components: { App }
})
