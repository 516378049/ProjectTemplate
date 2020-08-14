<template>
  <div class="container">
    <!--优惠券弹窗-->
    <cube-popup type="my-popup" :mask-closable="true" ref="cubePop">

      <div :style="popStyle">
        <mt-header :title="popTitle" style="font-size:large;">
          <mt-button icon="mintui mintui-quxiao" slot="right" @click="hidePopup"></mt-button>
        </mt-header>
        <div style="padding:10px">
          <slot></slot>
        </div>
        

        <div class="clear"></div>
      </div>
    </cube-popup>
  </div>
  
</template>
  <script>
    export default {
      props: {
        popTitle: {
          type: String,
          default: ''
        },
        width: {
          type: String,
          default: '80'
        },
        height: {
          type: String,
          default: '37'
        }
      },
      data() {
        return {}
      },
      created() {
        
      },
      mounted() {
       
      },
      computed: {
        popStyle() {
          let that =this
          return {
            width: that.width+'vw',
            height: that.height == 'auto' ? 'auto' : that.height+'vh',
            backgroundColor: 'rgb(251, 230, 220)'  /*此处不能用background-color 需要用驼峰命名*/
          }
        }
      },
      watch: {
      
      },
      methods: {
        //弹窗
        showPopup(item) {
          if (item) {
            this.popTitle = item
          }

          const component = this.$refs.cubePop
          component.show()
          setTimeout(() => {
            component.hide()
          }, 3600000)
        },
        hidePopup() {
          this.$refs.cubePop.hide()
        }
      },
      components: {
      }
    }
  </script>

  <style lang="stylus" rel="stylesheet/stylus">
  </style>
