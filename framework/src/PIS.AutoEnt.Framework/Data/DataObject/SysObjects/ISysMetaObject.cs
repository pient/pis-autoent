using System;

namespace PIS.AutoEnt.Data
{
    public interface ISysMetaObject : ISysObject
    {
        Guid MetadataId { get; set; }
    }
}
