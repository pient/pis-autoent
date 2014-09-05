using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public class RegRepository : SysMetaObjectRepository<SysRegistry>, IStdObjRepository<SysRegistry>, IRegRepository
    {
        #region Constructors

        public RegRepository()
        {
        }

        public RegRepository(SysDbContext ctx)
            : base(ctx)
        {
        }

        #endregion

        #region IRegRepository Members

        public SysRegistry GetSysRegistry()
        {
            var reg = this.FindByCode(SysRegistry.REG_SYS_CODE);

            return reg;
        }

        #endregion

        #region ObjRepository Members

        public override bool Update(SysRegistry entity)
        {
            // entity.XTag = CLRHelper.SerializeToXmlString<RegTag>(entity.RegTag);

            return base.Update(entity);
        }

        public override SysRegistry Create(SysRegistry entity)
        {
            // entity.XTag = CLRHelper.SerializeToXmlString<RegTag>(entity.RegTag);

            return base.Create(entity);
        }

        #endregion
    }
}
