<template>
  

  <div style="align-items:center;height:100%">
    <div style="position:fixed;top:0;left:0;width:100%; height:61.8%; text-align:center;color:azure;background-image:linear-gradient(#26a2ff,aliceblue);z-index:-1">
    </div>
    <mt-header title="订单确认" style="font-size:large;background-color:inherit">
      <router-link to="/" slot="left">
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
          <mt-cell class="bolder" title="一品香粥（订单明细）" style="border-bottom:2px outset rgba(0, 43, 247, 0.0618)"></mt-cell>
          <template v-for="item in selMenuList">
            <mt-cell class="orderList" :title=item.name>
              <span class="orderNum">x {{item.count}}</span> <span class="orderSingleAmount">￥{{item.price}}</span>
              <img slot="icon" :src="item.image" width="50" height="50">
            </mt-cell>
          </template>
          <div style="margin-top:15px;text-align:center;font-size:small;color:rgba(0,0,0,.5)">无有更多的啦！</div>
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
        <span>￥{{TotleMount}}</span><span style="font-size:smaller;color:#e0caca;margin-left:15px">| 已优惠￥0</span>
      </div>
      <mt-button style="float:right" type="primary" @click="prePay">立即支付</mt-button>
    </div>

  </div>
</template>

<script type = "text/javascript">
  export default {
    name: 'order-confirm',
    data(){
      return {
        OrderTime: {
          OrderTimeTxt: '现在',
          OrderTimeVal:''
        },
        OrderCusCount: {
          OrderCusCountTxt: '1人',
          OrderCusCountVal: ''
        },
        OrderRemark: "",
        OrderPayWay: { text:'线上支付',value:1},
        startY: -5,
        scrollbarFade: true,
        scrollHeight:0
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
        return this.$store.getters.getSelMenuList.map((item) => { return item.price * item.count }).reduce((sum, number) => {
          return sum + number;
        }, 0)
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
      prePay: function () {
        this.$router.push('OrderPay');
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
        this.OrderPayWay.value = selectedVal
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
