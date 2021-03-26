<template>
  <el-dialog title="商家信息"
             :visible.sync="showDialog"
             width="50%"
             height="100%"
             @close="handleClose">

    <el-form id="ModalForm" :model="model.seller" :rules="rules" ref="sellerFrom" label-width="150px">
      <el-input v-show="false" name="Id" v-model="model.seller.Id"></el-input>
      <el-form-item label="商家名称：" prop="name">
        <el-input name="name" v-model="model.seller.name" ></el-input>
      </el-form-item>
      <el-form-item label="商户图片：">
        <SingleUpload name="avatar" v-model="model.seller.avatar" @select="onSelect" :imgConfig="imgConfig" :fileUrl="model.seller.avatar" :fileName="fileName"></SingleUpload>
      </el-form-item>
      <el-form-item label="描述：">
        <el-input name="description" v-model="model.seller.description"></el-input>
      </el-form-item>
      <el-form-item label="公告信息：">
        <el-input name="bulletin" type="textarea"
                  placeholder="请输入公告信息"
                  maxlength="1000"
                  show-word-limit
                  autosize
                  v-model="model.seller.bulletin"></el-input>
      </el-form-item>
      <el-form-item label="商家状态：">
        <el-select v-model="model.seller.DelFlag" placeholder="请选择商家状态" change="selChange_DelFlag">
          <template v-for="item in selOptions_DelFlag">
            <el-option :label="item.lable" :key="item.value" :value="item.value"></el-option>
          </template>
        </el-select>
        <el-input v-show="false" name="DelFlag" v-model="model.seller.DelFlag"></el-input>
      </el-form-item>
    </el-form>
    <span slot="footer" class="dialog-footer">
      <el-button @click="showDialog = false">取 消</el-button>
      <el-button id="btnSellerEdit" type="primary" @click="handleSave('sellerFrom')">保存</el-button>
    </span>

  </el-dialog>
</template>

<script>

  import SingleUpload from '@/components/Upload/singleUpload'
import { get } from 'https';
  const defaultSeller = {
  }
  export default {
    // 接受父组件传递的值
    props: {
      addOrUpdateVisible: {
        type: Boolean,
        default: false
      },
      propSeller: {
        type: Object,
        default: Object.assign({}, defaultSeller)
      },
      propOption: {
        type: String,
        default: "view"
      },
    },
    data() {
      return {
        // 控制弹出框显示隐藏
        showDialog: false,
       
        model: {
          seller: Object.assign({}, this.propSeller)
        },
        selOptions_DelFlag: [{
          lable: '正常',
          value: 0
        },
        {
          lable: '关闭',
          value: 1
        }],
        rules: {
          name: [
            { required: true, message: '请输入商家名称', trigger: 'blur' }
          ]
        }
      }
    },
    mounted: function () {
    },
    computed: {
      imgConfig() {
        return { maxSize: 3 * 1024, maxWidth: 750, maxHeight: 750 }
      },
      fileName() {
        console.log('this.model.seller.fileName')
        console.log(this.model.seller.fileName)
        if (this.model.seller.fileName) {
          return this.model.seller.fileName
        } else {
          if (this.model.seller.avatar != null && this.model.seller.avatar !== '') {
            return this.model.seller.avatar.substr(this.model.seller.avatar.lastIndexOf("/") + 1);
          } else {
            return null;
          }
        }
     
      }
    },
    created() {
     
    },
    methods: {
      onSelect(url,name) {
        this.model.seller.fileName  = name
        this.model.seller.avatar = url
      },
      // 弹出框关闭后触发
      handleClose() {
        // 子组件调用父组件方法，并传递参数
        this.$emit('changeShow', 'false')
      },
      handleSelChange(value) {
        console.log(value)
      },
      handleSave(formName) {
        let that=this
        this.validate(formName).then(res => {
          var model = $("#ModalForm").serializeObject();
          model.avatar =that.model.seller.avatar
          serverApi.saveSeller(model).then(response => {
            if (response.RetCode === "0") {
              this.$message.success("保存成功")
              this.showDialog = false;//关闭弹窗
              this.$emit('getList')
            }
          })
        })
      },
      //表单验证通用方法
      validate(formName) {
        let that=this
        return new Promise(function(resolve) {
          that.$refs[formName].validate((valid) => {
            if (valid) {
              resolve()
            }
            else {
              that.$message({
                message: "表单验证不通过，请检查表单红色提示",
                type: 'warning',
                duration: 3000,
                showClose: false
              })
              return false
            }
          })
        })

      }
    },
    watch: {
      // 监听 addOrUpdateVisible 改变
      addOrUpdateVisible() {
        this.showDialog = this.addOrUpdateVisible
      },
      propSeller(newVal) {
        this.model.seller = Object.assign({}, newVal)
        //this.$set(this.model,"seller", Object.assign({}, newVal))
        //this.$forceUpdate()
      },
      'model.seller.DelFlag'(newVal) {
        //console.log('model.seller.DelFlag')
        //console.log(newVal)
      }
    },
    components: { SingleUpload }
  }
</script>
<style type="text/css">
  /*防止文本域字体限制提示遮住文本*/
  .el-textarea .el-input__count {
    background-color: rgba(0,0,0,0.0); /* 适应chrome等浏览器， IE6和部分IE7内核的浏览器(如QQ浏览器)会读懂，但解析为透明 */
    filter: Alpha(opacity=50); /* 只支持IE6、7、8、9 */
    zoom: 1; /* 激活IE6、7的haslayout属性，让它读懂Alpha */
    color: #1ec80b;
  }
</style>
