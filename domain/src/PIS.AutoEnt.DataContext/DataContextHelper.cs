using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PIS.AutoEnt.DataContext
{
    public static class DataContextHelper
    {
        public static EntityConnection BuildEntitySqlConnection(string connectionString)
        {
            MetadataWorkspace workspace = new MetadataWorkspace(new string[] { "res://*/" },
                new Assembly[] { Assembly.GetExecutingAssembly() });

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            var entityConnection = new EntityConnection(workspace, sqlConnection);

            return entityConnection;
        }
    }
}
