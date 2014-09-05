using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.Security;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public static class RepoInitializer
    {
        #region Initialize Data

        [PISTransaction]
        public static void Initialize()
        {
            RepoMapperConfig.Register();
        }

        #endregion
    }
}
