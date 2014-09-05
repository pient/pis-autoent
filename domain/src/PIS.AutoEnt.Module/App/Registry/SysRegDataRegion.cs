using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.XData.DataStore;

namespace PIS.AutoEnt.App
{
    public class SysRegDataRegion : RegDataRegion
    {
        #region Enums

        public enum SystemStatusEnum
        {
            UnInitialized,
            Initialized,
            Unknown
        }

        #endregion

        #region Properties

        public RegDataNode SystemNode
        {
            get
            {
                var node = this.GetRootNode(AppRegistry.Sys_Code);

                if (node == null)
                {
                    node = new RegDataNode()
                    {
                        Code = AppRegistry.Sys_Code,
                        Name = AppRegistry.Sys_Name,
                        Editable = AppRegistry.EditStatus.Readonly
                    };

                    this.SetRootNode(node);
                }

                return node;
            }
        }

        public SystemStatusEnum SystemStatus
        {
            get
            {
                var item = SystemNode.GetItemByCode(AppRegistry.Sys_Status_Code);

                if (item == null)
                {
                    return SystemStatusEnum.Unknown;
                }
                else
                {
                    return (SystemStatusEnum)item.Data.ToString().ToInteger(0);
                }
            }

            internal set
            {
                var item = SystemNode.GetItemByCode(AppRegistry.Sys_Status_Code);

                if (item == null)
                {
                    item = this.NewItem(SystemNode, AppRegistry.Sys_Status_Code, AppRegistry.Sys_Status_Name);
                }

                item.Data = value;
            }
        }

        #endregion

        #region Constructors

        public SysRegDataRegion(SysRegistry reg)
            : base(reg)
        {
        }

        #endregion

        #region Public Methods

        #endregion
    }
}
