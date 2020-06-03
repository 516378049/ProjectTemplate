<template>
  <div style="align-items:center;height:100%;">
    <div style="position:fixed;top:0;left:0;width:100%; height:61.8%; text-align:center;color:azure;background-image:linear-gradient(#26a2ff,aliceblue);z-index:-1">
    </div>
    <mt-header title="订单确认" style="font-size:large;background-color:inherit">
      <router-link to="/" slot="left">
        <mt-button icon="back"></mt-button>
      </router-link>
    </mt-header>

    <div class="start" style="margin-top:50px"></div>
    <div class="listDiv">
      <mt-cell class="OrderInfoList" title="用餐时间" to="" is-link value="立即用餐"></mt-cell>
      <mt-cell class="OrderInfoList" title="用餐人数" is-link value="1人"></mt-cell>
      <mt-cell class="OrderInfoList" title="订单备注" is-link value="口味、偏好等要求"></mt-cell>
      <mt-cell class="OrderInfoList" title="支付方式" is-link value="在线支付"></mt-cell>
      <mt-cell class="OrderInfoList" title="发票信息" is-link value="未选择"></mt-cell>
      <!--<mt-field class="OrderInfoList field" label="备注" placeholder="备注" type="textarea" rows="1"></mt-field>-->
      <!--<mt-cell class="OrderInfoList" title="备注" to="" is-link value="1人"></mt-cell>-->
    </div>
    <div class="listDiv">
      <mt-cell class="bolder" title=一品香粥></mt-cell>
      <mt-cell class="orderList" title="皮蛋瘦肉粥">
        <span class="orderNum">x1</span> <span class="orderSingleAmount">￥25</span>
        <img slot="icon" src="http://fuss10.elemecdn.com/c/cd/c12745ed8a5171e13b427dbc39401jpeg.jpeg?imageView2/1/w/114/h/114" width="50" height="50">
      </mt-cell>
      <mt-cell class="orderList" title="扁豆焖面">
        <span class="orderNum">x1</span> <span class="orderSingleAmount">￥7</span>
        <img slot="icon" src="http://fuss10.elemecdn.com/c/6b/29e3d29b0db63d36f7c500bca31d8jpeg.jpeg?imageView2/1/w/750/h/750" width="50" height="50">
      </mt-cell>
      <mt-cell class="orderList" title="VC无限橙果汁">
        <span class="orderNum">x1</span> <span class="orderSingleAmount">￥16</span>
        <img slot="icon" src="http://fuss10.elemecdn.com/e/c6/f348e811772016ae24e968238bcbfjpeg.jpeg?imageView2/1/w/750/h/750" width="50" height="50">
      </mt-cell>
      <div class="bottomCommitOrder">
        <div class="PriceTotel">
          <span>￥80.5</span><span style="font-size:smaller;color:#e0caca;margin-left:15px">| 已优惠￥16</span>
        </div>
        <mt-button style="float:right" type="primary" @click="prePay">立即支付</mt-button>
      </div>
    </div>
  </div>
</template>

<script type = "text/javascript">
  export default {
    name: 'order-confirm',
    //created: {
    //},
    methods: {
      prePay: function () {
        this.$router.push('OrderPay');
      }
    }
  }
</script>

<style lang="stylus" scoped>
  .listDiv {
    margin-bottom: 15px
  }
    .listDiv .orderList {
      margin-top: 5px
    }
      .listDiv .orderList .orderNum {
        margin-right: 15px;
        font-size: small;
        color: #000000
      }
      .listDiv .orderList .orderSingleAmount {
        font-weight: bolder;
        color: #000000;
        box-sizing: border-box;
        width: 30px;
      }
    .listDiv .OrderInfoList {
      margin-top: 5px;
    }
  .bottomCommitOrder {
    position: fixed;
    bottom: 0;
    padding-left: 10px;
    padding-top: 5px;
    padding-right: 10px;
    padding-bottom: 15px;
    box-sizing: border-box;
    width: 100%;
    height: 50px;
    background-color: #211f1f;
    color: #fff
  }

  .PriceTotel {
    display: inline-block;
    height: 41px;
    vertical-align: middle;
    padding-top: 10px
  }

</style>
