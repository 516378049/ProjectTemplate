using Framework;
using Model.EF;
using Model.orderMeal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI.Core.WebAPI;

namespace OrderMeal.Controllers
{
    [EnableCors(origins: "https://www.changchunamy.com/OrderMeal/", headers: "*", methods: "*")]
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
            sellers seller = Studio.Sellers.Get(X => X.Id == id && X.DelFlag == 0).FirstOrDefault();
            List<supports> supports = Studio.Supports.Get(X => X.sellerId == id && X.DelFlag == 0).ToList();
            seller.supports = supports;

            return CreateApiResult(seller);
        }
        /// <summary>
        /// get seller menus
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult getGoods(int id)
        {
            List<goods> goods = Studio.Goods.Get(X => X.sellerId == id && X.DelFlag == 0).ToList();
            foreach (goods good in goods)
            {
                List<foods> foods = Studio.Foods.Get(X => X.goodId == good.Id && X.DelFlag == 0).ToList();
                good.foods = foods;
                foreach (foods food in foods)
                {
                    List<ratings> ratings = Studio.Ratings.Get(X => X.foodId == food.Id && X.DelFlag == 0).ToList();
                    food.ratings = ratings;
                }
            }
            return CreateApiResult(goods);
        }
        /// <summary>
        /// get seller rattings 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult getRatingsSeller(int id)
        {
            List<RatingsSellers> ratings = Studio.RatingsSeller.Get(X => X.sellerId == id && X.DelFlag == 0).ToList();
            return CreateApiResult(ratings);
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
        public ApiResult getCartCount(int id, string deskNumber) {
            List<CartOrder> cart = RedisHelper.getRedisServer.StringGet<List<CartOrder>>("seller." + id + "." + deskNumber);
            if(cart==null)
            {
                return CreateApiResult("");
            }
            return CreateApiResult(cart);
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
        public ApiResult setCartCount(int id, string deskNumber,int menuId,int count)
        {
            CartOrder cart = new CartOrder( id, deskNumber, menuId, count);
            List<CartOrder> cartList = RedisHelper.getRedisServer.StringGet<List<CartOrder>>("seller." + id + "." + deskNumber);
            
            if (cartList==null)
            {
                cartList = new List<CartOrder>();
                
            }
            CartOrder _cart=cartList.FirstOrDefault(X=>X.menuId==menuId);
            if(_cart!=null)
            {
                _cart.count += count;
            }
            else
            {
                cartList.Add(cart);
            }
            RedisHelper.getRedisServer.StringSet("seller."+ id+"."+ deskNumber, cartList);
            return CreateApiResult("success");
        }
        #endregion

        #region 订单操作
        /// <summary>
        /// create order info
        /// </summary>
        /// <returns></returns>
        public ApiResult CreateOrderInfo()
        {
            return CreateApiResult("success");
        }
        #endregion
    }
}