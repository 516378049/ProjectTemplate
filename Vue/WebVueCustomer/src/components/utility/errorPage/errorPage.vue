<template>
  <div class="container">
    <div style="position:fixed;top:0;left:0;width:100%; height:100%; text-align:center;color:azure;background-image:linear-gradient(#8b4141,aliceblue);z-index:-1">
    </div>
    <mt-header title="错误页面" style="font-size:large;background-color:inherit">
      <router-link to="/" slot="left">
        <mt-button icon="back"></mt-button>
      </router-link>
    </mt-header>

    <div class="start" style="margin-top:70px"></div>
    <div class="errorContent">
      您访问的页面出错啦，我们会收集错误信息，并尽快改善,您也可以
      <router-link to="/wxLogin" style="color:#ffd800;">
        重新登录
      </router-link>
    </div>
    <div class="scroll-list-wrap" style="height:calc(100% - 150px);margin-top:10px">
      <cube-scroll ref="scroll" :options="options">
        <div class="errorMessage" v-html="errorContent">

        </div>
      </cube-scroll>
    </div>
  </div>
</template>

<script type="text/javascript">
  export default {
    data() {
      return {
        startY: 5,
        scrollbarFade: true
      }
    },
    computed:
    {
      options() {
        return {
          scrollbar: this.scrollbarFade,
          startY: this.startY
        }
      },
      errorContent() {
        if (this.$route.query.data) {
          console.log('错误信息：')
          console.log(this.$route.query.data)
          return '错误位置：' + this.$route.query.data.errLocation
            + '<br>错误编码：' + this.$route.query.data.RetCode
            + '<br>错误信息：请查看log'
            + '<br>错误信息：'
            + '<br>'+JSON.stringify(this.$route.query.data.RetMsg)
        }
        return '未知错误信息'
      }
    }
  }
</script>

<style lang="stylus" scoped>
  .errorContent {
    width: 100%;
    text-align: center;
    font-size:small ;
    line-height: 20px;
    color: #122938
  }
  .errorMessage {
    width: 100%;
    text-align: center;
    font-size:medium;
    line-height: 22px;
    color: #122938
  }
    
</style>
