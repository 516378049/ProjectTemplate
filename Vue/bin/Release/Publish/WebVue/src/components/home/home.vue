<template>
  <div class="container">

    <router-view />

    <mt-tabbar style="border-top: 1px solid rgba(0,0,0,.1);" v-model="SelectedMenu">
    <mt-tab-item id="MenuList">
      <svg style="width:20px;height:20px" class="icon svg-icon" aria-hidden="true">
        <use xlink:href="#mintui-caidan1"></use>
      </svg>
      <span style="display:block;margin-top:5px">菜单</span>
    </mt-tab-item>
    <mt-tab-item id="OrderList" :class="{'is-selected':this.currentRouter=='OrderList'}">
      <svg style="width:20px;height:20px" class="icon svg-icon" aria-hidden="true">
        <use xlink:href="#mintui-dingdan"></use>
      </svg>
      <span style="display:block;margin-top:5px">订单</span>
    </mt-tab-item>
    <mt-tab-item id="Me" :class="{'is-selected':this.currentRouter=='Me'}">
      <svg style="width:20px;height:20px;" class="icon svg-icon" aria-hidden="true">
        <use xlink:href="#mintui-wode1"></use>
      </svg>
      <span style="display:block;margin-top:5px">我的</span>
    </mt-tab-item>
  </mt-tabbar>
    <div class="clear"></div>
  </div>

  </template>
  <script>

    export default {
      data() {
        return {
          SelectedMenu:'',
          startY: 0,
          scrollbarFade: true,
          scrollHeight: "100%"
        }
      },
      props: {
      },
      created() {
        this.scrollHeight = "calc(100% - 150px)"
      },
      mounted() {
      },
      computed: {
        currentRouter() {
          //it should active the menu if it is the init page
          if (this.SelectedMenu != '') {
            return ''
          }

          var routerValue = this.$route.path
          if (routerValue.indexOf('Me')>-1) {
            return "Me"
          }
          else if (routerValue.indexOf('OrderList')>-1) {
            return "OrderList"
          }
        },
        options() {
          return {
            scrollbar: this.scrollbarFade,
            startY: this.startY
          }
        }
      },
      watch: {
        SelectedMenu() {
          if (this.SelectedMenu == 'OrderList') {
            this.$router.push({ path: '/Home/OrderList'})
          }
          else if (this.SelectedMenu == 'Me') {
            this.$router.push('/Home/Me')
          }
          else if (this.SelectedMenu == 'MenuList') {
            this.$router.push({ path: '/App' })
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
