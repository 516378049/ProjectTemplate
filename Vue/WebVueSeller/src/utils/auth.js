import Cookies from 'js-cookie'

const TokenKey = 'loginToken'
const loginId = 'loginId'
const sellerKey = 'seller'
export function getToken() {
  return Cookies.get(TokenKey)
}

export function setToken(token) {
  return Cookies.set(TokenKey, token)
}
export function getUserId() {
  return Cookies.get(loginId)
}
export function getSeller() {
  let seller = Cookies.get(sellerKey)

  try {
    if (typeof JSON.parse(seller) == "object") {
      seller = JSON.parse(seller)
    }
  } catch (e) {}

  return seller
}
export function setSeller(seller) {
  if (typeof seller === "object") {
    seller = JSON.stringify(seller)
  }
  return Cookies.set(sellerKey,seller)
}
export function setUserId(id) {
  return Cookies.set(loginId, id)
}

export function removeToken() {
  return Cookies.remove(TokenKey)
}
