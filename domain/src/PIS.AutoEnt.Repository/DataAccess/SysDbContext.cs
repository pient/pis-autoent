using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Data;
using System.Data.SqlClient;

namespace PIS.AutoEnt.Repository
{
    public class SysDbContext : PISFrameworkEntities, IEntityContext
    {
        #region Constructors

        public SysDbContext()
            : base(DataContextHelper.BuildEntitySqlConnection(AppDataAccessor.ConnectionString))
        {
        }

        #endregion

        #region IEntityContext Members

        public void Register(IEntityObject entity, EntityObjectState state)
        {
            this.Entry(entity).State = state.ToEntityState();
        }

        #endregion
    }
}
