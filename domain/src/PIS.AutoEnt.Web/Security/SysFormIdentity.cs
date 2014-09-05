using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using PIS.AutoEnt.Security;

namespace PIS.AutoEnt.Web.Security
{
    public class SysFormsIdentity : FormsIdentity, ISysIdentity
    {
        #region 私有函数

        private UserInfo user = null;

        #endregion

        #region Constructors

        public SysFormsIdentity(FormsAuthenticationTicket ticket)
            : base(ticket)
        {
        }

        #endregion

        #region ISysIdentity Members

        public UserInfo User
        {
            get
            {
                if (user == null)
                {
                    user = CLRHelper.DeserializeFromBase64<AppUser>(this.Ticket.UserData);
                }

                return user;
            }
        }

        #endregion
    }
}
