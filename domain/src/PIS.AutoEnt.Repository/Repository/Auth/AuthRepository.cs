using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityFramework.Extensions;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public class AuthRepository : SysStructedObjectRepository<SysAuth>, IAuthRepository
    {
        #region Constructors

        public AuthRepository()
        {
        }

        public AuthRepository(SysDbContext ctx)
            : base(ctx)
        {
        }

        #endregion

        #region IAuthRepository Members



        #endregion
    }
}
