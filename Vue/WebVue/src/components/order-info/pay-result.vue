<template>
  <div class="container">
    <div style="position:fixed;top:0;left:0;width:100%; height:61.8%; text-align:center;background-image:linear-gradient(#efb5b5,#ffffff);z-index:-1">
    </div>
    <mt-header title="支付结果" style="background-color:inherit;font-size:large;color:#000000">
      <router-link to="" slot="left">
        <mt-button icon="back" @click.native="$router.back(-1)"></mt-button>
      </router-link>
    </mt-header>
    <div class="start" style="margin-top:30px"></div>
    <div class="PayResultInfo">
      <div class="PayResultInfoSingle" style="font-size:x-large;color:#296844">支付成功</div>
      <div class="PayResultInfoSingle"><span style="font-weight:100;color:#000000">桌号：</span><span style="font-size:xx-large;font-weight:bolder;color:#000000">098</span></div>
      <div class="PayResultInfoSingle" style="color:#296844">服务员即将为您上餐...请您耐心等待！</div>
      <mt-button class="bolder" plain style="margin-top:20px;" type="default" size="small" @click="ShowOrderList" >查看订单</mt-button>
      <mt-button plain style="margin-top:20px;margin-left:30px" type="default" size="small" @click="OrderAdd">继续加菜</mt-button>
      <div class="clear"></div>
    </div>

    <div class="clear"></div>
  </div>

</template>
<script>
  export default {
    data() {
      return {}
    },
    props: {
    },
    computed: {
    },
    methods: {

      ShowOrderList: function () {
        this.$router.push('home')
      },

      OrderAdd: function () {
        this.$createDialog({
          type: 'alert',
          title: '健康的秘诀',
          content: '不能吃太饱哦！',
          icon: 'cubeic-like'
        }).show()
      }
    },
    components: {}
  }
</script>

<style lang="stylus" rel="stylesheet/stylus">
  .container .PayResultInfo {
    box-sizing: border-box;
    text-align: center;
    padding-left: 15px;
    padding-right: 15px;
  }

    .container .PayResultInfo .PayResultInfoSingle {
      margin-top: 15px;
    }
</style>
