import { get,post } from './helpers'

const getSeller = get('OrderMealCustomer/api.seller.getseller/')
const getGoods = get('OrderMealCustomer/api.seller.getGoods/')
const getRatings = get('OrderMealCustomer/api.seller.getRatingsSeller/')
const getCartCount = get('OrderMealCustomer/api.seller.getCartCount/')
const setCartCount = get('OrderMealCustomer/api.seller.setCartCount/')
const CreateOrderInfo = post('OrderMealCustomer/api.seller.CreateOrderInfo')
const getOrderInfoList = post('OrderMealCustomer/api.seller.getOrderInfoList')
const getOrderInfoListStatus = post('OrderMealCustomer/api.seller.getOrderInfoListStatus')
const JSInit = get('OrderMealCustomer/api.wxApi.JSInit')

const wxAuthorize = get('Authorize/Home/wxAuthorize')
const GetUnifiedOrderResult = get('wxPayApiNew/Home/GetUnifiedOrderResultNew')



export {
  getSeller,
  getGoods,
  getRatings,
  getCartCount,
  setCartCount,
  CreateOrderInfo,
  getOrderInfoList,
  getOrderInfoListStatus,
  wxAuthorize,
  JSInit,
  GetUnifiedOrderResult
}
