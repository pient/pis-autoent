using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Newtonsoft.Json;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.Web;
using PIS.AutoEnt.Web.Mvc;
using PIS.AutoEnt.Portal.GlobalResources;
using PIS.AutoEnt.Portal.Models.Account;

namespace PIS.AutoEnt.Portal.WebSite.Controllers
{
    public class AccountController : AbstractController
    {
        /// <summary>
        /// 登录控制
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string lang, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!string.IsNullOrEmpty(lang))
            {
                this.SetLanguage(lang);

                return RedirectToAction("Login");
            }

            var loginModel = RetrieveLoginModelFromCookie();

            return View(loginModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            try
            {
                var authInfo = new DtoModels.AuthInfo
                {
                    LoginName = model.UserName,
                    Password = model.Password,
                    RememberName = model.RememberName,
                    RememberPwd = model.RememberPwd
                };

                if (ModelState.IsValid)
                {
                    AppSecurity.Login(authInfo);

                    SaveLoginModelToCookie(model);

                    return RedirectToLocal(model, returnUrl);
                }
            }
            catch (AuthException authEx)
            {
                ModelState.AddModelError("", authEx.Reason.ToString());
            }
            catch
            {
                ModelState.AddModelError("", HomeResource.Login_LoginFailed);
            }

            return View(model);
        }

        /// <summary>
        /// 重新登陆
        /// </summary>
        /// <returns></returns>
        [RequireAuthorize]
        public ActionResult SignOut(string action = null)
        {
            AppSecurity.Logout();

            switch (action)
            {
                case "exit":
                    return Json(new { success = true });
                default:
                    return RedirectToAction("Login", "Account");
            }
        }

        #region Private Methods

        private void SaveLoginModelToCookie(LoginModel loginModel)
        {
            try
            {
                string modelString = JsonConvert.SerializeObject(loginModel, Formatting.None);
                string encryptString = CryptoManager.GetEncryptString(modelString);

                HttpCookie userCookie = new HttpCookie(SysConsts.UserKey, encryptString);
                userCookie.Expires = DateTime.Now.AddDays(10);    // Save for 10 days

                HttpContext.Response.Cookies.Set(userCookie);
            }
            catch { }
        }

        private LoginModel RetrieveLoginModelFromCookie()
        {
            LoginModel model = new LoginModel();

            try
            {
                HttpCookie userCookie = HttpContext.Request.Cookies.Get(SysConsts.UserKey);

                if (userCookie != null && !String.IsNullOrEmpty(userCookie.Value))
                {
                    string encryptString = CryptoManager.GetDecryptString(userCookie.Value);
                    model = JsonConvert.DeserializeObject<LoginModel>(encryptString);
                }
            }
            catch { }

            return model;
        }

        private ActionResult RedirectToLocal(LoginModel model, string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}
