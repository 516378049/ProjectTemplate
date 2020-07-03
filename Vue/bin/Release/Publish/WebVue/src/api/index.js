import { get } from './helpers'

const getSeller = get('OrderMealCustomer/api.seller.getseller/')
const getGoods = get('OrderMealCustomer/api.seller.getGoods/')
const getRatings = get('OrderMealCustomer/api.seller.getRatingsSeller/')
const wxAuthorize = get('OrderMeal/Home/wxAuthorize')


export {
  getSeller,
  getGoods,
  getRatings,
  wxAuthorize
}
