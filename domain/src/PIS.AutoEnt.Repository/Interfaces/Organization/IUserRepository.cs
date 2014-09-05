using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.Repository.Interfaces
{
    public interface IUserRepository : IStdObjRepository<OrgUser>
    {
        #region 密码操作

        bool VerifyPassword(OrgUser user, string password);

        void ChangePassword(OrgUser user, string password);

        OrgUser FindByLoginName(string loginName);

        #endregion
    }
}
