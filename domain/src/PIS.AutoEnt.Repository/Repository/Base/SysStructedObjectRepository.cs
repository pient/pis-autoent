using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public abstract class SysStructedObjectRepository<T> : SysMetaObjectRepository<T>, IStructedRepository<T>, IMetaObjectRepository<T>
        where T : class, ISysStructedObject
    {
        #region Constructors

        protected SysStructedObjectRepository()
            : this(new DataStructureRepository())
        {

        }

        protected SysStructedObjectRepository(SysDbContext ctx)
            : this(new DataStructureRepository(ctx))
        {

        }

        private SysStructedObjectRepository(IDataStructureRepository structRepo)
            : base(structRepo.DbContext)
        {
            StructRepository = structRepo;
        }

        #endregion

        #region DataRepository Members

        public override void Delete(T entity)
        {
            // 获取并删除所有关联节点(包括自己在内)
            var sQry = this.StructRepository.GetAllRelatedQry(entity);

            var list = this.QueryObjsWithStructure<T>(sQry);

            list.All((e) =>
            {
                var metadata = this.GetMetadata(e.Entity);

                if (metadata != null)
                {
                    EntityContext.Register(metadata, EntityObjectState.Deleted);
                }

                EntityContext.Register(e.Entity, EntityObjectState.Deleted);

                return true;
            });

            var _p_s_obj = this.GetParent(entity);

            if (_p_s_obj != null)
            {
                int _siblingcount = StructRepository.GetChildrenCount(_p_s_obj.Structure);

                if (_siblingcount <= 1)
                {
                    _p_s_obj.Structure.IsLeaf = true;

                    EntityContext.Register(_p_s_obj.Structure, EntityObjectState.Modified);
                }
            }

            // 同时删除Structed Object内所有内容
            StructRepository.ClearStructure(entity);
        }

        #endregion

        #region IStructedRepository Members

        public IDataStructureRepository StructRepository
        {
            get;
            private set;
        }

        public override void Dispose()
        {
            this.StructRepository.DbContext.SaveChanges();

            base.Dispose();
        }

        #endregion
    }
}
