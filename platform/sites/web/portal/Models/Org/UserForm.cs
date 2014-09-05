using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIS.AutoEnt.App;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Web;

namespace PIS.AutoEnt.Portal.Models.Org
{
    public class UserForm: StdFormData
    {
        #region Properties

        public string LoginName { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        #endregion

        #region Methods

        public UserForm()
        {
        }

        #endregion
    }
}