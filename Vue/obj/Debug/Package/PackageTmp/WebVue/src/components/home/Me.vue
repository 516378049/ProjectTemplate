<template>
  <div class="container">


    <div style="position:fixed;top:0;left:0;width:100%; height:210px; text-align:center;background-image:linear-gradient(rgb(30, 140, 228),#f6fbff);z-index:-1;-webkit-box-shadow: rgba(221, 210, 224, 0.1) 0px 0px 15px;
    box-shadow: rgba(221, 210, 224, 0.1) 0px 0px 15px">
    </div>
    <mt-header title="个人中心" style="background-color:inherit;font-size:large;">
      <mt-button style="color:#ff9494" icon="mintui mintui-Setting" slot="right"></mt-button>
    </mt-header>
    <div style="position:absolute;top:130px;display: flex;align-items:center;">
      <img style="height:48px; width:48px;border-radius:50%;" :src="userInfo.headimgurl"/><!--https://ss2.bdstatic.com/70cFvnSh_Q1YnxGkpoWK1HF6hhy/it/u=2832912555,407727011&fm=26&gp=0.jpg-->
      <div style="margin-left:10px">
        <div style="font-size:large;font-weight:bolder;color:#313e18;line-height:37px">长春</div>
        
        <div style="color:gray">地区：{{userInfo.province +' '+ userInfo.city}}</div>
      </div>
     
    </div>

    <div class="start" style="margin-top:170px"></div>

    <div class="HomeMeItem">
      <svg style="width:28px;height:28px" class="icon svg-icon" aria-hidden="true">
        <use xlink:href="#mintui-dingdan"></use>
      </svg>
      <mt-cell style="width:100%;background-color:inherit;border-bottom:1px solid rgba(232, 229, 229, 0.92);" title="订单" to="//github.com" is-link value="">
      </mt-cell>
    </div>
    <div class="HomeMeItem">
      <svg style="width:28px;height:28px" class="icon svg-icon" aria-hidden="true">
        <use xlink:href="#mintui-youhuiquan"></use>
      </svg>
      <mt-cell style="width:100%;background-color:inherit;border-bottom:1px solid rgba(232, 229, 229, 0.92);" title="优惠券" to="//github.com" is-link value="">
      </mt-cell>
    </div>
    <div class="HomeMeItem">
      <svg style="width:28px;height:28px" class="icon svg-icon" aria-hidden="true">
        <use xlink:href="#mintui-shezhi"></use>
      </svg>
      <mt-cell style="width:100%;background-color:inherit;border-bottom:1px solid rgba(232, 229, 229, 0.92);" title="设置" to="//github.com" is-link value="">
      </mt-cell>
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
        },
        userInfo() {
          return this.$store.state.userInfo
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
    .container .HomeMeItem {
      display: flex;
      align-items: center;
      background-image: linear-gradient(#edf9d97a, #fff)
    }

  </style>
