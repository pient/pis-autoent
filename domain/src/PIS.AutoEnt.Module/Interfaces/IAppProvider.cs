using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Security;

namespace PIS.AutoEnt.App.Interfaces
{
    public interface IAppProvider
    {
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="authMessage">登录信息</param>
        /// <returns>登录sessionId</returns>
        void Logon(AppUser appUser, DtoModels.AuthInfo authInfo);

        /// <summary>
        /// 注销当前登录
        /// </summary>
        void Logoff();

        void SetLanguage(string lang);

        string GetLanguage();
    }
}
