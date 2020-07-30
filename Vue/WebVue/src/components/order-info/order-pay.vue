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
      <div class="OrderInfoPaySingle">支付剩余时间：{{this.restPayTime}}</div>
      <div class="OrderInfoPaySingle"><span style="font-weight:bolder;color:#000000">￥</span><span style="font-size:xx-large;font-weight:bolder;color:#000000">{{orderInfo.OrderAmount}}</span></div>
      <div class="OrderInfoPaySingle">商家：{{orderInfo.SellerName}}</div>
      <div class="OrderInfoPaySingle">单号：{{orderInfo.OrderId}}</div>
      <!--<div class="OrderInfoPaySingle"><span style="display:inline-block;width:50%;text-align:right">支付剩余时间：</span>20:00</div>
    <div class="OrderInfoPaySingle"><span style="display:inline-block;width:50%;text-align:right">金额：</span><span style="font-weight:bolder;color:#000000">￥</span><span style="font-size:xx-large;font-weight:bolder;color:#000000">88.88</span></div>
    <div class="OrderInfoPaySingle"><span style="display:inline-block;width:50%;text-align:right">商家：</span>一品香粥</div>
    <div class="OrderInfoPaySingle"><span style="display:inline-block;width:50%;text-align:right">单号：</span>201705270202011001</div>-->
      <div class="clear"></div>
    </div>
    <div class="PayWay">

      <div>
        <img style="float:left" src="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1590644064945&di=1fa1afa6e5b5a3451bae65dc6e7a7668&imgtype=0&src=http%3A%2F%2Fimg011.21cnimg.com%2Fphoto%2F2017%2F0413%2F13%2F201217faf3a7a6a62c949082_o.jpg" height="48" width="48" slot="icon">
        <mt-radio style="width:100%;" align="right" title="" v-model="mtRadioSel" :options="wxPay">
        </mt-radio>
       
      </div>

      <!--<div style="width:100%">
        <img style="float:left" src="http://img.zcool.cn/community/01db1c5ad0dab9a8012138678d8db9.png@2o.png" height="48" width="48" slot="icon">
        <mt-radio style="width:100%" align="right" title="" v-model="mtRadioSel" :options="Alipay">
        </mt-radio>
      </div>-->

    </div>
    <mt-button style="margin-top:20px" type="danger" size="large" @click="pay">确认支付</mt-button>
    <div class="clear"></div>
  </div>
 
</template>
<script>
  import { GetUnifiedOrderResult } from 'api'
  import moment from 'moment'
  import { loadLocal } from '@/common/js/storage'
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
        ],
        restPayTime: '',
        IntervalId: '',
        StartTime: moment()
      }
    },
    props: {
    },
    computed: {
      orderInfo() {
        return {
          OrderCreateTime: this.$route.params.OrderCreateTime,
          OrderAmount: this.$route.params.OrderAmount,
          OrderId: this.$route.params.OrderId,
          SellerName: this.$route.params.SellerName
        }
      }
    },
    created() {
    },
    mounted() {
      var that=this
      that.IntervalId=setInterval(() => {
        that.diffTime()
      }, 1000)
    },
    methods: {
      diffTime() {
        var that = this
        let CurrentTime = moment()
        var _date = moment.duration(CurrentTime - that.StartTime, 'ms')
        if (this.restPayTime == '00：00') {
          
          clearInterval(that.IntervalId);
          this.$createDialog({
            type: 'alert',
            title: '过期提醒',
            content: '您的订单已过期，请您重新下单，谢谢！',
            icon: 'cubeic-warn',
            onConfirm: () => {
              that.$router.push({ path: "/App" });
            }
          }).show()
        }
        else {
          this.restPayTime = this.PrefixInteger((19 - _date._data.minutes), 2) + '：' + this.PrefixInteger((59 - _date._data.seconds), 2)
        }
      },
      pay: function () {
        var that = this
        console.log("OrderId: " + that.orderInfo.OrderId)
        GetUnifiedOrderResult({
          OrderId: that.orderInfo.OrderId,          TotalFee: '2',
          OpenId: that.$store.state.userInfo.openid,
          TradeType:'JSAPI'
        }).then((parms) => {
          wx.chooseWXPay({
            timestamp: parms.timeStamp, // 支付签名时间戳，注意微信jssdk中的所有使用timestamp字段均为小写。但最新版的支付后台生成签名使用的timeStamp字段名需大写其中的S字符
            nonceStr: parms. nonceStr, // 支付签名随机串，不长于 32 位
            package: parms.package, // 统一支付接口返回的prepay_id参数值，提交格式如：prepay_id=\*\*\*）
            signType: parms.signType, // 签名方式，默认为'SHA1'，使用新版支付需传入'MD5'
            paySign: parms.paySign, // 支付签名
            success: function (res) {
              that.$router.push('PayResult');
            }
          });
        })
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
