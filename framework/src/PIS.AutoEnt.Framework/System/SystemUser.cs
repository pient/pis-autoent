using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Security
{
    public class SystemUser : UserInfo
    {
        #region Consts

        public const string USER_ID = "982ddf97-94b6-471b-aae6-a176018afc34";
        public const string USER_NAME = "SYSTEM";

        #endregion

        #region Properties

        private static SystemUser _System = new SystemUser();

        public static SystemUser System
        {
            get { return _System; }
        }

        #endregion

        #region Constructors

        protected SystemUser()
        {
            this.UserId = USER_ID;
            this.LoginName = this.Name = USER_NAME;
            this.SecurityLevel = 100;
        }

        #endregion
    }
}
