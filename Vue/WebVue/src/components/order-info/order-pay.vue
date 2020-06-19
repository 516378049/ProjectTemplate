<template>
  <div class="container">
    <div style="position:fixed;top:0;left:0;width:100%; height:61.8%; text-align:center;background-image:linear-gradient(#94c1e4,#ffffff);z-index:-1">
      <!--#ededed-->
    </div>
    <mt-header title="订单支付" style="background-color:inherit;font-size:large;color:#000000">
      <router-link to="" slot="left">
        <mt-button icon="back" @click.native="$router.back(-1)"></mt-button>
      </router-link>
    </mt-header>

    <div class="start" style="margin-top:30px"></div>

    <div class="OrderInfoPay">
      <div class="OrderInfoPaySingle">支付剩余时间：20:00</div>
      <div class="OrderInfoPaySingle"><span style="font-weight:bolder;color:#000000">￥</span><span style="font-size:xx-large;font-weight:bolder;color:#000000">88.88</span></div>
      <div class="OrderInfoPaySingle">商家：一品香粥</div>
      <div class="OrderInfoPaySingle">单号：201705270202011001</div>
      <!--<div class="OrderInfoPaySingle"><span style="display:inline-block;width:50%;text-align:right">支付剩余时间：</span>20:00</div>
    <div class="OrderInfoPaySingle"><span style="display:inline-block;width:50%;text-align:right">金额：</span><span style="font-weight:bolder;color:#000000">￥</span><span style="font-size:xx-large;font-weight:bolder;color:#000000">88.88</span></div>
    <div class="OrderInfoPaySingle"><span style="display:inline-block;width:50%;text-align:right">商家：</span>一品香粥</div>
    <div class="OrderInfoPaySingle"><span style="display:inline-block;width:50%;text-align:right">单号：</span>201705270202011001</div>-->
      <div class="clear"></div>
    </div>
    <div class="PayWay">

      <div style="width:100%">
        <img style="float:left" src="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1590644064945&di=1fa1afa6e5b5a3451bae65dc6e7a7668&imgtype=0&src=http%3A%2F%2Fimg011.21cnimg.com%2Fphoto%2F2017%2F0413%2F13%2F201217faf3a7a6a62c949082_o.jpg" height="48" width="48" slot="icon">
        <mt-radio style="width:100%" align="right" title="" v-model="mtRadioSel" :options="wxPay">
        </mt-radio>
      </div>

      <div style="width:100%">
        <img style="float:left" src="http://img.zcool.cn/community/01db1c5ad0dab9a8012138678d8db9.png@2o.png" height="48" width="48" slot="icon">
        <mt-radio style="width:100%" align="right" title="" v-model="mtRadioSel" :options="Alipay">
        </mt-radio>
      </div>

    </div>
    <mt-button style="margin-top:20px" type="danger" size="large" @click="pay">确认支付</mt-button>
    <div class="clear"></div>
  </div>
 
</template>
<script>
  export default {
    name:'OrderPay', 
    data() {
      return {
        mtRadioSel: "",
        Alipay:[
          {
            label: '支付宝支付',
            value: 'Alipay1'
          }
        ],
        wxPay: [
          {
            label: '微信支付',
            value: 'wxPay1'
          }
        ]
      }
    },
    props: {
    },
    computed: {
    },
    methods: {
      pay: function () {
        this.$router.push('PayResult');
      }
    }
  }
</script>

<style lang="stylus" rel="stylesheet/stylus">
  .container .OrderInfoPay {
    text-align: center;
  }

    .container .OrderInfoPay .OrderInfoPaySingle {
      margin-top: 10px;
      color: #4d4b4b
    }

  .container .PayWay {
    box-sizing: border-box;
    margin-top: 15px;
    padding: 10px;
  }
  
</style>
