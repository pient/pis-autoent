using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.DataContext
{
    public partial class SysRegistry : ISysStdObject
    {        
        #region Consts

        public const string REG_SYS_CODE = "System";
        public const string REG_SYS_NAME = "System";

        #endregion

        #region Properties

        #endregion

        #region Constructors

        public SysRegistry()
        {
            this.Id = SystemHelper.NewCombId();
        }

        #endregion
    }
}
