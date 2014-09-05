using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.DataContext
{
    public static class ModelHelper
    {
        #region ISysMetaObject

        public static string GetObjectCode<T>() where T : ISysMetaObject
        {
            return typeof(T).Name;
        }

        #endregion

        #region ISysStructedObject

        public static string GetStructureCode<T>(string structureCode = null) 
            where T : ISysStructedObject
        {
            return String.IsNullOrEmpty(structureCode) ? GetDefStructureCode<T>() : structureCode;
        }

        public static string GetDefStructureCode<T>() 
            where T : ISysStructedObject
        {
            return GetObjectCode<T>();
        }

        public static int GetRootLevel<T>() 
            where T : ISysStructedObject
        {
            return 0;
        }

        #endregion
    }
}
