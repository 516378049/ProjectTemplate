<template>
  <div class="container">
    <div style="position:fixed;top:0;left:0;width:100%; height:61.8%; text-align:center;background-image:linear-gradient(rgb(167, 216, 255), rgb(255, 255, 255));z-index:-1">
    </div>
    <mt-header title="我的订单" style="background-color:inherit;font-size:large;color:#000000;">
    </mt-header>
    <div class="start" style="margin-top:0.618px"></div>

    <mt-cell class="bolder" style="background-color:inherit;border-bottom: 1px outset #f00;" title=所有订单></mt-cell>
    <div class="OrderlistDiv_Home scroll-list-wrap">
      <cube-scroll ref="scroll" :options="options">
        <template v-for="item in this.getOrderInfoList">
          <div class="OrderlistDivSingle">
            <div class="OrderlistDivSingleL">
              <img slot="icon" :src="item.avatar" width="50" height="50">
              <div class="clear"></div>
            </div>
            <div class="OrderlistDivSingleR">
              <div style="margin-top:5px;margin-bottom:10px">
                <i class="mintui" style="font-size:large">{{item.SellerName}}</i>
                <i class="mintui mintui-icon-test2" style="font-size:large"></i>
                <span style="float:right;font-size:small">{{item.Status|StatuStr}}</span>
              </div>
              <template v-for="(details,index) in item.OrderDetailsInfo">
                <div style="margin-top:5px" v-if="'012'.indexOf(index)>=0">
                  <span style="font-size:small;color:rgba(0,0,0,.5)">{{details.FoodName}}</span>
                  <span style="float:right;font-size:small">x1</span>
                </div>
              </template>
              <div style="margin-top:6.18px">
                <span style="font-size:xx-small;color:rgba(0,0,0,.5)">{{item.OrderDetailsInfo.length>3?'等4件商品':'共'+item.OrderDetailsInfo.length+'件商品'}}</span>
                <span style="float:right;font-size:small">总额： <span style="font-weight:bolder;color:rgba(0,0,0,1);">￥{{item.Amount}}</span></span>
              </div>
              <div style="margin-top:5px">
                <mt-button plain style="float:right" type="default" size="small">再来一单</mt-button>
                <div class="clear"></div>
              </div>
              <div style="margin-top:15px;text-align:right;font-size:small;color:rgba(0,0,0,.5)">下单时间：{{item.CreateTime|dateFormat('yyyy-MM-dd HH:mm')}}</div>
              <div class="clear"></div>
            </div>
            <div class="clear"></div>
          </div>
        </template>
        <div style="margin-top:15px;margin-bottom:10px; text-align:center;font-size:small;color:rgba(0,0,0,.5)">无有更多的啦！</div>
      </cube-scroll>
    </div>
      <div class="clear"></div>
    </div>

  </template>
  <script>
    import moment from 'moment'
    export default {
      data() {
        return {
          selected: "MyOrderList",
          startY: 0,
          scrollbarFade: true,
          scrollHeight: "100%"
        }
      },
      props: {
      },
      created() {
        this.scrollHeight =  "calc(100% - 132px)"
      },
      mounted() {
        console.log(this.$store.state.OrderInfoList.length)
        if (this.$store.state.OrderInfoList.length==0) {
          this.OrderInfoList()
        }
      },
      computed: {
        options() {
          return {
            scrollbar: this.scrollbarFade,
            startY: this.startY
          }
        },
        getOrderInfoList() {
          return this.$store.state.OrderInfoList
        }
      },
      filters: {
        //订单状态：1、待支付、2、商家待接单；3、商家已接单；4、订单完成；5、待评价；6、已评价；7、取消订单；8、申请退款；9、商家同意退款；10、退款成功
        StatuStr: function (value) {
          switch (value) {
            case 1: return '待支付';
            case 2: return '商家待接单';
            case 3: return '商家已接单';
            case 4: return '订单完成';
            case 5: return '待评价';
            case 6: return '已评价';
            case 7: return '已取消';
            case 8: return '退款中';
            case 9: return '商家同意退款';
            case 10: return '退款成功';
            default: return '未知状态'
          }
        }
      },
      watch: {
        selected() {
          if (this.selected == 'MealList') {
            this.$router.push('App')
          }
        },
        scrollHeight() {
          $(".scroll-list-wrap").css({ "height": this.scrollHeight })
        }
      },
      methods: {
        OrderInfoList() {
          var that=this
          this.$store.commit('getOrderInfoList', {
            startTime: that.$options.filters['dateFormat'](new Date(),'yyyy-MM-dd HH:mm'),
            slipAction: 'down'
          })
        },
        //订单按时间排序
        OrderListOrderBy(orderList) {
          orderList.sort(function (a, b) {
            return Date.parse(b.CreateTime) - Date.parse(a.CreateTime);//时间降序
          })
        }
      },
      components: {}
    }
  </script>

  <style lang="stylus" rel="stylesheet/stylus">
    .container .OrderlistDiv_Home .OrderlistDivSingle {
      box-sizing: border-box;
      color: black;
      margin-top: 5px;
      padding-top: 10px;
      padding-right: 20px;
      padding-bottom: 10px;
      border-bottom: 1px solid rgba(0,0,0,.1);
      width: 100%;
      background-image: linear-gradient(rgb(167, 216, 255), rgb(255, 255, 255));
      box-shadow: rgba(220, 218, 139, 0.54) 0px 0px 15px;
      -webkit-box-shadow: rgba(220, 218, 139, 0.54) 0px 0px 15px;
    }
    .container .OrderlistDiv_Home {
      box-sizing: border-box;
    }

      .container .OrderlistDiv_Home .OrderlistDivSingle .OrderlistDivSingleL {
        box-sizing: border-box;
        width: 80px;
        height: 100%;
        text-align: center;
        float: left;
      }

      .container .OrderlistDiv_Home .OrderlistDivSingle .OrderlistDivSingleR {
        width: calc(100% - 80px);
        float: left;
      }
  </style>
