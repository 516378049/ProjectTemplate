import { get,post } from './helpers'

const getSeller = get('OrderMealCustomer/api.seller.getseller/')//获取商家信息
const getGoods = get('OrderMealCustomer/api.seller.getGoods/')//获取菜单信息
const getRatings = get('OrderMealCustomer/api.seller.getRatingsSeller/')//获取商家点评信息
const getCartCount = get('OrderMealCustomer/api.seller.getCartCount/')//获取购物车
const setCartCount = get('OrderMealCustomer/api.seller.setCartCount/')//设置购物车
const CreateOrderInfo = post('OrderMealCustomer/api.seller.CreateOrderInfo')//创建订单
const OrderChangeStatus = get('OrderMealCustomer/api.seller.OrderChangeStatus')//修改订单状态
const getOrderInfoList = post('OrderMealCustomer/api.seller.getOrderInfoList')//获取订单列表
const getOrderInfoListStatus = post('OrderMealCustomer/api.seller.getOrderInfoListStatus')//获取订单状态
const JSInit = get('OrderMealCustomer/api.wxApi.JSInit')//获取JSAPI接口参数

const wxAuthorize = get('Authorize/Home/wxAuthorize')//登录获取验证信息
const GetUnifiedOrderResult = get('wxPayApiNew/Home/GetUnifiedOrderResultNew')//获取支付参数

const uploadFile = post('OrderMealCustomer/api.seller.uploadFile')//获取订单列表

export {
  getSeller,
  getGoods,
  getRatings,
  getCartCount,
  setCartCount,
  CreateOrderInfo,
  OrderChangeStatus,
  getOrderInfoList,
  getOrderInfoListStatus,
  wxAuthorize,
  JSInit,
  GetUnifiedOrderResult,
  uploadFile
}
