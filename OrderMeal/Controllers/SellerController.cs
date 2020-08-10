using Framework;
using Model.EF;
using Model.EF.EF_ExAttr;
using Model.orderMeal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI.Core.WebAPI;
using WeChat.Common;

using static WeChat.Common.PredicateBuilderExtension;

namespace OrderMeal.Controllers
{
    [EnableCors(origins: "https://www.changchunamy.com,http://localhost:8080", headers: "*", methods: "*")]
    public class SellerController : BaseApiController
    {
        #region default fundation
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        #endregion

        #region 获取商家信息
        /// <summary>
        /// get seller infomation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult getSeller(int id)
        {
            try
            {
                sellers seller = Studio.Sellers.Get(X => X.Id == id && X.DelFlag == 0).FirstOrDefault();
                List<supports> supports = Studio.Supports.Get(X => X.sellerId == id && X.DelFlag == 0).ToList();
                sellersEx sellerEx = new sellersEx(seller);
                sellerEx.supports = supports;
                return CreateApiResult(sellerEx);
            }
            catch (Exception ex)
            {
                return ErrorHandle(ex);
            }
        }
        /// <summary>
        /// get seller menus
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult getGoods(int id)
        {
            try
            {
                List<goods> goods = Studio.Goods.Get(X => X.sellerId == id && X.DelFlag == 0).ToList();
                List<goodsEx> l_goodsEx = new List<goodsEx>();
                foreach (goods good in goods)
                {
                    List<foods> foods = Studio.Foods.Get(X => X.goodId == good.Id && X.DelFlag == 0).ToList();
                    List<foodsEx> l_foodsEx = new List<foodsEx>();
                    foreach (foods food in foods)
                    {
                        List<ratings> ratings = Studio.Ratings.Get(X => X.foodId == food.Id && X.DelFlag == 0).ToList();

                        foodsEx foodsEx = new foodsEx(food);
                        foodsEx.ratings = ratings;
                        l_foodsEx.Add(foodsEx);
                    }

                    goodsEx goodsEx = new goodsEx(good);
                    goodsEx.foods = l_foodsEx;
                    l_goodsEx.Add(goodsEx);
                }
                return CreateApiResult(l_goodsEx);
            }
            catch (Exception ex)
            {
                return ErrorHandle(ex);
            }
           
        }
        /// <summary>
        /// get seller rattings 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult getRatingsSeller(int id)
        {
            try
            {
                List<RatingsSellers> ratings = Studio.RatingsSeller.Get(X => X.sellerId == id && X.DelFlag == 0).ToList();
                return CreateApiResult(ratings);
            }
            catch (Exception ex)
            {
                return ErrorHandle(ex);
            }
           
        }
        #endregion

        #region 设置获取缓存购物车
        /// <summary>
        /// get cart from redis cache by sellerid and desknumber
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deskNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult getCartCount(int id, string deskNumber)
        {
            try
            {
                List<CartOrder> cart = RedisHelper.getRedisServer.StringGet<List<CartOrder>>("seller." + id + "." + deskNumber);
                if (cart == null)
                {
                    return CreateApiResult("");
                }
                return CreateApiResult(cart);
            }
            catch (Exception ex)
            {
                return CreateApiResult("-1", ex.Message, "");
            }
          
        }
        /// <summary>
        /// set cart from redis cache
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deskNumber"></param>
        /// <param name="menuId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult setCartCount(int id, string deskNumber, int menuId, int count)
        {

            try
            {
                CartOrder cart = new CartOrder(id, deskNumber, menuId, count);
                List<CartOrder> cartList = RedisHelper.getRedisServer.StringGet<List<CartOrder>>("seller." + id + "." + deskNumber);

                if (cartList == null)
                {
                    cartList = new List<CartOrder>();

                }
                CartOrder _cart = cartList.FirstOrDefault(X => X.menuId == menuId);
                if (_cart != null)
                {
                    _cart.count += count;
                }
                else
                {
                    cartList.Add(cart);
                }
                RedisHelper.getRedisServer.StringSet("seller." + id + "." + deskNumber, cartList);
                return CreateApiResult("success");
            }
            catch (Exception ex)
            {
                return ErrorHandle(ex);
            }

  
        }
        #endregion

