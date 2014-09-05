using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace PIS.AutoEnt.Security
{
    public interface ISysPrincipal : IPrincipal
    {
        UserInfo GetUser();
    }
}
