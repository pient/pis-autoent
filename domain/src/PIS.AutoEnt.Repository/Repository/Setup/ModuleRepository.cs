using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework.Extensions;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public class ModuleRepository : SysStructedObjectRepository<SysModule>, IModuleRepository
    {
        #region Constructors

        public ModuleRepository()
        {
            AuthRepository = new AuthRepository();
        }

        public ModuleRepository(SysDbContext ctx)
            : base(ctx)
        {
            AuthRepository = new AuthRepository(ctx);
        }

        #endregion

        #region IModuleRepository Members

        public IAuthRepository AuthRepository
        {
            get;
            private set;
        }

        public override bool Update(SysModule entity)
        {
            base.Update(entity);

            return this.UpdateAuthByModule(entity);
        }

        public override SysModule Create(SysModule entity)
        {
            var _mdl = base.Create(entity);

            return _mdl;
        }

        public override void Delete(SysModule entity)
        {
            var _auth = this.GetRelatedAuth(entity.Id);
            
            base.Delete(entity);

            if (_auth != null)
            {
                AuthRepository.Delete(_auth);
            }
        }

        public override void Dispose()
        {
            this.AuthRepository.DbContext.SaveChanges();

            base.Dispose();
        }

        #endregion

        #region Support Methods

        internal bool UpdateAuthByModule(SysModule entity)
        {
            var _auth = this.GetRelatedAuth(entity.Id);

            entity.MapToAuth(_auth);

            return AuthRepository.Update(_auth);
        }

        #endregion
    }
}
