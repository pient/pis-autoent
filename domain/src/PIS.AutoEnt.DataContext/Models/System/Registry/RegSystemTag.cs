using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.DataContext
{
    [Serializable]
    public class RegSystemTag
    {
        #region Properties

        public bool IsAppInitialized
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public RegSystemTag()
        {
            this.IsAppInitialized = false;
        }

        #endregion
    }
}
