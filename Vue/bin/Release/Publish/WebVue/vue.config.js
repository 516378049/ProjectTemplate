const webpack = require('webpack')
const path = require('path')
const appData = require('./data.json')
const seller = appData.seller
const goods = appData.goods
const ratings = appData.ratings

function resolve(dir) {
    return path.join(__dirname, dir)
}

module.exports = {
  css: {
    loaderOptions: {
      stylus: {
        'resolve url': true,
        'import': [
          './src/theme'
        ]
      }
    }
  },
  pluginOptions: {
    'cube-ui': {
      postCompile: true,
      theme: true
    }
  },
  configureWebpack: {
    plugins: [
      new webpack.ProvidePlugin({
        $: "jquery",
        jQuery: "jquery",
        "windows.jQuery": "jquery"
      })
    ]
  },
  //开发环境设置代理，防止跨域问题
  devServer: {
    /* 代理前拦截
    before(app) {
      app.get('/api/seller', function (req, res) {
        res.json({
          errno: 0,
          data: seller
        })
      })
      app.get('/api/goods', function (req, res) {
        res.json({
          errno: 0,
          data: goods
        })
      })
      app.get('/api/ratings', function (req, res) {
        res.json({
          errno: 0,
          data: ratings
        })
      })
    },*/
    proxy: {
      //没用到的前缀代理需要注释，否则会影响到其他前缀的匹配
      //'/api': {//以/api开始请求地址进行转发 
      //  target: 'http://vuecli.test/', //API服务器的地址 modified by changchun 20200619
      //  changeOrigin: true,
      //  pathRewrite: {
      //    '^/api': '' //将地址中的/api去掉 eg:http://localhost:8900/api/items 变为 http://localhost:8900/items
      //  }
      //},
      '/OrderMealCustomer': {
        target: 'http://local.ordermeal.vue/',
        changeOrigin: true,
        pathRewrite: {
          '^/OrderMealCustomer': ''
        }
      },
      '/Authorize': {   //如果有前缀相同的，那么短点的前缀写后面，不然会有限匹配
        target: 'http://vuecli.test/',
        changeOrigin: true,
        pathRewrite: {
          '^/Authorize': ''
        }
      }


      
    }
  }
  ,
  chainWebpack(config) {
    config.resolve.alias
      .set('components', resolve('src/components'))
      .set('common', resolve('src/common'))
      .set('api', resolve('src/api'))

    config.plugin('context')
      .use(webpack.ContextReplacementPlugin, [/moment[/\\]locale$/, /zh-cn/])
  },
  baseUrl: ''
}
