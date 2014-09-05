using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.Web;

namespace PIS.AutoEnt.Web.Portal
{
    public class UserData
    {
        #region Properties

        public string Sid { get; set; }
        public string Usr { get; set; }
        public string Pwd { get; set; }
        public bool LogUsr { get; set; }
        public bool LogPwd { get; set; }

        #endregion

        #region Constructors

        public UserData()
        {
        }

        public UserData(AppUser appUser, DtoModels.AuthInfo authInfo)
        {
            this.Sid = appUser.SessionId;
            this.Usr = authInfo.RememberName ? authInfo.LoginName : "";
            this.Pwd = authInfo.RememberPwd ? authInfo.Password : "";
            this.LogUsr = authInfo.RememberName;
            this.LogPwd = authInfo.RememberPwd;
        }

        #endregion

        #region Public Methods
        
        public string ToJsonString()
        {
            string jsonString = JsonConvert.SerializeObject(this, Formatting.None);

            return jsonString;
        }

        #endregion

        #region Static Members

        public static UserData FromJsonString(string jsonString)
        {
            UserData userData = JsonConvert.DeserializeObject<UserData>(jsonString);
            return userData;
        }

        public static UserData GetCurrentUserData()
        {
            UserData userData = null;

            var identity = HttpContext.Current.User.Identity as FormsIdentity;

            if (identity != null)
            {
                string userDataString = identity.Ticket.UserData;

                if (!String.IsNullOrEmpty(userDataString))
                {
                    userData = UserData.FromJsonString(userDataString);
                }
            }

            return userData;
        }

        #endregion
    }
}
