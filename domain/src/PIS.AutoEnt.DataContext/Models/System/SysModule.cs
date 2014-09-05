using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.DataContext
{
    public partial class SysModule : ISysStdStructedObject
    {
        #region Enums & Classes

        public class ModuleStatus
        {
            public const string Enabled = "Enabled";
            public const string Disabled = "Disabled";
        }

        #endregion

        #region Constructors

        public SysModule()
        {
            this.Id = SystemHelper.NewCombId();
        }

        #endregion
    }

    public class SysModuleWithStructure : SysObjWithStructure<SysModule>
    {
        #region Constructors

        public SysModuleWithStructure(SysModule entity, SysDataStructure structure)
            : base(entity, structure)
        {
        }

        #endregion
    }

    public class SysModulesWithStructure : EasyCollection<SysObjWithStructure<SysModule>>, ICollection<SysObjWithStructure<SysModule>>
    {
    }
}
