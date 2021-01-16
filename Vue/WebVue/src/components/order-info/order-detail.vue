<template>
  <div class="container" style="padding-bottom:0">
    <div style="position:fixed;top:0;left:0;width:100%; height:100%; text-align:center;background-image:linear-gradient(#94c1e4,#ffffff);z-index:-1">

    </div>
    <mt-header title="订单详情" style="background-color:inherit;font-size:large;color:#000000">
      <router-link to="" slot="left">
        <mt-button icon="back" @click.native="$router.back(-1)"></mt-button>
      </router-link>
    </mt-header>

    <div class="start" style="margin-top:30px"></div>
    <div class="scroll-list-wrap" style="height:calc(100% - 70px)">
      <cube-scroll ref="scroll" :options="options">
        <div class="PayResultInfo">
          <div class="PayResultInfoSingle" style="font-size:x-large;color:#296844">{{this.getOrderInfoByOrderNum.Status|StatuStr}}</div>
          <div class="PayResultInfoSingle" style="color:rgb(56, 95, 57)">{{restPayTime}}</div>
          <div class="PayResultInfoSingle"><span style="font-weight:700;color:#000000">桌号：</span><span style="font-size:xx-large;font-weight:bolder;color:#000000">{{this.getOrderInfoByOrderNum.DeskNumber|PrefixInteger(3)}}</span></div>

          <mt-button v-show="this.showButton.indexOf('cancel')>=0" plain style="margin-top:20px;margin-left:10px;margin-right:10px" type="default" size="small" @click="OrderCancel()">取消订单</mt-button>
          <mt-button v-show="this.showButton.indexOf('refound')>=0" plain style="margin-top:20px;margin-left:10px;margin-right:10px" type="default" size="small" @click="OrderRefound()">申请退款</mt-button>
          <mt-button v-show="this.showButton.indexOf('orderAgain')>=0" style="margin-top:20px;margin-left:10px;margin-right:10px" type="primary" size="small" @click="routerPush('App')">再来一单</mt-button>
          <mt-button v-show="this.showButton.indexOf('appraise')>=0" style="margin-top:20px;margin-left:10px;margin-right:10px" type="primary" size="small" @click="cubePop()">我要评价</mt-button>
          <mt-button v-show="this.showButton.indexOf('pay')>=0" style="margin-top:20px;margin-left:10px;margin-right:10px" type="primary" size="small" @click="routerPush('OrderPay',
            {
              OrderId: getOrderInfoByOrderNum.OrderNum,
              SellerName: getOrderInfoByOrderNum.SellerName,
              OrderCreateTime: getOrderInfoByOrderNum.CreateTime,
              OrderAmount: getOrderInfoByOrderNum.AmountReal
            })">立即支付</mt-button>
          <div class="clear"></div>

        </div>
        <br />
        <div class="OrderInfoDetails">
          <mt-cell class="bolder" :title="this.getOrderInfoByOrderNum.SellerName +'：(订单明细)'" style="border-radius: 30px 30px 0 0;"></mt-cell>
          <template v-for="item in this.getOrderInfoByOrderNum.OrderDetailsInfo">
            <mt-cell class="orderList" :title=item.FoodName>
              <span class="orderNum">x{{item.Count}}</span> <span style="width:15px"></span> <span class="orderSingleAmount">￥{{item.AmountReal}}</span>
              <img slot="icon" :src="item.image" width="50" height="50">
            </mt-cell>
          </template>
          <mt-cell title="" value="">
            <span>总金额：<span style="color:red">￥{{this.getOrderInfoByOrderNum.AmountReal}}</span></span>
          </mt-cell>
        </div>

        <div class="OrderInfoOther">
          <div class="OrderInfoOtherList title">订单其他信息：</div>
          <div class="OrderInfoOtherList content">店铺名称：{{getOrderInfoByOrderNum.SellerName}}</div>
          <div class="OrderInfoOtherList content">用餐时间：{{getOrderInfoByOrderNum.BookTime|dateFormat('yyyy-MM-dd HH:mm')}}</div>
          <div class="OrderInfoOtherList content">用餐人数：{{getOrderInfoByOrderNum.CusCount}}</div>
          <div class="OrderInfoOtherList content">订单备注：{{getOrderInfoByOrderNum.Remark}}</div>
          <div class="OrderInfoOtherList content">支付方式：在线支付</div>
          <div class="OrderInfoOtherList content">下单时间：{{getOrderInfoByOrderNum.CreateTime|dateFormat('yyyy-MM-dd HH:mm')}}</div>
          <div class="clear"></div>
        </div>
      </cube-scroll>
    </div>

    <cubePop ref="cubePop" popTitle="评价" width="100" height="auto">

      <div>
        <div style="display: flex;align-items:center">
          <div style="float:left;margin:auto 0">服务：</div>
          <div style="float:left;"><cube-rate v-model="Rateing.rateService"></cube-rate></div>
        </div>
        <div style="display: flex;">
          <div style="float:left;margin:auto 0">环境：</div>
          <div style="float:left;"><cube-rate v-model="Rateing.rateService"></cube-rate></div>
        </div>
        <div style="display: flex;">
          <div style="float:left;margin:auto 0">口味：</div>
          <div style="float:left;"><cube-rate v-model="Rateing.rateService"></cube-rate></div>
        </div>
        <br />
        <div style="display: flex;">
          <cube-textarea v-model="Rateing.text" :maxlength=1000 :autoExpand="autoExpand" :autofocus="autofocus" style="width:80vw;" placeholder="请留下您的宝贵意见~~ "></cube-textarea>
        </div>
        <br />
        <div style="display: flex;">
          <cube-upload action="#"
                       :auto="false"
                       :simultaneous-uploads="1"
                       :max=6
                       @file-click="fileClick"
                       @files-added="filesAdded" />
        </div>
        <div style="display: flex;">最多6张图片</div>
        <br />
        <cube-button primary @click="save">评价</cube-button>
      </div>
    </cubePop>


    <cube-popup type="my-popup" :mask-closable="true" ref="showImg">
      <img style="width:270px;height:270px" :src="imgUrl" />
    </cube-popup>
  </div>
 
