using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIS.AutoEnt
{
    public enum EntityObjectState
    {
        Added,
        Modified,
        Deleted
    }

    public interface IEntityContext : IDisposable
    {
        void Register(IEntityObject entity, EntityObjectState state);

        int SaveChanges();
    }
}
