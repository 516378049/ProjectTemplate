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
    //[EnableCors(origins: "https://www.changchunamy.com,http://localhost:8080", headers: "*", methods: "*")]
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
                return CreateApiExpResult(ex.Message);
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
                    X.Status = 0;
                    X.CreateUserId = UserId;
                    X.UpdateUserId = UserId;
                });
                StudioTra.OrderDetailsInfo.InsertRange(l_OrderDetailsInfo);
                StudioTra.CommitTransaction();
                //clear the cart
                string _DeskNumber = orderinfo.DeskNumber.ToString();
                while (_DeskNumber.Length < 3)
                {
                    _DeskNumber = "0" + _DeskNumber;
                }
                RedisHelper.getRedisServer.KeyDelete("seller." + +orderinfo.SellerId + "." + _DeskNumber);
            }
            catch (Exception e)
            {
                StudioTra.Rollback();
                Log.ILog4_Error.Error("插入订单出错：", e);
                return CreateApiExpResult("插入订单出错：" + e.Message);
            }
            finally
            {

            }
            return CreateApiResult("success");
        }

        /// <summary>
        /// 订单取消，status 默认为7取消
        /// </summary>
        /// <param name="orderNum">订单编号</param>
        /// <param name="status">订单状态：1、待支付、2、商家待接单；3、商家已接单；4、订单完成；5、待评价；6、已评价；7、取消订单；8、申请退款；9、商家同意退款；10、退款成功</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult OrderChangeStatus(string orderNum = "", int status = 7)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderNum))
                {
                    return CreateApiExpResult( "未知的订单编号：");
                }

                OrderInfo order = Studio.OrderInfo.Get(X => X.OrderNum == orderNum && X.DelFlag == 0).FirstOrDefault();
                if (order == null)
                {
                    return CreateApiExpResult("未知的订单编号：");
                }
                order.Status = status;
                Studio.OrderInfo.Update(order);
            }
            catch (Exception e)
            {
                Log.ILog4_Error.Error("订单取消出错：", e);
                return CreateApiExpResult("订单取消出错：" + e.Message);
            }
            finally
            {

            }
            return CreateApiResult("success");
        }

        /// <summary>
        /// 改变订单行状态
        /// </summary>
        /// <param name="orderNum">订单编号</param>
        /// <param name="status">订单状态：1、待支付、2、商家待接单；3、商家已接单；4、订单完成；5、待评价；6、已评价；7、取消订单；8、申请退款；9、商家同意退款；10、退款成功</param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult OrderDetailsChangeStatus(string data = "", int status = 0, int OrderId = 0, int Id = 0)
        {

            try
            {
                ChangeStatusParams Params = JsonHelper.ToObject<ChangeStatusParams>(data);

                List<OrderDetailsInfo> order = new List<OrderDetailsInfo>();
                OrderInfo orderInfo = new OrderInfo();
                orderInfo = Studio.OrderInfo.GetById(OrderId);
                if (Params.Id > 0)
                {
                    order = Studio.OrderDetailsInfo.Get(X => X.Id == Params.Id && X.OrderId == Params.OrderId && X.DelFlag == 0).ToList();
                }
                else
                {
                    order = Studio.OrderDetailsInfo.Get(X => X.OrderId == Params.OrderId && X.DelFlag == 0).ToList();
                }

                if (order == null)
                {
                    return CreateApiExpResult("未知的订单编号：");
                }

                StudioTra.BeginTransaction();
                for (int i = 0; i < order.Count; i++)
                {
                    if (orderInfo.Status == 3)//已接单状态更新订单明细情况
                    {
                        if ((order[i].Status ==null || order[i].Status==0) && status == 1) //待制作->制作完成
                        {
                            order[i].Status = Params.status;
                            StudioTra.OrderDetailsInfo.Update(order[i]);
                        }

                        if (order[i].Status == 1 && status == 0) //制作完成->待制作
                        {
                            order[i].Status = Params.status;
                            StudioTra.OrderDetailsInfo.Update(order[i]);
                        }

                        if (order[i].Status == 1 && status == 2) //待配送->配送完成
                        {
                            order[i].Status = Params.status;
                            StudioTra.OrderDetailsInfo.Update(order[i]);
                        }

                        if (order[i].Status == 2 && status == 1) //配送完成->待配送
                        {
                            order[i].Status = Params.status;
                            StudioTra.OrderDetailsInfo.Update(order[i]);
                        }
                    }
                }

                //更新主订单状态（如果所有订单明细配送完成）
                if (status == 2)//配送完成请求
                {
                    int count = 0;

                    var orderList = StudioTra.OrderDetailsInfo.Get(X => X.OrderId == Params.OrderId && X.DelFlag == 0).ToList();
                    orderList.ForEach(X =>
                    {
                        if (X.Status == 1 || X.Status == 0 || X.Status==null) //是否存在状态为待配送和待制作的订单明细，如果不存在则须更新主订单状态
                        {
                            count++;
                        }
                    });
                    if (count == 0) //如果不存在则须更新主订单状态
                    {
                        var OI = StudioTra.OrderInfo.Get(X => X.Id == OrderId).FirstOrDefault();
                        OI.Status = 4; //整笔订单制作完成
                        StudioTra.OrderInfo.Update(OI);
                    }

                }
                StudioTra.CommitTransaction();
            }
            catch (Exception e)
            {
                StudioTra.Rollback();
                Log.ILog4_Error.Error("订单明细状态修改出错：", e);
                return CreateApiExpResult("订单明细状态修改出错：" + e.Message);
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
        public ApiResult getOrderInfoList(QueryOrderInfoParams Params)
        {
            //int userId = 0, int sellerId = 0, string startTime = "", string slipAction = "", int count = 0,int status = -1, string orderId = ""
            int userId = Params.userId;
            int sellerId = Params.sellerId;
            DateTime startTime = Params.startTime;
            string sortCreateTime= Params.sortCreateTime;
            bool timeEquals= Params.timeEquals;
            string slipAction = Params.slipAction;
            int count = Params.count;
            int status = Params.status;
            string OrderNum = Params.OrderNum;
            int DetailStatus =-1;

            

            if (!string.IsNullOrEmpty(Params.DetailStatus))
            {
                if (!int.TryParse(Params.DetailStatus, out DetailStatus))
                {
                    return CreateApiExpResult("订单明细状态值必须为整形数字");
                }
            }
            try
            {
                //string Params = GetRequestStreamData();
                //userId = int.Parse(JsonHelper.ConvertJsonResult(Params, "userId"));
                //sellerId = int.Parse(JsonHelper.ConvertJsonResult(Params, "sellerId"));
                //startTime = JsonHelper.ConvertJsonResult(Params, "startTime");
                //slipAction = JsonHelper.ConvertJsonResult(Params, "slipAction");
                //count = int.Parse(JsonHelper.ConvertJsonResult(Params, "count"));
                //status = int.Parse(JsonHelper.ConvertJsonResult(Params, "status"));
                //orderId = (JsonHelper.ConvertJsonResult(Params, "orderId"));

                List<OrderInfo> orderinfoList = new List<OrderInfo>();
                Expression<Func<OrderInfo, bool>> filter = X => X.SellerId == sellerId && X.DelFlag == 0;

                if (!string.IsNullOrEmpty(OrderNum))
                {
                    Expression<Func<OrderInfo, bool>> filter1 = X => X.OrderNum == OrderNum;
                    filter = PredicateBuilderExtension.And(filter, filter1);
                }

                if (userId > 0)
                {
                    Expression<Func<OrderInfo, bool>> filter1 = X => X.CreateUserId == userId;
                    filter = PredicateBuilderExtension.And(filter, filter1);
                }

                if (slipAction == "down")
                {
                    if (timeEquals)
                    {
                        startTime = startTime.AddMilliseconds(-1);  //查询闭区间需要减去一毫秒
                    }
                    else
                    {
                        startTime = startTime.AddMilliseconds(1);  //
                    }
                    DateTime _startTime = startTime;//DateTime.Parse(startTime);
                    Expression<Func<OrderInfo, bool>> filter1 = X => X.CreateTime  >= _startTime;
                    filter = PredicateBuilderExtension.And(filter, filter1);
                }
                else if (slipAction == "up")
                {
                    if (timeEquals)
                    {
                        startTime = startTime.AddMilliseconds(1);  //查询闭区间需要加上一毫秒
                    }
                    else
                    {
                        startTime = startTime.AddMilliseconds(-1);  //
                    }

                    DateTime _startTime = startTime;// DateTime.Parse(startTime);
                    Expression<Func<OrderInfo, bool>> filter1 = X => X.CreateTime <= _startTime;
                    filter = PredicateBuilderExtension.And(filter, filter1);
                }
                else
                {
                    DateTime _startTime = DateTime.MinValue;
                    Expression<Func<OrderInfo, bool>> filter1 = X => X.CreateTime > _startTime;
                    filter = PredicateBuilderExtension.And(filter, filter1);
                }

                if (status>0)
                {
                    Expression<Func<OrderInfo, bool>> filter1 = X => X.Status == status;
                    filter = PredicateBuilderExtension.And(filter, filter1);
                }

               

                Expression<Func<OrderInfo, bool>> InitFilter = null;


                Func<IQueryable<OrderInfo>, IOrderedQueryable<OrderInfo>> sort = null;
                if (!string.IsNullOrEmpty(sortCreateTime) && sortCreateTime.ToUpper()=="ASC")
                {
                    sort = X => X.OrderBy(Y => Y.CreateTime);
                }
                else
                {
                    sort = X => X.OrderByDescending(Y => Y.CreateTime);
                }
               

                var _orderinfoList= Studio.OrderInfo.Get(InitFilter, sort, "", 1, count);
                _orderinfoList = Studio.OrderInfo.Get(filter, sort, "", 1, count);
                orderinfoList = _orderinfoList.ToList();

                List<OrderInfoEx> orderinfoExList = new List<OrderInfoEx>(); 

                orderinfoList.ForEach(X =>
                {
                    OrderInfoEx oiEx = new OrderInfoEx(X);
                    if (DetailStatus > -1)
                    {
                        var detailList= Studio.OrderDetailsInfo.Get(Y => Y.OrderId == X.Id && (Y.Status == DetailStatus || Y.Status == null) && Y.DelFlag == 0).ToList();
                        if (detailList!=null && detailList.Count>0)
                        {
                            oiEx.OrderDetailsInfo = detailList;
                            orderinfoExList.Add(oiEx);
                        }
                    }
                    else
                    {
                        oiEx.OrderDetailsInfo = Studio.OrderDetailsInfo.Get(Y => Y.OrderId == X.Id && Y.DelFlag == 0).ToList();
                        orderinfoExList.Add(oiEx);
                    }
                });

                return CreateApiResult(orderinfoExList);
            }
            catch (Exception ex)
            {
                return CreateApiExpResult( ex.Message);
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

                _studio = _studio.Select(X => new OrderInfo() { OrderNum = X.OrderNum, Status = X.Status });
                List<OrderInfo> orderinfoList = _studio.ToList();
                return CreateApiResult(orderinfoList);
            }
            catch (Exception ex)
            {
                return ErrorHandle(ex);
            }
        }
        #endregion

        #region 上传图片
        [HttpPost]
        public ApiResult uploadFile()
        {
            try
            {
                HttpContextBase context = (HttpContextBase)(Request.Properties["MS_HttpContext"]);
                HttpFileCollectionBase files = context.Request.Files;

                List<FileList> fileLists = new List<FileList>();
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    byte[] bytes = new byte[file.ContentLength];
                    file.InputStream.Read(bytes, 0, bytes.Length);
                    string msg = "";
                    string rePath = HttpUploadFile.LocalUploadFile(bytes, "/Content/image", file.FileName, ref msg);
                    fileLists.Add(new FileList { Name = file.FileName , Path= rePath });
                }

                return CreateApiResult(fileLists);
            }
            catch (Exception ex)
            {

                return ErrorHandle(ex);
            }


        }
        #endregion
    }
    class FileList{
        public string Name { set; get; }
        public string Path { set; get; }
    }
}