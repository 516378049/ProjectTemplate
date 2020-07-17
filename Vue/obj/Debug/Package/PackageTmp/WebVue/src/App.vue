<template>
  <div id="app">
       <!--@touchmove.prevent-->
       
    <v-header :seller="seller"></v-header>
    <div class="tab-wrapper">
      <tab :tabs="tabs"></tab>
    </div>
    <navFloatIcons></navFloatIcons>
  </div>
</template>

<script>
  import qs from 'query-string'
  import { loadLocal,saveLocal } from '@/common/js/storage'
  import { getSeller } from 'api'
  import VHeader from 'components/v-header/v-header'
  import Goods from 'components/goods/goods'
  import Ratings from 'components/ratings/ratings'
  import Seller from 'components/seller/seller'
  import Tab from 'components/tab/tab'
  import navFloatIcons from '@/components/utility/nav-float-icons/nav-float-icons.vue'
  import 'weui'
import { map } from 'when';
import { locale } from 'core-js';
  export default {
    data() {
      return {
        seller: {
          //id: qs.parse(location.search).id
          id: loadLocal('seller').id
        }
      }
    },
    computed: {
      tabs() {
        return [
          {
            label: '商品',
            component: Goods,
            data: {
              seller: this.seller
            }
          },
          {
            label: '评论',
            component: Ratings,
            data: {
              seller: this.seller
            }
          },
          {
            label: '商家',
            component: Seller,
            data: {
              seller: this.seller
            }
          }
        ]
      }
    },
    created() {
      this._getSeller()
    },
    mounted() {
      //sycn server's cache cart to vuex
      var that = this;
      that.$store.dispatch('a_syncGoods')
    },
    methods: {
      _getSeller() {
        var _seller = loadLocal('seller_' + this.seller.id)
        if (_seller) {
          this.seller = Object.assign({}, this.seller, _seller)
          return
        }

        getSeller({
          id: loadLocal('seller').id
        }).then((seller) => {
          this.seller = Object.assign({}, this.seller, seller)
          //save to localeStorage
          saveLocal('seller_' + this.seller.id, seller)
        })
      }
    },
    components: {
      Tab,
      VHeader,
      navFloatIcons
    }
  }
</script>

<style lang="stylus" scoped>
  #app
    .tab-wrapper
      position: fixed
      top: 136px
      left: 0
      right: 0
      bottom: 0
      
</style>
