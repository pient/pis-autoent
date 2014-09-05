using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt.DataContext
{
    public partial class PISFrameworkEntities
    {
        public PISFrameworkEntities(DbConnection connection)
            : base(connection, true)
        {
        }

        public PISFrameworkEntities(string connectionString)
            : base(connectionString)
        {
        }
    }
}
