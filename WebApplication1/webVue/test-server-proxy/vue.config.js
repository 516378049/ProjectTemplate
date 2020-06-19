// 将任何未知请求 (没有匹配到静态文件的请求) 代理到http://localhost:4000
module.exports = {
    devServer: {
        proxy: {
            '/api': {
                target: 'http://vuecli.test/',
                changeOrigin: true
                //pathRewrite: {
                //    '^/api': ''
                //}
            }
        }
    }
}

//module.export ={
//    //devServer: {
//    //    proxyTable: {
//    //        '/api': {
//    //            target: 'http://localhost:3000'
//    //        },
//    //        '/goods/*': {
//    //            target: 'http://localhost:3000'
//    //        },
//    //        '/users/**': {
//    //            target: 'http://localhost:3000'
//    //        }
//    //    }
//    //    //proxy: {
//    //    //    '/api': {
//    //    //        target: 'http://vuecli.test/',
//    //    //        changeOrigin: true,
//    //    //        pathRewrite: {
//    //    //            '^/api': ''
//    //    //        }
//    //    //    }
//    //    //}
//    //}
//}