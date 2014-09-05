using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.DtoModels
{
    public class AuthInfo
    {
        #region Properties

        public Guid? UserId { get; set; }

        public string SessionId { get; set; }

        public string LoginName { get; set; }

        public string Password { get; set; }

        public bool RememberName { get; set; }

        public bool RememberPwd { get; set; }

        /// <summary>
        /// For Changing Password
        /// </summary>
        public string NewPassword { get; set; }

        public string AuthType { get; set; }

        #endregion
    }
}
