using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository.Interfaces;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.Repository
{
    public class DataStructureRepository : SysDataRepository<SysDataStructure>, IDataStructureRepository
    {
        #region Constructors

        public DataStructureRepository()
        {
        }

        public DataStructureRepository(SysDbContext ctx)
            : base(ctx)
        {
        }

        #endregion

        #region IDataStructureRepository Members

        public int GetSiblingsCount(SysDataStructure entity)
        {
            IQueryable<SysDataStructure> qry = this.GetSiblingQry(entity);

            var count = qry.Count();

            return count;
        }

        public IList<SysDataStructure> GetPrevSiblings(SysDataStructure entity)
        {
            IQueryable<SysDataStructure> qry = this.GetSiblingQry(entity)
                .Where(e => e.SortIndex < entity.SortIndex);

            var list = qry.ToList();

            return list;
        }

        public IList<SysDataStructure> GetNextSiblings(SysDataStructure entity)
        {
            IQueryable<SysDataStructure> qry = this.GetSiblingQry(entity)
                .Where(e => e.SortIndex > entity.SortIndex);

            var list = qry.ToList();

            return list;
        }

        public SysDataStructure GetPrevSibling(SysDataStructure entity)
        {
            IQueryable<SysDataStructure> qry = this.GetSiblingQry(entity)
                .Where(e => e.SortIndex < entity.SortIndex);

            var obj = qry.LastOrDefault();

            return obj;
        }

        public SysDataStructure GetNextSibling(SysDataStructure entity)
        {
            IQueryable<SysDataStructure> qry = this.GetSiblingQry(entity)
                .Where(e => e.SortIndex > entity.SortIndex);

            var obj = qry.FirstOrDefault();

            return obj;
        }

        public int GetChildrenCount(SysDataStructure parent)
        {
            if (parent.IsLeaf == true)
            {
                return 0;
            }

            IQueryable<SysDataStructure> qry = this.GetChildrenQry(parent);

            var count = qry.Count();

            return count;
        }

        public SysDataStructure GetFirstChild(SysDataStructure parent)
        {
            if (parent.IsLeaf == true)
            {
                return null;
            }

            IQueryable<SysDataStructure> qry = this.GetChildrenQry(parent);

            var obj = qry.ToList().FirstOrDefault();

            return obj;
        }

        public SysDataStructure GetLastChild(SysDataStructure parent)
        {
            if (parent.IsLeaf == true)
            {
                return null;
            }

            IQueryable<SysDataStructure> qry = this.GetChildrenQry(parent);

            var obj = qry.ToList().LastOrDefault();

            return obj;
        }

        public SysDataStructure GetChildByPosition(SysDataStructure parent, int position)
        {
            if (parent.IsLeaf == true)
            {
                return null;
            }

            IQueryable<SysDataStructure> qry =
                this.GetChildrenQry(parent).Skip(position).Take(1);

            var obj = qry.ToList().FirstOrDefault();

            return obj;
        }

        public IList<SysDataStructure> GetChildren(SysDataStructure parent)
        {
            if (parent.IsLeaf == true)
            {
                return null;
            }

            var qry = this.GetChildrenQry(parent);

            var list = qry.ToList();

            return list;
        }

        public IQueryable<SysDataStructure> GetChildrenQry(SysDataStructure parent)
        {
            IQueryable<SysDataStructure> qry = this.DbContext.SysDataStructures.Where(e => e.StructureCode == parent.StructureCode
                && e.ParentId == parent.ObjectId).OrderBy(e => e.SortIndex);

            return qry;
        }

        public IQueryable<SysDataStructure> GetSiblingQry(SysDataStructure entity)
        {
            IQueryable<SysDataStructure> qry = this.DbContext.SysDataStructures.Where(e => e.StructureCode == entity.StructureCode
                && e.ParentId == entity.ParentId && e.Id != entity.Id).OrderBy(e => e.SortIndex);

            return qry;
        }

        public IQueryable<SysDataStructure> GetAllRelatedQry<T>(T entity) where T : ISysStructedObject
        {
            string idStr = entity.Id.ToString();

            var qry = this.DbContext.SysDataStructures.Where
               (s => s.Path.Contains(idStr) || s.ObjectId == entity.Id);

            return qry;
        }

        public void ClearStructure<T>(T entity) where T : ISysStructedObject
        {
            string idStr = entity.Id.ToString();

            var list = this.DbContext.SysDataStructures.Where(s => s.Path.Contains(idStr) || s.ObjectId == entity.Id).ToList();

            list.All((e) =>
            {
                EntityContext.Register(e, EntityObjectState.Deleted);

                return true;
            });
        }

        #endregion
    }
}
