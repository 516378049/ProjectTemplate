<template>
  <div>
    <!--:on-success="handleUploadSuccess"-->
    <el-upload :action="useOss?ossUploadUrl:minioUploadUrl"
               :data="useOss?dataObj:null"
               list-type="picture"
               :multiple="false"
               :show-file-list="true"
               :file-list="fileList"
               :before-upload="beforeUpload"
               :on-remove="handleRemove"
               :on-preview="handlePreview">
      <el-button id="btn_global_single_upload" size="small" type="primary">点击上传</el-button>
      <div slot="tip" class="el-upload__tip">只能上传jpg/png文件，且不超过10MB</div>
    </el-upload>
    <el-dialog :visible.sync="dialogVisible" style="text-align:center" append-to-body>
      <img style=""  :src="fileList[0].url" alt="">
    </el-dialog>
  </div>
</template>

<script>
  import {policy} from '@/api/oss'
  import * as imageConversion from 'image-conversion';//图片压缩
  import imageCompress from '@/utils/plugins/imageCompress';//图片压缩

  export default {
    name: 'singleUpload',
    props: {
      value: String,
      fileName:String,
      fileUrl:String,
      imgConfig:Object
    },
    computed: {
      imageUrl() {
        return this.fileUrl//this.value;
      },
      imageName() {
        if (this.fileName != null && this.fileName !== '') {
          return this.fileName;
        } else {
          return null;
        }
      },
      showFileList: {
        get: function () {
          //return this.value !== null && this.value !== ''&& this.value!==undefined;
            return this.fileUrl !== null && this.fileUrl !== ''&& this.fileUrl!==undefined;
        },
        set: function (newValue) {
        }
      },
      fileList() {
        return [{
          name: this.imageName,
          url: this.imageUrl
        }]
      }
    },
    data() {
      return {
        dataObj: {
          policy: '',
          signature: '',
          key: '',
          ossaccessKeyId: '',
          dir: '',
          host: '',
          // callback:'',
        },
        imgSize:{width:520,height:520},
        dialogVisible: false,
        useOss:false, //使用oss->true;使用MinIO->false
        ossUploadUrl:'',
        minioUploadUrl:'selleradmin/api.selleradmin.uploadFile',
      };
    },
    created(){

    },
    mounted(){

    },
    methods: {
      emitInput(val,name) {
        this.$emit('input', val,name)
      },
      handleRemove(file, fileList) {
        console.log("handleRemove");
      },
      handlePreview(file) {
        this.dialogVisible = true;
      },
      beforeUpload(file) {
        let that=this
        let _self = this;
        console.log(file);
        //压缩图片
  		  let CompressRes=imageCompress(file,that.imgConfig).then(res=>{
        console.log(res);
        let formData = new FormData();
        //上传至服务器
        formData.append("uploadFile",res.compressBlob,file.name);
          serverApi.uploadFile(formData).then(response => {
          _self.handleUploadSuccess(response, file)
          //设置图片展示的高宽适应当前图片
          _self.imgSize.width=res.width
          _self.imgSize.height=res.height
        })
      })
        return false//在beforeUpload事件中压缩图片，并且使用axios上传

        if(!this.useOss){
          //不使用oss不需要获取策略
          return true;
        }
        //使用oss
        return new Promise((resolve, reject) => {
          policy().then(response => {
            _self.dataObj.policy = response.data.policy;
            _self.dataObj.signature = response.data.signature;
            _self.dataObj.ossaccessKeyId = response.data.accessKeyId;
            _self.dataObj.key = response.data.dir + '/${filename}';
            _self.dataObj.dir = response.data.dir;
            _self.dataObj.host = response.data.host;
            // _self.dataObj.callback = response.data.callback;
            resolve(true)
          }).catch(err => {
            console.log(err)
            reject(false)
          })
        })
      },
      handleUploadSuccess(res, file) {
        this.fileList.pop();
        let url = this.dataObj.host + '/' + this.dataObj.dir + '/' + file.name;
        if(!this.useOss){
          //不使用oss直接获取图片路径
          url = res.Message[0].Path//res.data.url;
        }
        console.log("file.name")
        console.log(file.name)
        this.fileList.push({name: file.name, url: url});
        //this.emitInput(this.fileList[0].url);
        this.$emit('select',this.fileList[0].url,file.name)
      }
    }
  }
</script>

<style type="text/css">
</style>


