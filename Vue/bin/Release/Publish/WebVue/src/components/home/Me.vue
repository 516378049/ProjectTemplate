<template>
  <div class="container">


    <div style="position:fixed;top:0;left:0;width:100%; height:210px; text-align:center;background-image:linear-gradient(rgb(30, 140, 228),#f6fbff);z-index:-1;-webkit-box-shadow: rgba(221, 210, 224, 0.1) 0px 0px 15px;
    box-shadow: rgba(221, 210, 224, 0.1) 0px 0px 15px">
    </div>
    <mt-header title="个人中心" style="background-color:inherit;font-size:large;">
      <mt-button style="color:#ff9494" icon="mintui mintui-Setting" slot="right"></mt-button>
    </mt-header>
    <div style="position:absolute;top:130px;display: flex;align-items:center;">
      <img style="height:48px; width:48px;border-radius:50%;" :src="userInfo.headimgurl" />
      <div style="margin-left:10px">
        <div style="font-size:large;font-weight:bolder;color:#313e18;line-height:37px">{{userInfo.nikename}}</div>

        <div style="color:gray">地区：{{userInfo.province +' '+ userInfo.city}}</div>
      </div>

    </div>

    <div class="start" style="margin-top:170px"></div>

    <div class="HomeMeItem">
      <svg style="width:28px;height:28px" class="icon svg-icon" aria-hidden="true">
        <use xlink:href="#mintui-dingdan"></use>
      </svg>
      <mt-cell style="width:100%;background-color:inherit;border-bottom:1px solid rgba(232, 229, 229, 0.92);" title="订单" is-link value="" @click.native="routerPush('OrderList')"  >
      </mt-cell>
    </div>
    <div class="HomeMeItem">
      <svg style="width:28px;height:28px" class="icon svg-icon" aria-hidden="true">
        <use xlink:href="#mintui-youhuiquan"></use>
      </svg>
      <mt-cell style="width:100%;background-color:inherit;border-bottom:1px solid rgba(232, 229, 229, 0.92);" title="优惠券"  is-link value="" @click.native='showPopup("优惠中心")'>
      </mt-cell>
    </div>
    <div class="HomeMeItem">
      <svg style="width:28px;height:28px" class="icon svg-icon" aria-hidden="true">
        <use xlink:href="#mintui-shezhi"></use>
      </svg>
      <mt-cell style="width:100%;background-color:inherit;border-bottom:1px solid rgba(232, 229, 229, 0.92);" title="会员中心" is-link value="" @click.native='showPopup("会员中心")'>
      </mt-cell>
    </div>

    <div class="clear"></div>

    <!--优惠券弹窗-->
    <cube-popup type="my-popup" position="left" :mask-closable="true" ref="DiscountCoupon">
      <div style="width:100vw;height:100vh;background-color:#f6fbff;">
        <mt-header :title="SelectedItem" style="font-size:large;">
          <mt-button icon="back" slot="left" @click="hidePopup"></mt-button>
          <mt-button icon="mintui mintui-quxiao" slot="right" @click="hidePopup"></mt-button>
        </mt-header>
        <div v-if="SelectedItem=='优惠中心'">
          <cube-tab-bar style="background-color:aliceblue" v-model="selectedLabel" show-slider>
            <cube-tab v-for="(item, index) in tabs" :label="item.label" :key="item.label">
              <!--<i slot="icon" :icon="item.icon"></i>-->
              {{item.label}}
            </cube-tab>
          </cube-tab-bar>
          <cube-tab-panels style="" v-model="selectedLabel">
            <cube-tab-panel v-for="(item, index) in tabs" :label="item.label" :key="item.label">
              <div style="width:100%;text-align:center;margin-top:15px;padding:5px 0 5px 0">无有优惠信息</div>
            </cube-tab-panel>
          </cube-tab-panels>
        </div>
        <div class="HomeMePopup" v-if="SelectedItem=='会员中心'">
          <cube-form :model="model"
                     :schema="schema"
                     :options="options"
                     @submit="submitHandler">
          </cube-form>

          <cube-form :model="model"
                     :schema="schema"
                     :options="options"
                     @submit="submitHandler">
            <cube-form-group>
              <cube-form-item :field="fields[0]">
                <cube-button @click="showDatePicker" style="background-color:aliceblue;color:black;text-align:left">{{model.dateValue || '请选择日期'}}</cube-button>
              </cube-form-item>
            </cube-form-group>
          </cube-form>
          <div style="padding:0 10px 0 10px">
            <mt-button style="margin-top:20px;" type="danger" size="large" @click="commit">保存</mt-button>
          </div>
          <div class="clear"></div>
        </div>
      </div>
    </cube-popup>





  </div>
  