        #region 订单操作
        /// <summary>
        /// create order info
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResult CreateOrderInfo()
        {
            try
            {
                string Params = GetRequestStreamData();
                string _orderinfo = JsonHelper.ConvertJsonResult(Params, "orderinfo");
                string _l_OrderDetailsInfo = JsonHelper.ConvertJsonResult(Params, "l_OrderDetailsInfo");
                //保存订单
                OrderInfo orderinfo = JsonHelper.ToObject<OrderInfo>(_orderinfo);
                orderinfo.Status = 1;
                orderinfo.CreateUserId = UserId;
                orderinfo.UpdateUserId = UserId;
                List<OrderDetailsInfo> l_OrderDetailsInfo = JsonHelper.ToObject<List<OrderDetailsInfo>>(_l_OrderDetailsInfo);

                StudioTra.BeginTransaction();

                orderinfo = StudioTra.OrderInfo.Insert(orderinfo);
                l_OrderDetailsInfo.ForEach(X => {
                    X.OrderId = orderinfo.Id;
                    X.CreateUserId = UserId;
                    X.UpdateUserId = UserId;
                });
                StudioTra.OrderDetailsInfo.InsertRange(l_OrderDetailsInfo);
                StudioTra.CommitTransaction();
                //clear the cart
                string _DeskNumber = orderinfo.DeskNumber.ToString();
                while(_DeskNumber.Length<3)
                {
                    _DeskNumber = "0" + _DeskNumber;
                }
                RedisHelper.getRedisServer.KeyDelete("seller." +  +orderinfo.SellerId +"." +_DeskNumber);
            }
            catch (Exception e)
            {
                StudioTra.Rollback();
                Log.ILog4_Error.Error("插入订单出错：", e);
                return CreateApiResult("-1", "插入订单出错：" + e.Message);
            }
            finally
            {

            }
            return CreateApiResult("success");
        }

        /// <summary>
        /// Get orderInfolist
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sellerId"></param>
        /// <param name="startTime"></param>
        /// <param name="slipAction">1、向下拉(down)取查询当前时间以前订单，向上(up)拉取订单，查询当前时间以后订单</param>
        /// <param name="count">记录数</param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult getOrderInfoList(int userId = 0, int sellerId = 0, string startTime = "", string slipAction = "", int count = 0)
        {

            try
            {
                string Params = GetRequestStreamData();
                userId = int.Parse(JsonHelper.ConvertJsonResult(Params, "userId"));
                sellerId = int.Parse(JsonHelper.ConvertJsonResult(Params, "sellerId"));
                startTime = JsonHelper.ConvertJsonResult(Params, "startTime");
                slipAction = JsonHelper.ConvertJsonResult(Params, "slipAction");
                count = int.Parse(JsonHelper.ConvertJsonResult(Params, "count"));

                List<OrderInfo> orderinfoList = new List<OrderInfo>();
                Expression<Func<OrderInfo, bool>> filter = X => X.CreateUserId == userId && X.SellerId == sellerId && X.DelFlag == 0;
                if (slipAction == "down")
                {
                    DateTime _startTime = DateTime.Parse(startTime).AddSeconds(1);//加上一秒钟，因为数据库中存在毫秒，传入时没有毫秒
                    Expression<Func<OrderInfo, bool>> filter1 = X => X.CreateTime > _startTime;
                    filter = PredicateBuilderExtension.And(filter, filter1);
                }
                if (slipAction == "up")
                {
                    DateTime _startTime = DateTime.Parse(startTime);
                    Expression<Func<OrderInfo, bool>> filter1 = X => X.CreateTime < _startTime;
                    filter = PredicateBuilderExtension.And(filter, filter1);
                }
                else
                {
                    DateTime _startTime = DateTime.MinValue;
                    Expression<Func<OrderInfo, bool>> filter1 = X => X.CreateTime > _startTime;
                    filter = PredicateBuilderExtension.And(filter, filter1);
                }
                var _orderinfoList = Studio.OrderInfo.Get(filter, X => X.OrderByDescending(Y => Y.CreateTime), "", 1, count);
                orderinfoList = _orderinfoList.ToList();

                List<OrderInfoEx> orderinfoExList = new List<OrderInfoEx>();

                orderinfoList.ForEach(X =>
                {
                    OrderInfoEx oiEx = new OrderInfoEx(X);
                    oiEx.OrderDetailsInfo = Studio.OrderDetailsInfo.Get(Y => Y.OrderId == X.Id && Y.DelFlag == 0).ToList();
                    orderinfoExList.Add(oiEx);
                });

                return CreateApiResult(orderinfoExList);
            }
            catch (Exception ex)
            {
                return CreateApiResult("-1", ex.Message, "");
            }

         
        }
        /// <summary>
        /// get OrderInfo List Status 
        /// </summary>
        /// <param name="_orderinfoList"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult getOrderInfoListStatus()
        {
            try
            {
                string Params = GetRequestStreamData();
                string OrderInfoList = JsonHelper.ConvertJsonResult(Params, "OrderInfoList");
                List<OrderInfo> _orderinfoList = JsonHelper.ToObject<List<OrderInfo>>(OrderInfoList);
                Expression<Func<OrderInfo, bool>> filter = X => 1 == 0;
                _orderinfoList.ForEach(X => {
                    Expression<Func<OrderInfo, bool>> filter1 = Y => Y.OrderNum == X.OrderNum;
                    filter = PredicateBuilderExtension.Or(filter, filter1);
                });
                Expression<Func<OrderInfo, bool>> filter2 = (X => X.DelFlag == 0);
                filter = PredicateBuilderExtension.And(filter, filter2);
                var _studio = Studio.OrderInfo.Get(filter);

                _studio = _studio.Select(X=>new OrderInfo() { OrderNum=X.OrderNum,Status=X.Status });
                List<OrderInfo> orderinfoList = _studio.ToList();
                return CreateApiResult(orderinfoList);
            }
            catch (Exception ex)
            {
                return ErrorHandle(ex);
            }
        }
        #endregion
    }
}