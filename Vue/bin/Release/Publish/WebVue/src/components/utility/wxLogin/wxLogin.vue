<template>
  <div class="container">
    <div style="position:fixed;top:0;left:0;width:100%; height:61.8%; text-align:center;color:azure;background-image:linear-gradient(#26a2ff,aliceblue);z-index:-1">
    </div>
    <mt-header title="登录" style="font-size:large;background-color:inherit">
      <router-link to="" slot="left">
        <mt-button icon="back" @click.native="$router.back(-1)"></mt-button>
      </router-link>
    </mt-header>

    <div class="start" style="margin-top:70px"></div>
    <mt-field style="border: 1px solid #b6b6b6;" label="手机号码：" placeholder="请输入手机号码" value="15566866897" type="tel" v-model="login.phone"></mt-field>
    <span>
      <mt-field style="border: 1px solid #b6b6b6;width:calc(100% - 102px);float:left" label="手机验证：" placeholder="请输手机验证码" value="1111" v-model="login.password"></mt-field>
      <mt-button style="height:50px;width:100px" type="default" plain>发送验证</mt-button>
    </span>
    <mt-button style="margin-top:20px" type="danger" size="large" @click="m_login">登 录</mt-button>
    <div class="PayWay">

    </div>
  </div>
</template>

<script type="text/javascript">
  import cnst from '@/common/js/Global'
  import { saveLocal, loadLocal } from 'common/js/storage'
  export default {
    
    data() {
      return {
        login: {
          phone: "",
          password: ""
        },
        seller:{
          id: "",
          deskNumber:""
        }
      }
    },
    computed: {
    },
    mounted() {
      var that = this
      that.seller.id = that.$route.query.id
      that.seller.deskNumber = that.$route.query.deskNumber
      if (!that.seller.id && !that.seller.deskNumber) {
        this.$createDialog({
          title: '错误信息',
          content: `位置商家和桌号，您可扫描餐厅二维码打开`,
        }).show()
        return
      }
      saveLocal('seller', that.seller);

      //如果是微信访问直接通过微信授权登录
      var ua = navigator.userAgent.toLowerCase();
      if (ua.match(/MicroMessenger/i) == "micromessenger") {
        var goto_Auth2 = cnst.url.authorizeUrl;
        location.href = goto_Auth2;
      }
    },

    methods: {
      m_login() {
        location.href = cnst.url.wxLoginUrl;
      }
    }
  }
</script>

<style lang="stylus" scoped>
</style>
