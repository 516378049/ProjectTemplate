<template>
  

  <div style="align-items:center;height:100%">
    <div style="position:fixed;top:0;left:0;width:100%; height:61.8%; text-align:center;color:azure;background-image:linear-gradient(#26a2ff,aliceblue);z-index:-1">
    </div>
    <mt-header title="订单确认" style="font-size:large;background-color:inherit">
      <router-link to="/App" slot="left">
        <mt-button icon="back"></mt-button>
      </router-link>
    </mt-header>

    <div class="start" style="margin-top:10px"></div>

    <div class="scroll-list-wrap">
      <cube-scroll ref="scroll" :options="options">
        <div class="listDiv">
          <mt-cell class="OrderInfoList" title="用餐时间" is-link :value=OrderTime.OrderTimeTxt @click.native="openPicker()"></mt-cell>
          <mt-cell class="OrderInfoList" title="用餐人数" is-link :value=OrderCusCount.OrderCusCountTxt @click.native="showPicker(customCount)"></mt-cell>

          <mt-cell class="OrderInfoList" title="订单备注" is-link :value="OrderRemark==''?'口味、偏好等要求':OrderRemark"  @click.native='showPopup()'>
          </mt-cell>

          <mt-cell class="OrderInfoList" title="支付方式" is-link :value="OrderPayWay.text"  @click.native="showPicker(PayWay)"></mt-cell>
          <!--<mt-cell class="OrderInfoList" title="发票信息" is-link value="未选择"></mt-cell>-->
        </div>

        <div class="listDiv">
          <mt-cell class="bolder" :title="getSeller.name+'（订单明细）'" style="border-bottom:2px outset rgba(0, 43, 247, 0.0618)"></mt-cell>
          <template v-for="item in selMenuList">
            <mt-cell class="orderList" :title=item.name>
              <span class="orderNum">x {{item.count}}</span> <span class="orderSingleAmount">￥{{item.price * item.count}}</span>
              <img slot="icon" :src="item.image" width="50" height="50">
            </mt-cell>
          </template>

          <mt-cell v-if="this.isUseDiscount==1"  @click.native='UseDiscount(0)' class="" is-link>
            <span  class="orderNum">用户随机优惠：</span> <span style="color:orangered">-￥{{AmountDiscount}}</span>
          </mt-cell>
          <mt-cell v-if="this.isUseDiscount==0" @click.native='UseDiscount(1)'  class="" is-link>
            <span class="orderNum">不使用优惠券：</span> <span style="color:orangered">-￥{{AmountDiscount}}</span>
          </mt-cell>

          <mt-cell class="">
            <span class="orderNum">合计：</span> <span style="color:#141f06;font-weight:bold">￥{{AmountReal}}</span><span style="color:#141f06;font-size:x-small;margin-left:5px">元</span>
          </mt-cell>

          <!--<div style="margin-top:15px;text-align:center;font-size:small;color:rgba(0,0,0,.5)">无有更多的啦！</div>-->
        </div>
      </cube-scroll>
    </div>
    <div>
      <cube-popup type="my-popup" position="bottom" :mask-closable="true" ref="OrderRemark">
        <cube-textarea v-model="OrderRemark"
                       placeholder="口味、偏好等要求"
                       :maxlength="30"
                       :readonly="false"
                       :disabled="false"
                       :autofocus="true"
                       style="height:100px">
        </cube-textarea>
        <cube-button :primary="true" @click="hidePopup">填好了</cube-button>
      </cube-popup>

    </div>
    <div class="bottomCommitOrder">
      <div class="PriceTotel">
        <span>￥{{AmountReal}}</span>
        <span style="font-size:x-small">元</span>
        <span style="font-size:smaller;color:#e0caca;margin-left:15px">| 已优惠￥{{AmountDiscount}}</span>
      </div>
      <mt-button style="float:right" type="primary" @click="prePay">确认订单</mt-button>
    </div>

  </div>
</template>

