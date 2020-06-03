<template>
  <div class="scroll-list-wrap">
    <cube-scroll ref="scroll"
                 :data="items"
                 :options="options">
    </cube-scroll>
  </div>
</template>

<script type = "text/javascript">
  const _data = [
    '😀 😁 😂 🤣 😃 😄 ',
    '🙂 🤗 🤩 🤔 🤨 😐 ',
    '👆🏻 scroll up/down 👇🏻 ',
    '😔 😕 🙃 🤑 😲 ☹️ ',
    '🐣 🐣 🐣 🐣 🐣 🐣 ',
    '👆🏻 scroll up/down 👇🏻 ',
    '🐥 🐥 🐥 🐥 🐥 🐥 ',
    '🤓 🤓 🤓 🤓 🤓 🤓 ',
    '👆🏻 scroll up/down 👇🏻 ',
    '🦔 🦔 🦔 🦔 🦔 🦔 ',
    '🙈 🙈 🙈 🙈 🙈 🙈 ',
    '👆🏻 scroll up/down 👇🏻 ',
    '🚖 🚖 🚖 🚖 🚖 🚖 ',
    '✌🏻 ✌🏻 ✌🏻 ✌🏻 ✌🏻 ✌🏻 '
  ]
  export default {
    data() {
      return {
        items: _data,
        scrollbarFade: true,
        startY: 0
      }
    },
    computed: {
      options() {
        return {
          scrollbar: this.scrollbarFade,
          startY: this.startY
        }
      }
     
    }
  }
  
</script>

<style lang="stylus" rel="stylesheet/stylus">
  .scroll-list-wrap {
    height:calc(100% - 120px);
    /*border: 1px solid rgba(0, 0, 0, 0.1);
    border-radius: 5px;*/
    transform: rotate(0deg);
  }
</style>
