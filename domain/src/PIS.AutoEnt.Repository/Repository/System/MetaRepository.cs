using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public class MetaRepository : SysDataRepository<SysMetadata>, IMetaRepository
    {
        #region Constructors

        public MetaRepository()
        {
        }

        public MetaRepository(SysDbContext ctx)
            : base(ctx)
        {
        }

        #endregion
    }
}
