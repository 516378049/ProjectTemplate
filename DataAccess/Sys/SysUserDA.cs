using FluentData;
using Models;
using ThinkDev.FrameWork.Result;

namespace JR.NewTenancy.DataAccess.Sys
{
    public class SysUserDA : DBContextBase
    {

        #region 本地用户初始化
        /// <summary>
        /// 本地用户初始化
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ResultInfo<int> InitLocalUserInfo(SysUserInfo userInfo, string ipaddress)
        {
            using (IDbContext context = Context())
            {
                var cmd = context.StoredProcedure("[SP_UserInfoUpdate]")
                    .Parameter("UserID", userInfo.UserID)
                    .Parameter("UserName", userInfo.UserName)
                    .Parameter("NickName", userInfo.NickName)
                    .Parameter("Sex", userInfo.Sex)
                    .Parameter("Phone", "")
                    .Parameter("Mobile", userInfo.Mobile)
                    .Parameter("Email", userInfo.Email)
                    .Parameter("QQ", userInfo.QQ)
                    .Parameter("CompanyID", userInfo.CompanyID)
                    .Parameter("CompanyName", userInfo.CompanyName)
                    .Parameter("IPAddress", ipaddress);

                cmd.Data.Command.Parameter("ReturnValue", 0, DataTypes.Int32, FluentData.ParameterDirection.ReturnValue, 11);
                cmd.Execute();
                int ret = cmd.Data.Command.ParameterValue<int>("ReturnValue");
                ResultInfo<int> result = SetRullErr<int>(ret.ToString(), ret, 0);
                return result;
            }
        }
        #endregion
    }
}