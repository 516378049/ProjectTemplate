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
  import { getSeller, JSInit } from 'api'
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
      var currentUrl = window.location.href;
      JSInit({
        currentURL: currentUrl
      }).then((r) => {
        wx.config({
          debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
          appId: r.appId, // 必填，公众号的唯一标识
          timestamp: r.timestamp, // 必填，生成签名的时间戳
          nonceStr: r.nonceStr, // 必填，生成签名的随机串
          signature: r.signature,// 必填，签名，见附录1
          jsApiList: [
            'chooseWXPay'
          ] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
        });
        wx.ready(function () {
          //wx.hideMenuItems({
          //  menuList: [
          //    "menuItem:share:appMessage"
          //  ] // 要隐藏的菜单项，只能隐藏“传播类”和“保护类”按钮，所有menu项见附录3
          //});
        });

        wx.error(function (res) {
          console.log(res);
          // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。

        });
      })
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
