<template>
  <div id="testDiv1" style="width:100vh;height:100vh"  @touchstart="touchstart"  >
    <div class="inner1" @click="toggle($event)"></div>
    <div class="ball-container">
      <transition
               @before-enter="beforeDrop"
               @enter="dropping"
               @after-enter="afterDrop">

        <div class="ball" v-show="ball.show">
          <div class="inner inner-hook"></div>
        </div>
      </transition>
    </div>
  </div>
</template>

<script type = "text/javascript">
  export default {
    data() {
      return {
        ball: {
          show: false,
          el: null,
          x: 0,
          y: 0 

        }
      }
    },
    computed: {
      options() {
        return {
          
        }
      }
    },
    methods: {
      touchstart(e) {
        // 获取x 坐标
        e.targetTouches[0].clientX
        // 获取y 坐标
        e.targetTouches[0].clientY

        console.log(e.targetTouches[0].clientX)
        console.log(e.targetTouches[0].clientY)
        this.ball.x = e.targetTouches[0].clientX-100
        this.ball.y = -(window.innerHeight - e.targetTouches[0].clientY-100)
        this.ball.show = true
      },

      toggle(obj) {
        this.ball.show = true
        this.ball.el = obj.target
      },
      beforeDrop(el) {
        //const ball = this.dropBalls[this.dropBalls.length - 1]
        //const rect = this.ball.el.getBoundingClientRect()
        //const x = rect.left 
        //const y = -(window.innerHeight - rect.top)
        const x = this.ball.x
        const y = this.ball.y
        el.style.display = ''
        //el.style.transform = el.style.webkitTransform = `translate3d(${x},${y}px,0)`
        const inner = el.getElementsByClassName("inner-hook")[0]
        inner.style.transform = inner.style.webkitTransform = `translate3d(${x}px,${y}px,0)`
      },
      dropping(el, done) {
      
        this._reflow = document.body.offsetHeight
        //el.style.transform = el.style.webkitTransform = `translate3d(0,0,0)`
        const inner = el.getElementsByClassName("inner-hook")[0]
        inner.style.transform = inner.style.webkitTransform = `translate3d(0,0,0)`
        el.addEventListener('transitionend', done)
      },
      afterDrop(el) {
        const ball = this.ball
        if (ball) {
          ball.show = false
          el.style.display = 'none'
        }
      }
    }
  }
  
</script>

<style lang="stylus" rel="stylesheet/stylus">
  .ball-container
    .ball
      position: fixed
      left: 100px
      bottom: 100px
      z-index: 200
      transition: all .2s linear  /*transition: all .5s cubic-bezier(0.49, -0.29, 0.75, 0.41)*/
      .inner
        width: 16px
        height: 16px
        border-radius: 50%
        background: $color-blue
        transition: all 1s cubic-bezier(.66,.1,1,.41)/*all .5s cubic-bezier(0.49, -0.29, 0.75, 0.41)*/
   .inner1
      z-index: 201
      width: 16px
      height: 16px
      border-radius: 50%
      background: $color-blue
      margin-left:300px
      margin-top:300px
     
</style>
