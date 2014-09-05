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
    public abstract class SysMetaObjectRepository<T> : SysDataRepository<T>, IMetaObjectRepository<T>
        where T : class, ISysMetaObject
    {
        #region Constructors

        protected IMetaRepository MetaRepository
        {
            get;
            set;
        }

        protected SysMetaObjectRepository()
        {
            MetaRepository = new MetaRepository();
        }

        protected SysMetaObjectRepository(SysDbContext ctx)
            : base(ctx)
        {
            MetaRepository = new MetaRepository(ctx);
        }

        #endregion

        #region DataRepository Members

        public override T Create(T entity)
        {
            entity.Id = (entity.Id == Guid.Empty ? SystemHelper.NewCombId() : entity.Id);

            SysMetadata metadata = new SysMetadata()
            {
                ObjectCode = ModelHelper.GetObjectCode<T>(),
                ObjectId = entity.Id
            };

            EntityContext.Register(metadata, EntityObjectState.Added);

            entity.MetadataId = metadata.Id;
            EntityContext.Register(entity, EntityObjectState.Added);

            return entity;
        }

        public override bool Update(T entity)
        {
            SysMetadata metadata = this.GetMetadata(entity);

            if (metadata != null)
            {
                metadata.LastModifiedTime = SystemHelper.GetCurrentTime();
                EntityContext.Register(metadata, EntityObjectState.Modified);
            }

            EntityContext.Register(entity, EntityObjectState.Modified);

            return true;
        }

        public override void Delete(T entity)
        {
            var metadata = this.GetMetadata(entity);

            if (metadata != null)
            {
                EntityContext.Register(metadata, EntityObjectState.Deleted);
            }

            EntityContext.Register(entity, EntityObjectState.Deleted);
        }

        #endregion
    }
}
