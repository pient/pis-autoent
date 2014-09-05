using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Web;

namespace PIS.AutoEnt.Portal.Models.Home
{
    public class ModuleItems : EasyCollection<ModuleItem>
    {
    }

    public class ModuleItem : PageData
    {
        #region Constructors

        public ModuleItem()
        {
            Items = new ModuleItems();
        }

        #endregion

        #region Properties

        public string Icon { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public Guid? ParentId { get; set; }
        public int? PathLevel { get; set; }
        public int? SortIndex { get; set; }
        public bool? leaf { get; set; }

        public object Tag { get; set; }

        #endregion

        #region Navigator Properties

        public ModuleItems Items { get; set; }

        #endregion

        #region Static Methods

        public static ModuleItem FromMdlTree(TreeNode<SysModule> mdlTree)
        {
            var mdlItem = new ModuleItem()
            {
                Code = mdlTree.Code,
                Title = mdlTree.Name,
                Path = mdlTree.Tag.MdlPath,
                Icon = mdlTree.Tag.Icon,
                Status = mdlTree.Tag.Status,

                ParentId = mdlTree.ParentId,
                PathLevel = mdlTree.PathLevel,
                SortIndex = mdlTree.SortIndex,
                leaf = mdlTree.IsLeaf,
            };

            foreach (var n in mdlTree.Items)
            {
                var _it = FromMdlTree(n);

                mdlItem.Items.Add(_it);
            }

            return mdlItem;
        }

        #endregion
    }
}