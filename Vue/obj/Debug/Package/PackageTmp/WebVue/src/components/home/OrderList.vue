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
        <template v-for="count in 5">
          <div class="OrderlistDivSingle">
            <div class="OrderlistDivSingleL">
              <img slot="icon" src="http://fuss10.elemecdn.com/c/cd/c12745ed8a5171e13b427dbc39401jpeg.jpeg?imageView2/1/w/114/h/114" width="50" height="50">
              <div class="clear"></div>
            </div>
            <div class="OrderlistDivSingleR">
              <div style="margin-top:5px;margin-bottom:10px">
                <i class="mintui" style="font-size:large">一品香粥</i>
                <i class="mintui mintui-icon-test2" style="font-size:large"></i>
                <span style="float:right;font-size:small">订单已完成</span>
              </div>
              <div style="margin-top:5px">
                <span style="font-size:small;color:rgba(0,0,0,.5)">皮蛋瘦肉粥</span>
                <span style="float:right;font-size:small">x1</span>
              </div>
              <div style="margin-top:5px">
                <span style="font-size:small;color:rgba(0,0,0,.5)">扁豆焖面</span>
                <span style="float:right;font-size:small">x1</span>
              </div>
              <div style="margin-top:5px">
                <span style="font-size:small;color:rgba(0,0,0,.5)">VC无限橙果汁</span>
                <span style="float:right;font-size:small">x1</span>
              </div>
              <div style="margin-top:6.18px">
                <span style="font-size:xx-small;color:rgba(0,0,0,.5)">等4件商品</span>
                <span style="float:right;font-size:small">总额： <span style="font-weight:bolder;color:rgba(0,0,0,1);">￥88.89</span></span>
              </div>
              <div style="margin-top:5px">
                <mt-button plain style="float:right" type="default" size="small">再来一单</mt-button>
                <div class="clear"></div>
              </div>
              <div style="margin-top:15px;text-align:right;font-size:small;color:rgba(0,0,0,.5)">下单时间：2020-05-20 19:00</div>
              <div class="clear"></div>
            </div>
            <div class="clear"></div>
          </div>
        </template>
        <div style="margin-top:15px;text-align:center;font-size:small;color:rgba(0,0,0,.5)">无有更多的啦！</div>
      </cube-scroll>
    </div>
      <div class="clear"></div>
    </div>

  </template>
  <script>

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
        this.scrollHeight =  "calc(100% - 150px)"
      },
      mounted() {
        
      },
      computed: {
        options() {
          return {
            scrollbar: this.scrollbarFade,
            startY: this.startY
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
      },
      components: {}
    }
  </script>

  <style lang="stylus" rel="stylesheet/stylus">
    .container .OrderlistDiv_Home .OrderlistDivSingle {
      box-sizing: border-box;
      color: black;
      padding-top: 10px;
      padding-right: 20px;
      padding-bottom: 10px;
      border-bottom: 1px solid rgba(0,0,0,.1);
      width: 100%;
      background-image: linear-gradient(rgb(167, 216, 255), rgb(255, 255, 255))
    }

  </style>
