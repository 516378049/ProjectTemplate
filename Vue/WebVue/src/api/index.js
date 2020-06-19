import { get } from './helpers'

const getSeller = get('api/seller')
const getGoods = get('api/goods')
const getRatings = get('api/ratings')

const wxAuthorize = get('OrderMeal/wxAuthorize')

export {
  getSeller,
  getGoods,
  getRatings,
  wxAuthorize
}
