using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.DataContext
{
    public class SysObjWithStructure<T>
        where T : class, ISysStructedObject
    {
        #region Properties

        public T Entity { get; protected set; }

        public SysDataStructure Structure { get; protected set; }

        #endregion

        #region Constructors

        public SysObjWithStructure(T entity, SysDataStructure structure)
        {
            this.Entity = entity;
            this.Structure = structure;
        }

        #endregion
    }
}