<script type = "text/javascript">
  import { CreateOrderInfo } from '@/api'
  import moment from 'moment'
  import { loadLocal } from '@/common/js/storage'


  export default {
    name: 'order-confirm',
    data(){
      return {
        isdoing: false,
        OrderTime: {
          OrderTimeTxt: '现在',
          OrderTimeVal: ''
        },
        OrderCusCount: {
          OrderCusCountTxt: '1人',
          OrderCusCountVal: '1'
        },
        OrderRemark: "",
        OrderPayWay: { text:'线上支付',value:1},
        startY: -5,
        scrollbarFade: true,
        scrollHeight: 0,
        isUseDiscount:1
      }
    },
    
    created() {
      this.scrollHeight = "calc(100% - 100px)"
    },
    mounted() {
    
    },

    computed: {
      options() {
        return {
          scrollbar: this.scrollbarFade,
          startY: this.startY
        }
      },
      selMenuList() {
        return this.$store.getters.getSelMenuList
      },
      TotleMount() {
        let _TotleMount= this.$store.getters.getSelMenuList.map((item) => { return item.price * item.count }).reduce((sum, number) => {
          return sum + number;
        }, 0)
        return _TotleMount.toString()
      },
      AmountDiscount() {
        let discountAmount = this.TotleMount - this.RandAmount
        return discountAmount.toString()
      },
      AmountReal() {
        let relmount = this.RandAmount
        return relmount.toString()
      },
      //随机产生1-10分钱
      RandAmount() {
        let discountAmount = 0.0
        while (discountAmount == 0) {
          discountAmount = Math.random() / 10
          discountAmount = this.ParseNumber(discountAmount, 2)
        }
        console.log(discountAmount)
        if (this.isUseDiscount == 0) { //不使用优惠券
          return this.TotleMount
        }
        else {
          return discountAmount
        }

      },
      getSeller() {
        let seller = loadLocal('seller')
        return loadLocal('seller_' + seller.id)
      },
      customCount() {
        var _data = []
        for (var i = 1; i <= 50; i++) {
          _data.push({ text: i + '人', value: i })
        }
        var params = {
          title: '用餐人数',
          data: [_data],
          onSelect: this.selectHandle,
          onCancel: this.cancelHandle
        }
        return params
      },
      PayWay() {
        var params = {
          title: '支付方式',
          data: [[
            { text: '在线支付', value: 1 }
            //,{ text: '线下支付', value: 2 }
          ]],
          onSelect: this.selectHandlePayWay,
          onCancel: this.cancelHandlePayWay
        }
        return params
      }
    },
    watch: {
      scrollHeight: function(){
        $(".scroll-list-wrap").css("height", this.scrollHeight)
      }
    },
    methods: {
      //使用优惠券
      UseDiscount: function (val) {
        this.isUseDiscount = val;
      },
      ParseNumber: function (num, numFixed) {
        let numStr = num.toString()
        let index = numStr.indexOf('.')
        return  Number(numStr.slice(0, index + numFixed+1))
      },
      prePay: function () {
        let that = this

        if (that.isdoing) {
          return false
        }
        else {
          that.isdoing = true;
        }
        let toast = this.$createToast({
          time: 0,
          txt: '正在创建订单......'
        })
        toast.show()

        //订单编号：年月日时分秒+商家ID+桌号
        let seller = loadLocal('seller')
        let sellerMod = loadLocal('seller_' + seller.id)
        let _OrderId = moment(new Date()).format('YYYYMMDDHHMMSS') + seller.id + seller.deskNumber
        let _Amount = that.TotleMount
        let _AmountReal = that.AmountReal
        let _AmountDiscount = that.AmountDiscount
        let _SellerId = seller.id
        let _SellerName = sellerMod.name
        let _avatar = sellerMod.avatar
        let _BookTime = that.OrderTime.OrderTimeVal == '' ? that.Global.Fun.dateFormat(new Date(), 'yyyy-MM-dd HH:mm') : that.OrderTime.OrderTimeVal
        //moment(new Date()).format('YYYY-MM-DD HH:MM:SS')
        //创建订单
        CreateOrderInfo(
          {
            orderinfo: {
              OrderNum: _OrderId,
              DeskNumber: seller.deskNumber,
              Amount: _Amount,
              AmountDiscount: _AmountDiscount,
              AmountReal: _AmountReal,
              SellerId: _SellerId,
              SellerName: _SellerName,
              avatar: _avatar,
              PayWay: that.OrderPayWay.value,
              BookTime: _BookTime,
              CusCount: that.OrderCusCount.OrderCusCountVal,
              Remark: that.OrderRemark
            },
            l_OrderDetailsInfo: that.$store.getters.getSelMenuList.map((X) => {
              return {
                GoodId: X.goodId,
                GoodName: that.$store.state.goods.filter((Y) => Y.Id == X.goodId)[0].name,
                FoodId: X.Id,
                FoodName: X.name,
                image: X.image,
                Count: X.count,
                AmountTotal: X.count * X.price,
                AmountDiscount: 0,
                AmountReal: X.count * X.price,
                Remark: ''
              }
            })
          }
        ).then((X) => {
          this.$router.push({
            name: 'OrderPay', params: {
              OrderId: _OrderId,
              SellerName: _SellerName,
              OrderAmount: _AmountReal
            }
          });
          that.isdoing == false
          toast.hide()
        }).catch((X) => {
          that.isdoing == false
          toast.hide()
        })
      },
      openPicker() {
        this.$createTimePicker({
          showNow: true,
          minuteStep: 5,
          delay: 15,
          onSelect: (selectedTime, selectedText, formatedTime) => {
            this.OrderTime.OrderTimeTxt = selectedText
            this.OrderTime.OrderTimeVal = formatedTime
          },
          onCancel: () => {
          }
        }).show()
      },

      showPicker(obj) {
        //if (!this.picker) {
          this.picker = this.$createPicker(obj)
          this.picker.show()
        //}
      },
      selectHandlePayWay(selectedVal, selectedIndex, selectedText) {
        this.OrderPayWay.text = selectedText.join(', ')
        //this.OrderPayWay.value = selectedVal //mintui选择时
        this.OrderPayWay.value = selectedVal.join(', ')
      },
      cancelHandlePayWay() {
        //this.$createToast({
        //  type: 'correct',
        //  txt: 'Picker canceled',
        //  time: 1000
        //}).show()
      },

      selectHandle(selectedVal, selectedIndex, selectedText) {
        this.OrderCusCount.OrderCusCountTxt = selectedText.join(', ')
        this.OrderCusCount.OrderCusCountVal = selectedVal.join(', ')
      },
      cancelHandle() {
      },
      showPopup() {
        const component = this.$refs.OrderRemark
        component.show()
        setTimeout(() => {
          component.hide()
        }, 600000)
      },
      hidePopup() {
        this.$refs.OrderRemark.hide()
      }

    }
  }
</script>

<style lang="stylus" scoped>
  .listDiv {
    margin-bottom: 5px;
    
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
        /*width: 30px;*/
        /*margin-right:10px*/
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
