using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.DataContext
{
    public static class ModelExtensions
    {
        #region SysObjWithStructure Extensions

        public static TreeNode<T> ToTreeNode<T>(this SysObjWithStructure<T> node)
            where T : class, ISysStdStructedObject
        {
            var _e = node.Entity;
            var _s = node.Structure;

            var _n = new TreeNode<T>()
            {
                Id = _s.ObjectId,
                ParentId = _s.ParentId,
                Code = _e.Code,
                Name = _e.Name,
                Path = _s.Path,
                PathLevel = _s.PathLevel,
                SortIndex = _s.SortIndex,
                IsLeaf = _s.IsLeaf,

                Tag = _e
            };

            return _n;
        }

        public static TreeNodes<T> ToTree<T>(this IEnumerable<SysObjWithStructure<T>> objs, int? rootLevel = null, bool clearRootParentId = false)
            where T : class, ISysStdStructedObject
        {
            var rootNodes = new TreeNodes<T>();
            var subNodes = new List<TreeNode<T>>();

            var _objs = objs.OrderBy(o => o.Structure.SortIndex);

            int? _rootLevel = null;

            if (objs.Count() > 0)
            {
                _rootLevel = (rootLevel != null ? rootLevel : objs.Min(o => o.Structure.PathLevel ?? 0));
            }

            foreach (var _o in _objs)
            {
                var _n = ToTreeNode(_o);

                if (_o.Structure.PathLevel == _rootLevel)
                {
                    if (clearRootParentId == true)
                    {
                        _n.ParentId = null;
                    }

                    rootNodes.Add(_n);
                }
                else
                {
                    subNodes.Add(_n);
                }
            }

            foreach (var _n in rootNodes)
            {
                _n.OrganizeNodes(subNodes);
            }

            return rootNodes;
        }

        public static TreeNodes<T> ToTreeTable<T>(this IEnumerable<SysObjWithStructure<T>> objs, int? rootLevel = null)
            where T : class, ISysStdStructedObject
        {
            return null;
        }

        #endregion
    }
}
