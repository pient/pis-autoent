using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PIS.AutoEnt.Portal.GlobalResources;
using PIS.AutoEnt.Web;
using PIS.AutoEnt.Web.Mvc;

namespace PIS.AutoEnt.Portal.Models.Account
{
    [Serializable]
    public class LoginModel : AbstractViewModel
    {
        #region Properties

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberName { get; set; }

        public bool RememberPwd { get; set; }

        #endregion

        #region Constructors

        public LoginModel()
        {
            RememberName = true;
            RememberPwd = false;
        }

        #endregion
    }
}