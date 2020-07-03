using Model.EF;
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
    [EnableCors(origins: "http://www.changchunamy.com/OrderMeal/", headers: "*", methods: "*")]
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

    }
}