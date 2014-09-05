using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIS.AutoEnt.App;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Web;

namespace PIS.AutoEnt.Portal.Models.Sys
{
    public class RegisterForm: StdStructedFormData
    {
        #region Properties

        public string Editable { get; set; }
        public string EditPage { get; set; }
        public string RegDataType { get; set; }
        public string DataType { get; set; }
        public string DisplayData { get; set; }
        public string Data { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        #endregion

        #region Methods

        public RegisterForm()
        {
        }

        #endregion
    }
}