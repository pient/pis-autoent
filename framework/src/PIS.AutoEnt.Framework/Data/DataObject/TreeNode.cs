using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.Data
{
    public class TreeNodes<T> : EasyCollection<TreeNode<T>>
    {

    }

    public class TreeNode<T>
    {
        #region Constructors

        public TreeNode()
        {
            Items = new TreeNodes<T>();
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
        public bool? IsLeaf { get; set; }

        public TreeNodes<T> Items { get; set; }

        public T Tag { get; set; }

        #endregion

        #region 公共方法

        public void OrganizeNodes(ICollection<TreeNode<T>> nodes)
        {
            foreach (var _n in nodes)
            {
                if (_n.ParentId == this.Id)
                {
                    this.Items.Add(_n);

                    _n.OrganizeNodes(nodes);
                }
            }
        }

        #endregion
    }

    public class TreeNode : TreeNode<object>
    {
    }
}