</template>
<script>
  import { OrderChangeStatus, uploadFile } from '@/api'
  import cubePop from '@/components/utility/cubePopup/cubePop.vue'
import { setTimeout } from 'timers';

  export default {
    name: 'OrderPay',
    data() {
 
      return {
        restPayTime: '',
        TimeSetStop:false,
        IntervalId: '',

        imgUrl: "",
        fileList: [],
        autofocus: false,
        autoExpand: false,
        Rateing: {
          rateService: 5,
          rateComfortLevel: 5,
          rateTaste: 5,
          text:''
        }
      }
      
    },
    props: {
    },
    computed: {
      options() {
        return {
          scrollbar: true,
          startY: 0
        }
      },
      getOrderInfoByOrderNum() {
        
        var _orderNum = this.$route.params.orderNum
        if (_orderNum) {
          return this.$store.getters.getOrderInfoList.filter(X => X.OrderNum == _orderNum)[0]
        }
        else {
          return { status: '', CreateTime:new Date()}//防止属性返回没有值报错
        }
        
      },
      CreateTime() {
        var _orderNum = this.$route.params.orderNum
        if (_orderNum)
          return this.$store.getters.getOrderInfoList.filter(X => X.OrderNum == _orderNum)[0].CreateTime
      },
      showButton() {
        var _date = this.Global.Fun.dateDiff(new Date(), this.CreateTime, 'm')
        var options = []
        //订单待支付状态，可以取消，继续支付
        if (this.getOrderInfoByOrderNum.Status == 1) {
          options.push('cancel');
          if (_date < 20) {
            options.push('pay');
          }
          else {
            options.push('orderAgain');
          }
        }
        //订单已支付，等待商家接单状态，可以申请退款
        else if (this.getOrderInfoByOrderNum.Status == 2) {
          options.push('refound');
        }
        else {
          options.push('appraise');
          options.push('orderAgain');
        }
        return options
      },
      FileList: {
        get: function () { return this.fileList },
        set: function (value) { this.fileList.push(value) }
      }
    },
    created() {
      //console.log(this.getOrderInfoByOrderNum)
    },
    mounted() {
      if (!this.$route.params.orderNum) {
        this.$router.push('App')
      }
      else {
        this.diffTime()
      }
      
    },
    methods: {
      routerPush(name, params) {
        this.$router.push({ name: name, params: params })
      },
      diffTime() {
        var that = this
        var _date = that.Global.Fun.dateDiff(new Date(), that.CreateTime, 'm')
        if (that.getOrderInfoByOrderNum.Status == 1) {
          if (_date >= 20) {
            that.restPayTime = "订单超时，请重新下单"
          }
          else {
            that.restTimeDiff()
          }
        }
        else {
          that.restPayTime = "欢迎光临 ！"
        }
      },
      restTimeDiff() {
        var that = this
        var timeOutSnd = 20 * 60  //20分钟
        if (that.CreateTime && !that.TimeSetStop) {
          //var _date = that.Global.Fun.dateDiff(new Date(), that.CreateTime, 'm')
          //if (_date < 20) {
          var _dateSeconds = that.Global.Fun.dateDiff(new Date(), that.CreateTime, 's')
          let restSnd = timeOutSnd - _dateSeconds //剩下秒数
          //that.restPayTime = '剩余支付时间：' + that.$options.filters['PrefixInteger'](19 - _date, 2) + '：' + that.$options.filters['PrefixInteger'](59 - (_dateSeconds % 60), 2)
          let re = that.CountDown(restSnd)
          if (re != '') {
            that.restPayTime = '剩余支付时间：' + re
            setTimeout(() => {
              that.restTimeDiff()
            }, 1000)
          }
          else {
            that.$createDialog({
              type: 'alert',
              title: '过期提醒',
              content: '您的订单已过期，请您重新下单，谢谢~！',
              icon: 'cubeic-warn',
              onConfirm: () => {
                that.$router.push({ path: "/App" });
              }
            }).show()
          }
        }
        else {
        }
        // }
      },
      //倒计时
      CountDown(maxtime) {
        if (maxtime >= 0) {
          let minutes = Math.floor(maxtime / 60);
          let seconds = Math.floor(maxtime % 60);
          let msg =  minutes + "分" + seconds + "秒";
          return msg 
        } else {
          return ''
        }
      },
      OrderCancel() {
        var that=this
        var _orderNum = that.$route.params.orderNum
        if (_orderNum) {
          
          that.$createDialog({
              type: 'confirm',
              //icon: 'cubeic-alert',
              title: '取消订单',
              content: `确认取消订单？`,
              confirmBtn: {
                text: '确定',
                active: true,
                disabled: false,
                href: 'javascript:;'
              },
              cancelBtn: {
                text: '取消',
                active: false,
                disabled: false,
                href: 'javascript:;'
              },
              onConfirm: () => {
                OrderChangeStatus({ orderNum: _orderNum, status: 7 }).then(X=>{

                  that.$createToast({
                    type: 'correct',
                    txt: '订单已取消',
                    time: 2000
                  }).show()

                  //刷新订单
                  that.$store.dispatch('a_getOrderInfoList', {
                    slipAction: 'down'
                  })
                  that.restPayTime = "欢迎下次光临 ！"
                })
              },
              onCancel: () => {

              }
            }).show()
         
        }
       
      },
      OrderRefound() {
        var that = this
        var _orderNum = that.$route.params.orderNum
        if (_orderNum) {

          that.$createDialog({
            type: 'confirm',
            //icon: 'cubeic-alert',
            title: '申请退款',
            content: `确认申请退款？`,
            confirmBtn: {
              text: '确定',
              active: true,
              disabled: false,
              href: 'javascript:;'
            },
            cancelBtn: {
              text: '取消',
              active: false,
              disabled: false,
              href: 'javascript:;'
            },
            onConfirm: () => {
              OrderChangeStatus({ orderNum: _orderNum, status: 8 }).then(X => {

                that.$createToast({
                  type: 'correct',
                  txt: '已申请退款',
                  time: 2000
                }).show()

                //刷新订单
                that.$store.dispatch('a_getOrderInfoList', {
                  slipAction: 'down'
                })
                that.restPayTime = "已申请退款，等待商家处理 ！"
              })
            },
            onCancel: () => {

            }
          }).show()

        }

      },
      cubePop(option) {
        this.$refs.cubePop.showPopup()
      },
      filesAdded(files) {
        const that =this

        let formDataAdd = new FormData();

        for (let k in files) {
          const file = files[k]
          formDataAdd.append(file.name, file);
        }

        let toast = this.$createToast({
          time: 0,
          txt: '图片上传中......'
        })
        toast.show()

        uploadFile(formDataAdd).then((items) => {   //这里formDataAdd不能改成{formDataAdd}，否则后台接收不到文件
          toast.hide()
          //console.log(items)
          items.forEach((item) => {
            if (that.fileList.indexOf(item) == -1)
            { that.fileList.push(item) }
            console.log(item)
          })
        })
      },
      fileClick(files) {
        const that = this
        console.log(that.fileList)
        let toast = this.$createToast({
          time: 0,
          txt: '图片加载中......'
        })
        toast.show()
        let imgUrl=[]
        let currentImageIndex = 0
        let index=0
        that.fileList.forEach((item) => {
          if (item.Name.toString().trim() === files.name.toString().trim()) {
            currentImageIndex = index
          }
          imgUrl.push(item.Path)
          index++
        })
        //that.imgUrl = imgUrl
        //this.$refs.showImg.show()


        setTimeout(function () {
          that.imagePreview = that.$createImagePreview({
            imgs: imgUrl.filter((currentValue, indedx) => { return indedx == currentImageIndex }),
            initialIndex: 0
            //imgs: imgUrl.map(X => { return X }),
            //initialIndex: currentImageIndex
          })
          that.imagePreview.show()
          toast.hide()
        }, 100)

       
        
        console.log(currentImageIndex)
        console.log(imgUrl)
      },
      save() {
        alert("评价成功")
      }
    },
    beforeDestroy() {    //页面关闭时清除定时器  
      this.TimeSetStop == true;
    },
    components: {
      cubePop
    }
  }
</script>

<style lang="stylus" rel="stylesheet/stylus">
  .OrderInfoDetails {
    padding-left: 5px;
    padding-right: 5px;
    padding-bottom:15px;
    border-radius: 30px 30px;
    background-color:#ffffff
  }
  .OrderInfoOther {
    margin-top: 5px;
    padding: 10px 5px 30px 10px;
    border-radius: 30px 30px;
    background-color: #ffffff
  }
    .OrderInfoOther .OrderInfoOtherList {
      margin-top: 10px;
      color: rgba(0,0,0,.5)
    }
      .OrderInfoOther .OrderInfoOtherList.title {
        font-weight:bolder;
      }
      .OrderInfoOther .OrderInfoOtherList.content {
        font-size: small;
      }
</style>
