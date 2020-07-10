import storage from 'good-storage'

const SELLER_KEY = '__seller__'

// storage.session.set  // --temp storage , will clear after close the broswer

// storage.set  // --long term storage , will long term storage in the broswer

let saveLocal = function (key, value) {
  storage.session.set(key, value)
}
let loadLocal = function (key) {
  return storage.session.get(key, null)
}

export { saveLocal }
export { loadLocal }

export function saveToLocal(id, key, val) {
  const seller = storage.session.get(SELLER_KEY, {})
  if (!seller[id]) {
    seller[id] = {}
  }
  seller[id][key] = val
  storage.session.set(SELLER_KEY, seller)
}

export function loadFromLocal(id, key, def) {
  const seller = storage.session.get(SELLER_KEY, {})
  if (!seller[id]) {
    return def
  }
  return seller[id][key] || def
}
