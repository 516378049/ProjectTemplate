import storage from 'good-storage'

const SELLER_KEY = '__seller__'

let saveLocal = function (key, value) {
  storage.set(key, value);
}
let loadLocal = function (key) {
  return storage.get(key, null);
}

export { saveLocal }
export { loadLocal }

export function saveToLocal(id, key, val) {
  const seller = storage.get(SELLER_KEY, {})
  if (!seller[id]) {
    seller[id] = {}
  }
  seller[id][key] = val
  storage.set(SELLER_KEY, seller)
}

export function loadFromLocal(id, key, def) {
  const seller = storage.get(SELLER_KEY, {})
  if (!seller[id]) {
    return def
  }
  return seller[id][key] || def
}
