using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PIS.AutoEnt.DataContext
{
    public class RegistryObject
    {
        #region Properties

        [XmlElement]
        public RegisterNode Registry { get; internal set; }

        #endregion

        #region Constructors

        public RegistryObject()
            : this(new RegisterNode())
        {
        }

        public RegistryObject(RegisterNode regNode)
        {
            Registry = regNode;
        }

        public RegistryObject(string regxml)
        {
            Registry = CLRHelper.DeserializeFromXmlString<RegisterNode>(regxml);
        }

        #endregion

        #region Public Methods

        public string ToXmlString()
        {
            string regxml = CLRHelper.SerializeToXmlString(Registry);

            return regxml;
        }

        #endregion
    }
}
