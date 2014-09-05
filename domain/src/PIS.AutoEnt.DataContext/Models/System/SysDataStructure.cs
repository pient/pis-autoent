using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.DataContext
{
    public partial class SysDataStructure : ISysObject
    {
        #region Consts

        #endregion

        #region Constructors

        internal SysDataStructure()
        {
            this.Id = SystemHelper.NewCombId();
            this.IsLeaf = true;
        }

        public SysDataStructure(Guid objectId, string structureCode)
            : this()
        {
            this.ObjectId = objectId;
            this.StructureCode = structureCode;
        }

        #endregion
    }
}
