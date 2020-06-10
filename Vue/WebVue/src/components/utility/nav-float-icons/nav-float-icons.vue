<template>
  <div>
    
    <float-icons padding="10 10 60 10" class="icons-warp">


      <div class="float-icon-item NavFirst">
        <img slot="icon" src="https://unpkg.com/@icon/font-awesome/icons/bars.svg" @click="showNavIcons" onblur="hideNavIcons" alt="">
        <img slot="icon" src="https://unpkg.com/@icon/font-awesome/icons/eject.svg" @click="showNavIcons" onblur="hideNavIcons" alt="" hidden>
        <span>导航</span>
        <span hidden>导航</span>
      </div>

      <div class="float-icon-item navIcons" style="display:none">
        <img slot="icon" src="https://unpkg.com/@icon/font-awesome/icons/list-ul.svg" alt="" @click="handleIcons('home')">
        <span>订单</span>
      </div>

      <!--<div class="float-icon-item navIcons" style="display:none">
    <img slot="icon" src="https://unpkg.com/@icon/font-awesome/icons/user-circle.svg">
    <span>会员</span>
  </div>-->
    </float-icons>
  </div>
</template>

<script>
  import FloatIcons from '@/components/utility/nav-float-icons/s-icons'

  export default {
    data(){
        return{
            opacity:0.618
      }
    },
    components: {
      'float-icons': FloatIcons
    },
    methods: {
      // 点击按钮
      handleIcons(router) {
        this.$router.push(router)
      },
      showNavIcons: function () {
  console.log(this.opacity)
        if (this.opacity == 1) {
          this.opacity=0.618
        }
        else {
          this.opacity=1
        }
        $(".icons-warp .float-icon-item.NavFirst img").toggle()
        $(".icons-warp .float-icon-item.NavFirst span").toggle()

        $(".icons-warp").toggleClass("toggleNav")
        $(".icons-warp .float-icon-item.navIcons").toggle()
      },
      hideNavIcons: function () {
        alert()
      }
    },
    watch:{
        opacity:function(){
                $(".icons-warp").css("opacity", `${this.opacity}`)
        }
    }
  }
</script>
<style lang='stylus' scoped>

  .toggleNav {
    padding-top: 5px;
    padding-bottom: 5px;
  }

  .icons-warp {
    border-radius: 25px;
    opacity: 0.618;
  }
    .icons-warp .float-icon-item {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      position: relative;
      width: 50px;
      height: 50px;
    }
      .icons-warp .float-icon-item img {
        width: 25px;
        height: 25px;
      }
    .icons-warp .float-icon-item span {
      font-size: 9px;
      color: #666666;
    }

</style>
