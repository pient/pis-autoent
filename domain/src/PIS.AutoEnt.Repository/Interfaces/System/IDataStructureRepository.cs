using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.Repository.Interfaces
{
    public interface IDataStructureRepository : IDataRepository<SysDataStructure>, ISysRepository
    {
        int GetSiblingsCount(SysDataStructure entity);

        SysDataStructure GetPrevSibling(SysDataStructure entity);

        SysDataStructure GetNextSibling(SysDataStructure entity);

        IList<SysDataStructure> GetPrevSiblings(SysDataStructure entity);

        IList<SysDataStructure> GetNextSiblings(SysDataStructure entity);

        int GetChildrenCount(SysDataStructure parent);

        SysDataStructure GetFirstChild(SysDataStructure parent);

        SysDataStructure GetLastChild(SysDataStructure parent);

        SysDataStructure GetChildByPosition(SysDataStructure parent, int position);

        IList<SysDataStructure> GetChildren(SysDataStructure parent);

        void ClearStructure<T>(T entity) where T : ISysStructedObject;

        IQueryable<SysDataStructure> GetAllRelatedQry<T>(T entity) where T : ISysStructedObject;
    }
}
