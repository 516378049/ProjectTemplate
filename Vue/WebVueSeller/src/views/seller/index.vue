<template>
  <div class="app-container">
    <!--search-->
    <el-card class="filter-container" shadow="never">
      <div>
        <i class="el-icon-search"></i>
        <span>筛选搜索</span>
      </div>
      <div style="margin-top: 15px">
        <el-form :inline="true" :model="listQuery" size="small" label-width="140px">
          <el-form-item label="商家名称：">
            <el-input v-model="listQuery.name" class="input-width" placeholder="商家名称"></el-input>
          </el-form-item>
          <el-form-item label="商家状态：">
            <el-switch @change=""
                       :active-value="0"
                       :inactive-value="1"
                       v-model="listQuery.delflag">
            </el-switch>
          </el-form-item>
          <el-form-item>
            <el-button style="float:right"
                       type="primary"
                       @click="handleSearchList()"
                       size="small">
              查询搜索
            </el-button>
            <el-button style="float:right;margin-right: 15px"
                       @click="handleResetSearch"
                       size="small">
              重置
            </el-button>
          </el-form-item>
        </el-form>
      </div>
      <!--ADD-->
      <el-button style="float:right"
                 type="success"
                 @click="handleShow()"
                 size="small">商家入驻</el-button>
    </el-card>
    <!--List-->
    <div class="table-container">
      <el-table ref="sellerTable"
                :data="list"
                style="width: 100%"
                v-loading="listLoading"
                border>
        <el-table-column label="商家名称" align="center">
          <template slot-scope="scope">
            {{scope.row.name}}
          </template>
        </el-table-column>
        <el-table-column label="商户图片" align="center">
          <template slot-scope="scope">
            <img style="height: 80px" :src="scope.row.avatar">
          </template>
        </el-table-column>
        <el-table-column label="描述" align="center">
          <template slot-scope="scope">
            {{scope.row.description}}
          </template>
        </el-table-column>
        <el-table-column label="公告信息" align="center">
          <template slot-scope="scope">
            <el-popover placement="top-start"
                        title="公告信息"
                        width="200"
                        trigger="hover"
                        :content="scope.row.bulletin">
              <span slot="reference">
                {{getBulletin(scope.row.bulletin)}}
              </span>
            </el-popover>
          </template>
        </el-table-column>
        <el-table-column label="商家状态" align="center">
          <template slot-scope="scope">
            <el-popover placement="top-start"
                        :title="scope.row.DelFlag==0?'正常':'已关闭'"
                        width="200"
                        trigger="hover"
                        content="">
              <el-switch slot="reference"
                         @change=""
                         :active-value="0"
                         :inactive-value="1"
                         v-model="scope.row.DelFlag">
              </el-switch>
            </el-popover>
          </template>
        </el-table-column>
        <el-table-column label="最近登录" align="center">
          <template slot-scope="scope">
            {{scope.row.UpdateTime}}
          </template>
        </el-table-column>
        <el-table-column label="创建时间" align="center">
          <template slot-scope="scope">
            {{scope.row.CreateTime}}
          </template>
        </el-table-column>
        <el-table-column label="修改时间" align="center">
          <template slot-scope="scope">
            {{scope.row.UpdateTime}}
          </template>
        </el-table-column>
        <el-table-column label="修改时间" align="center">
          <template slot-scope="scope">
            <el-button @click="handleShowDetail(scope.row)" size="small">详情</el-button>
            <el-button type="primary" @click="handleEdit(scope.row)" size="small">编辑</el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>
    <sellerDetail :addOrUpdateVisible="addOrUpdateVisible" :propSeller="propSeller" propOption="propOption" @getList="getList" @changeShow="handleShowAddOrUpdate" ref="addOrUpdateRef"></sellerDetail>
  </div>
</template>
<script>
  
  import sellerDetail from './components/sellerDetail'
  //初始化默认查询参数
  const defaultListQuery = {
    name: '',
    delflag: 0,
    pageNum: 1,
    pageSize: 10
  }
  export default {
    name: 'sellerList',
    data() {
      return {
        listQuery: Object.assign({}, defaultListQuery),
        list: null,
        total: null,
        listLoading: false,
        addOrUpdateVisible: false,
        propSeller: {},
        propOption:"view"
      }
    },
    created() {
      this.getList();
    },
    methods: {
      //搜索方法
      getList() {
        
        this.listLoading = true;
        this.list=[]
        serverApi.getSeller(this.listQuery).then(response => {
          if (response.RetCode === "0") {
            this.list = response.Message;
            this.total = 1;
            this.totalPage = 1;
            this.pageSize = 10;
          }
        }).finally(res => { this.listLoading = false;})
      },
      //重置输入
      handleResetSearch() {
        this.listQuery = Object.assign({}, defaultListQuery)
      },
      //搜索事件
      handleSearchList() {
        this.listQuery.pageNum = 1;
        this.getList()
      },
      //编辑
      handleEdit(row) {
        $("#btnSellerEdit").show()
        $("#btn_global_single_upload").show()
        this.addOrUpdateVisible = true
        this.propSeller = Object.assign({}, row)
      },
      //详情
      handleShowDetail(row) {
        //隐藏保存编辑按钮和上传按钮
        $("#btnSellerEdit").hide()
        $("#btn_global_single_upload").hide()
        this.addOrUpdateVisible = true
        this.propSeller = Object.assign({}, row)
      },
      //展示弹窗
      handleShow() {
        this.addOrUpdateVisible = true
        this.propSeller = null
      },
      //子组件关闭弹窗事件
      handleShowAddOrUpdate(data) {
        if (data === 'false') {
          this.addOrUpdateVisible = false
        } else {
          this.addOrUpdateVisible = true
        }
      },
    },
    computed: {
      getBulletin() {
        return function (str) {
          if (str && str.length > 10) {
            return str.substr(0, 10) + '...'
          } else {
            return str + '...'
          }
        }
      }
    },
    components: {
      sellerDetail
    }
  }
</script>
<style rel="stylesheet/scss" lang="scss" scoped>
</style>


