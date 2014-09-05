using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public class RoleRepository : OrgRepository<OrgRole>, IRoleRepository
    {
        #region Constructors

        public RoleRepository()
        {
        }

        public RoleRepository(SysDbContext ctx)
            : base(ctx)
        {
        }

        #endregion

        #region IRoleRepository Members

        #endregion
    }
}
