const getters = {
  sidebar: state => state.app.sidebar,
  device: state => state.app.device,
  token: state => state.user.token,
  seller: state => state.user.seller,
  avatar: state => state.user.avatar,
  name: state => state.user.name,
  nick_name: state => state.user.nick_name,
  id: state => state.user.id,
  roles: state => state.user.roles,
  addRouters: state => state.permission.addRouters,
  routers: state => state.permission.routers
}
export default getters
