import { get } from './helpers'

const getSeller = get('api/api/seller')
const getGoods = get('api/api/goods')
const getRatings = get('api/api/ratings')

const wxAuthorize = get('api/Home/wxAuthorize')

export {
  getSeller,
  getGoods,
  getRatings,
  wxAuthorize
}
