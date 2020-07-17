import { get } from './helpers'

const getSeller = get('OrderMealCustomer/api.seller.getseller/')
const getGoods = get('OrderMealCustomer/api.seller.getGoods/')
const getRatings = get('OrderMealCustomer/api.seller.getRatingsSeller/')
const getCartCount = get('OrderMealCustomer/api.seller.getCartCount/')
const setCartCount = get('OrderMealCustomer/api.seller.setCartCount/')

const wxAuthorize = get('Authorize/Home/wxAuthorize')
const JSInit = get('Authorize/Home/JSInit')
const GetUnifiedOrderResult = get('wxPayApi/Home/GetUnifiedOrderResultNew')

export {
  getSeller,
  getGoods,
  getRatings,
  getCartCount,
  setCartCount,
  wxAuthorize,
  JSInit,
  GetUnifiedOrderResult
}
