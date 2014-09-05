using System;
using System.Collections.Generic;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.DataContext
{
    public partial class SysObject : ISysStdObject
    {
        #region Consts

        #endregion

        #region Constructors

        public SysObject()
        {
            this.Id = SystemHelper.NewCombId();
        }

        #endregion
    }

    [Serializable]
    public class SysTag
    {
        #region Constructors

        public SysTag()
        {
        }

        #endregion
    }
}
