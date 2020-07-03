<template>
  <div class="container">
  </div>
</template>

<script type="text/javascript">
  import { saveLocal, loadLocal } from 'common/js/storage'
  import { wxAuthorize } from '@/api/index'
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
      var code = that.$route.query.code
      if (code) {
        wxAuthorize({
          code: code
        }).then((userInfo) => {
          console.log(userInfo)
          this.$store.commit('wxAuthorize', { userInfo: userInfo })
          //enter seller menulist
          that.$router.push("App");
          }).catch(e => { })
      }
    },

    methods: {

    }
  }
</script>

<style lang="stylus" scoped>
</style>
