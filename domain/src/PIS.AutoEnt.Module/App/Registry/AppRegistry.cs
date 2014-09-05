using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.AutoEnt.App;
using PIS.AutoEnt.XData;

namespace PIS.AutoEnt
{
    public static class AppRegistry
    {
        #region Consts

        public const string Sys_Code = "Sys";
        public const string Sys_Name = "系统";

        public const string Sys_Status_Code = "SYS_STATUS";
        public const string Sys_Status_Name = "系统状态";

        public const string Sys_Ptl_Layout_Code = "SysPortal.Layout";
        public const string Sys_Ptl_Portlet_Code = "SysPortal.Portlet";

        #endregion

        #region Classes and Enums

        public class EditStatus
        {
            #region Consts

            public const string Default = "Default";
            public const string Readonly = "Readonly";
            public const string OnlyAddSub = "OnlyAddSub";
            public const string NotEditable = "NotEditable";

            #endregion
        }

        public class RegDataType
        {
            #region Consts

            public const string Default = "Default";
            public const string Enum = "Enum";
            public const string Config = "Config";

            #endregion
        }
        
        #endregion

        #region System

        public static SysRegDataRegion System
        {
            get
            {
                return AppPortal.RegDataStore.SysRegion;
            }
        }

        public static RegDataNode SystemNode
        {
            get
            {
                return AppPortal.RegDataStore.SysRegion.SystemNode;
            }
        }

        public static RegDataNode GetSysNodeByCode(string code)
        {
            var _node = AppRegistry.System.GetNodeByCode(code) as RegDataNode;

            return _node;
        }

        public static object GetSysObjById(Guid id)
        {
            return AppRegistry.System.GetById(id);
        }

        public static RegDataItem GetSysItemById(Guid id)
        {
            return AppRegistry.System.GetById(id) as RegDataItem;
        }

        public static RegDataNode GetSysNodeById(Guid id)
        {
            var _node = AppRegistry.System.GetById(id) as RegDataNode;

            return _node;
        }

        public static void PersistSystem()
        {
            AppPortal.RegDataStore.PersistSysRegion();
        }

        #endregion
    }
}
