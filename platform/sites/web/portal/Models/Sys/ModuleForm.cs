using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Web;

namespace PIS.AutoEnt.Portal.Models.Sys
{
    public class ModuleForm : StdStructedFormData
    {
        public string Type { get; set; }
        public string Icon { get; set; }
        public string MdlPath { get; set; }
        public string Description { get; set; }
        public bool? IsEnabled { get; set; }

        #region Methods

        public SysModule ToSysModule()
        {
            return null;
        }

        #endregion
    }
}