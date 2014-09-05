using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Security;

namespace PIS.AutoEnt
{
    /// <summary>
    /// 登录Session状态
    /// </summary>
    public enum UserSessionState
    {
        Valid,  // 有效
        None,   // 不存在
        Timeout,    // 超时
        Released,   // 已释放
        Faulted,    // 已出错
        Unknown     // 未知
    }

    public interface IPortalProvider
    {
        /// <summary>
        /// 获取NServiceBus
        /// </summary>
        /// <returns></returns>
        // IServiceBus RetrieveServiceBus();

        /// <summary>
        /// 获取当前用户令牌
        /// </summary>
        /// <returns></returns>
        PortalUser GetCurrentUser();

        /// <summary>
        /// 由登录sessionID胡偶去用户信息
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        PortalUser GetUser(string sessionID);

        /// <summary>
        /// 获取当前登录状态
        /// </summary>
        /// <returns></returns>
        UserSessionState GetSessionState(string sessionID);

        /// <summary>
        /// 刷新登录Session
        /// </summary>
        /// <param name="sessionID"></param>
        void RefreshSession(string sessionID);

        /// <summary>
        /// 释放登录Session
        /// </summary>
        /// <param name="sessionID"></param>
        void ReleaseSession(string sessionID);

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="authMessage">登录信息</param>
        /// <returns>登录sessionId</returns>
        void Login(IAuthPackage authPackage);

        /// <summary>
        /// 注销当前登录
        /// </summary>
        void Logout(string sessionID);
    }
}