</template>
  <script>
    export default {
      data() {
        return {
          //优惠券属性
          selectedItem: "",

          selectedLabel: '全部',
          tabs: [
            {
              label: '全部',
              heroes: ['无有优惠信息']
            },
            {
              label: '已使用',
              heroes: ['无有优惠信息']
            },
            {
              label: '未使用',
              heroes: ['无有优惠信息']
            }
          ],
          selected: "MyOrderList",
          startY: 0,
          scrollbarFade: true,
          scrollHeight: "100%",
          //用户信息
          model: {
            nickname: "",
            sex:[],
            province:"",
            city:"",
            telnumber:"",
            birthDay:""
          },
          fields: [
            {
              modelKey: 'birthDay',
              label: '出生日期：',
              rules: {
                required: false
              }
            }],
          schema: {
            groups: [
              {
                //legend: '会员信息',
                fields: [
                  {
                    type: 'input',
                    modelKey: 'nickname',
                    label: '名称：',
                    props: {
                      placeholder: '请输入名称'
                    },
                    rules: {
                      required: false
                    },
                    // validating when blur
                    trigger: 'blur'
                  },
                  {
                    type: 'checkbox-group',
                    modelKey: 'sex',
                    label: '性别',
                    props: {
                      options: ['男', '女']
                    },
                    rules: {
                      required: false
                    }
                  },
                  {
                    type: 'input',
                    modelKey: 'province',
                    label: '省份：',
                    props: {
                      placeholder: '请输入省份'
                    },
                    rules: {
                      required: false
                    },
                    // validating when blur
                    trigger: 'blur'
                  },
                  {
                    type: 'input',
                    modelKey: 'city',
                    label: '城市：',
                    props: {
                      placeholder: '请输入城市'
                    },
                    rules: {
                      required: false
                    },
                    // validating when blur
                    trigger: 'blur'
                  },
                  {
                    type: 'input',
                    modelKey: 'telnumber',
                    label: '手机号：',
                    props: {
                      placeholder: '请输入手机号'
                    },
                    rules: {
                      required: true
                    },
                    trigger: 'blur'
                  }
                ]
              }
            ]
          }
        }
      },
      created() {
        this.scrollHeight =  "calc(100% - 150px)"
      },
      mounted() {
        var userinfo = this.$store.getters.getUserInfo
        this.model.nickname = userinfo.nickname
        this.model.sex = userinfo.sex==1?["男"]:["女"]
        this.model.province = userinfo.province
        this.model.city = userinfo.city
        this.model.telnumber = userinfo.telnumber
        this.model.birthDay = userinfo.birthDay
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
        },
        SelectedItem: {
          get: function () { return this.selectedItem },
          set: function (newValue) { this.selectedItem = newValue;}
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
        submitHandler(e) {
          e.preventDefault()
        },
        //弹窗
        showPopup(item) {
          this.SelectedItem = item
          const component = this.$refs.DiscountCoupon
          component.show()
          setTimeout(() => {
            component.hide()
          }, 600000)
        },
        hidePopup() {
          this.$refs.DiscountCoupon.hide()
        },
        routerPush(name, params) {
          this.$router.push({ name: name, params: params })
        },
        //个人信息填写
        showDatePicker() {
          var that=this
          this.$createDatePicker({
            showNow: true,
            title: '选择日期',
            minuteStep: 5,
            delay: 15,
            value: new Date(),
            onSelect: (selectedTime, selectedText, formatedTime) => {
              this.model.dateValue = that.Global.Fun.dateFormat(new Date(formatedTime[0], formatedTime[1] - 1, formatedTime[2]), 'yyyy-MM-dd')  
            },
            onCancel: () => {
            }
          }).show()
        },
        commit() {
          console.log(this.$store.getters.getUserInfo)
        }
      },
      components: {
      }
    }
  </script>

  <style lang="stylus" rel="stylesheet/stylus">
    .container .HomeMeItem {
      display: flex;
      align-items: center;
      background-image: linear-gradient(#edf9d97a, #fff)
    }
    .HomeMePopup .cube-form-item .cube-checkbox, .cube-form-item .cube-radio {
      display: inline-block;
      margin-right: 20px;
    }
    .HomeMePopup .cube-form_standard .cube-form-item {
      background-color: aliceblue;
    }
    .HomeMePopup .cube-form_standard .cube-input input {
      background-color: aliceblue;
    }
  </style>
