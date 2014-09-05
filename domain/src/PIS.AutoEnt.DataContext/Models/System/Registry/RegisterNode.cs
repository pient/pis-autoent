using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PIS.AutoEnt.DataContext
{
    public class RegisterItem
    {
        #region Enums

        public enum TypeEnum
        {
            Default = 0,
            Config = 1,
            Enum = 2,
        }

        #endregion

        #region TypeConveters

        public class TypeEnumConverter : TypeConverter
        {
            #region TypeConverter Members

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(TypeEnum)) { return true; }

                return base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string)) { return true; }

                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                return value.ToString();
            }

            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                return CLRHelper.GetEnum<TypeEnum>(value, TypeEnum.Default, true);
            }

            #endregion
        }

        #endregion

        #region Properties

        [XmlAttribute]
        [TypeConverter(typeof(GuidConverter))]
        public Guid Id { get; set; }

        [XmlAttribute]
        public string Code { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [TypeConverter(typeof(TypeEnumConverter))]
        public TypeEnum RegType { get; set; }

        [XmlElement]
        public object Data { get; set; }

        #endregion

        #region Constructors

        public RegisterItem()
        {
            Id = SystemHelper.NewCombId();
        }

        #endregion
    }

    [Serializable]
    public class RegisterNode : RegisterItem
    {
        #region Properties

        [XmlArray]
        public EasyCollection<RegisterItem> Items
        {
            get;
            set;
        }

        [XmlArray]
        public EasyCollection<RegisterNode> Nodes
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public RegisterNode()
        {
            RegType = TypeEnum.Default;

            Items = new EasyCollection<RegisterItem>();
            Nodes = new EasyCollection<RegisterNode>();
        }

        #endregion
    }
}
