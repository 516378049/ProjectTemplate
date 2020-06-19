using WebAPI.Core.DataAccess;
using WebAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ThinkDev.FrameWork.Result;

namespace WebAPI.Core.Business
{
    public class ApiAutomaticConfigRule : Base
    {
        private ApiAutomaticConfigDA da = new ApiAutomaticConfigDA();

        /// <summary>
        /// 获取API配置
        /// </summary>
        /// <param name="action"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public ApiAutomaticConfig ConfigQuery(string action, string module)
        {
            return this.da.ConfigQuery(action, module);
        }

        //public DataSet DGetDataSet(string connStr, string code, List<DynamicParam> paramList, out int output)
        //{
        //    return this.da.DGetDataSet(connStr, code, paramList, out output);
        //}

        //public DataTable DGetTable(string connStr, string code, List<DynamicParam> paramList, out int output)
        //{
        //    return this.da.DGetTable(connStr, code, paramList, out output);
        //}

        public dynamic DGet(string connStr, string code, List<DynamicParam> paramList, string returnType, out int output)
        {
            dynamic info;
            switch (returnType)
            {
                case "Object":
                case "List":
                    info = this.da.DGet(connStr, code, paramList, returnType, out output);
                    break;
                case "DataTable":
                    info = this.da.DGetTable(connStr, code, paramList, out output);
                    break;
                case "DataSet":
                    info = this.da.DGetDataSet(connStr, code, paramList, out output);
                    break;
                default:
                    info = null;
                    output = 0;
                    break;

            }
            return info;
        }

        public ResultInfo<string> DPost(string connStr, string code, List<DynamicParam> paramList, List<DynamicResult> resultList)
        {
            ResultInfo<string> result = this.da.DPost(connStr, code, paramList);
            if (result.HasError)
            {
                if (resultList != null && resultList.Count > 0)
                {
                    DynamicResult dr = resultList.Find(f => f.RetCode == result.HRESULT);
                    if (dr != null)
                    {
                        return new ResultInfo<string>(true, dr.RetMsg, dr.RetCode, result.ObjectValue);
                    }
                }
                return new ResultInfo<string>(true, "未知错误", result.HRESULT, result.ObjectValue);
            }
            return result;
        }
    }
}
