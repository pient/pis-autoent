using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Security;

namespace PIS.AutoEnt.DataContext
{
    public partial class SysMetadata : IEntityObject
    {
        #region Constructors

        public SysMetadata()
        {
            Initialize();
        }

        private void Initialize()
        {
            Id = SystemHelper.NewCombId();

            UserInfo user = SysPrincipal.CurrentUser;

            if (user == null)
            {
                user = Security.SystemUser.System;
            }

            this.CreatedTime = SystemHelper.GetCurrentTime();

            this.OwnerType = (int)UserType.System;
            this.OwnerId = user.UserId;
            this.OwnerName = user.Name;
        }

        #endregion
    }
}
