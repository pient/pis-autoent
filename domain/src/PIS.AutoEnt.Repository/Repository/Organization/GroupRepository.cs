using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public class GroupRepository : SysStructedObjectRepository<OrgGroup>, IGroupRepository
    {
        #region Constructors

        public GroupRepository()
        {
        }

        public GroupRepository(SysDbContext ctx)
            : base(ctx)
        {
        }

        #endregion

        #region IGroupRepository Members

        public override void Delete(OrgGroup entity)
        {
            entity.SysAuths.Clear();
            entity.OrgUsers.Clear();
            entity.OrgFunctions.Clear();

            base.Delete(entity);
        }

        #endregion
    }
}
