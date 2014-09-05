using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PIS.AutoEnt.DataContext
{
    public class SysRegistryObject
    {
        #region Properties

        public RegisterNode Registry { get; protected set; }

        #endregion

        #region Constructors

        public SysRegistryObject(RegisterNode regNode)
        {
            Registry = regNode;
        }

        #endregion

        #region Public Methods

        public RegisterNode GetNode(string Code)
        {
            return null;
        }

        #endregion
    }
}
