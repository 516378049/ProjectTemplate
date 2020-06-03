<template>
  <div>
    
    <float-icons padding="10 10 60 10" class="icons-warp">


      <div class="float-icon-item">
        <img slot="icon" src="https://unpkg.com/@icon/font-awesome/icons/bars.svg" @click="showNavIcons" onblur="hideNavIcons" alt="">
        <span>导航</span>
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
    components: {
      'float-icons': FloatIcons
    },
    methods: {
      // 点击按钮
      handleIcons(router) {
        console.log('router', router)
        this.$router.push(router)
      },
      showNavIcons: function () {
        if ($(".icons-warp").css("opacity") == 1) {
          $(".icons-warp").css("opacity", "0.618")
        }
        else {
          $(".icons-warp").css("opacity", "1")
        }
        $(".icons-warp").toggleClass("toggleNav")
        $(".icons-warp .float-icon-item.navIcons").toggle()
      },
      hideNavIcons: function () {
        alert()
      }
    }
  }
</script>
<style lang='stylus' scoped>

  .toggleNav {
    padding-top: 10px;
    padding-bottom: 10px;
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
        margin-bottom: 3px;
      }
    .icons-warp .float-icon-item span {
      font-size: 9px;
      color: #666666;
    }

</style>
