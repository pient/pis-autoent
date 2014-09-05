using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.DtoModels
{
    public class UserAccount
    {
        #region Properties

        public string LoginName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        public string Password { get; set; }

        #endregion
    }
}
