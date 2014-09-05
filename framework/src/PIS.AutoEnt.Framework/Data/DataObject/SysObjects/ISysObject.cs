using System;

namespace PIS.AutoEnt.Data
{
    public interface ISysObject : IEntityObject
    {
        Guid Id { get; set; }
    }
}
