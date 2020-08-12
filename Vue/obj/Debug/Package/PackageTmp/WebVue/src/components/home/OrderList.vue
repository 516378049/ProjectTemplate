<template>
  <div class="container">
    <div style="position:fixed;top:0;left:0;width:100%; height:61.8%; text-align:center;background-image:linear-gradient(rgb(167, 216, 255), rgb(255, 255, 255));z-index:-1">
    </div>
    <mt-header title="我的订单" style="background-color:inherit;font-size:large;color:#000000;">
    </mt-header>
    <div class="start" style="margin-top:0.618px"></div>

    <mt-cell class="bolder" style="background-color:inherit;border-bottom: 1px outset #f00;" title=所有订单></mt-cell>
    <div class="OrderlistDiv_Home scroll-list-wrap">
      <cube-scroll ref="scroll" :options="options" @pulling-down="onPullingDown" @pulling-up="onPullingUp">

        <!--<template slot="pulldown" slot-scope="props">
          <div v-if="props.pullDownRefresh" class="cube-pulldown-wrapper"  style="top:0px">
            <div class="pulldown-content">
              <span v-if="props.beforePullDown">{{ pullDownTip }}</span>
              <template v-else>
                <span v-if="props.isPullingDown">正在更新...</span>
                <span v-else>更新成功~~</span>
              </template>
            </div>
          </div>
        </template>-->

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
                <router-link :to="{name:'OrderDetail',params:{orderNum:item.OrderNum}}">
                  <div style="margin-top:5px" v-if="'012'.indexOf(index)>=0">
                    <span style="font-size:small;color:rgba(0,0,0,.5)">{{details.FoodName}}</span>
                    <span style="float:right;font-size:small">x {{details.Count}}</span>
                  </div>
                </router-link>
              </template>

              <div style="margin-top:6.18px">
                <span style="font-size:xx-small;color:rgba(0,0,0,.5)">{{item.OrderDetailsInfo.length>3?'等'+item.OrderDetailsInfo.length+'件商品':'共'+item.OrderDetailsInfo.length+'件商品'}}</span>
                <span style="float:right;font-size:small">总额： <span style="font-weight:bolder;color:rgba(0,0,0,1);">￥{{item.Amount}}</span></span>
              </div>
              <div style="margin-top:5px;width:100%;text-align:right;">
                <mt-button style="" type="primary" size="small" @click="roterPush('App');">再来一单</mt-button>
                <mt-button plain style="margin-left:10px" type="default" size="small" @click="roterPush('OrderDetail',{orderNum:item.OrderNum});"> 订单明细</mt-button>
                <div class="clear"></div>
              </div>
              <div style="margin-top:15px;text-align:right;font-size:small;color:rgba(0,0,0,.5)">下单时间：{{item.CreateTime|dateFormat('yyyy-MM-dd HH:mm')}}</div>
              <div class="clear"></div>
            </div>
            <div class="clear"></div>
          </div>
        </template>
        <div style="margin-top:15px;margin-bottom:10px; text-align:center;font-size:small;color:rgba(0,0,0,.5)">{{scrollInfo}}</div>
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
          scrollInfo: '',
          selected: "MyOrderList",
          startY: 0,
          scrollbarFade: true,
          scrollHeight: "100%",
          pullAction: {
            pullDown: true,
            pullUp: true,
            action:''
          }
        }
      },
      props: {
      },
      created() {
        this.scrollHeight =  "calc(100% - 132px)"
      },
      mounted() {
        if (this.getOrderInfoList.length == 0) {
          this.OrderInfoList('')
        }
        else {
          this.OrderInfoList('down')
        }
      },
      computed: {
        options() {
          var that=this
          return {
            scrollbar: this.scrollbarFade,
            startY: this.startY,
            pullDownRefresh:  {
              threshold: 60,
              stop: 40,
              stopTime: 1000,
              txt: "更新成功"
            },
            //pullDownRefresh: that.pullAction.pullDown ?{
            //  threshold: 60,
            //  stop: 40,
            //  stopTime:1000,
            //  txt:"更新成功"
            //} : false,
            pullUpLoad: that.pullAction.pullUp ? {
              threshold: 100,
              visibale: true,
              txt: { more: '更新成功！', nomore:'更新成功...' }
            } : false
          }
        },
        getOrderInfoList() {
          return this.$store.getters.getOrderInfoList
        }
      },
      filters: {

      },
      watch: {
        selected() {
          if (this.selected == 'MealList') {
            this.$router.push('App')
          }
        },
        scrollHeight() {
          $(".scroll-list-wrap").css({ "height": this.scrollHeight })
        },
        getOrderInfoList(newVal, oldVal) {
          if (this.pullAction.action == 'up' && newVal.length == oldVal.length) {
            this.pullAction.pullUp = false;
            this.scrollInfo ='无有更多的啦！'
          }
          console.log('OrderInfoList watch');
          console.log(newVal);
          console.log(oldVal);
        }
      },
      methods: {
         OrderInfoList(slipAction) {
          this.$store.dispatch('a_getOrderInfoList', {
            slipAction: slipAction
          })
           setTimeout(() => {
             if (this.$refs.scroll) {
               this.$refs.scroll.forceUpdate();
               this.$refs.scroll.refresh();
             }

           }, 1000)

         
        },
        onPullingDown() {
          this.pullAction.action ='down'
          this.OrderInfoList('down')
        },
        onPullingUp() {
          this.pullAction.action = 'up'
          this.OrderInfoList('up')
        },
        //订单按时间排序
        OrderListOrderBy(orderList) {
          orderList.sort(function (a, b) {
            return Date.parse(b.CreateTime) - Date.parse(a.CreateTime);//时间降序
          })
        },
        roterPush(name, params) {
          this.$router.push({ name: name, params: params})
        }
      },
      components: {}
    }
  </script>

  <style lang="stylus" rel="stylesheet/stylus">
    .container .OrderlistDiv_Home .OrderlistDivSingle {
      box-sizing: border-box;
      border-radius: 30px 30px;
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
