<template>
  <div class="container">
  </div>
</template>

<script type="text/javascript">
  import { saveLocal, loadLocal } from 'common/js/storage'
  import { wxAuthorize } from '@/api/index'
  import qs from 'query-string'
  export default {
    
    data() {
      return {
        userInfo: {
          name:'长春'
        }
      }
    },
    computed: {
    },
    mounted() {
      var that = this
      var code = that.$route.query.code         //针对路由  { path: '/xxx', query: { id: id } }  注意获取参数用$route 路由跳转用$router 多个r
      //var code = that.$route.params.code      //针对路由  { name: 'xxx', params: { id: id } }
      
      //微信传过来的code暂时通过window.location.href来获取
      //var _url = window.location.href
      //var code=_url.substring(_url.indexOf('=') + 1, _url.indexOf('#'))
      if (code) {
        wxAuthorize({
          code: code
        }).then((userInfo) => {
          console.log(userInfo)
          this.$store.commit('wxAuthorize', { userInfo: userInfo })
          //enter seller menulist
          that.$router.push({ path: "/"});
          }).catch(e => { })
      }
    },

    methods: {

    }
  }
</script>

<style lang="stylus" scoped>
</style>
