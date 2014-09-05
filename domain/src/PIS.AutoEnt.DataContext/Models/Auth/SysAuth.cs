using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.DataContext
{
    public partial class SysAuth : ISysStdStructedObject
    {
        #region Types

        public enum TypeEnum
        {
            Auto = 0,
            Module = 1
        }

        #endregion

        [NotMapped]
        public virtual TypeEnum AuthType
        {
            get { return CLRHelper.GetEnum<TypeEnum>(this.Type, TypeEnum.Auto, true); }
            set { this.Type = value.ToString(); }
        }
    }
}
