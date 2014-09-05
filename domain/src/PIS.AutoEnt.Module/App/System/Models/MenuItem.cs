using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.Portal
{
    public class MenuItems : EasyCollection<MenuItem>
    {
    }

    public class MenuItem
    {
        #region Constructors

        public MenuItem()
        {
            Items = new MenuItems();
        }

        #endregion

        #region Properties

        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int? PathLevel { get; set; }
        public int? SortIndex { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public bool? leaf { get; set; }

        public object Tag { get; set; }

        #endregion

        #region Navigator Properties

        public MenuItems Items { get; set; }

        #endregion
    }
}
