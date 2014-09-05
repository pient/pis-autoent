using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Framework.Tests
{
    [TestClass]
    public class TestingInitializer
    {
        public static readonly Guid TestObjectId = new Guid("39bfe4f5-9200-4522-a173-6de37e77ab82");
        public const string TestObjectCode = "TEST_OBJECT";

        public static SysObject InitializeSysTestObject()
        {
            SysObject obj = null;

            using (var uow = SysDataAccessor.NewUnitOfWork())
            {
                var repo = AppDataAccessor.GetRepository<IObjRepository>(uow.Context as SysDbContext);

                obj = repo.Find(TestObjectId);

                if (obj == null)
                {
                    obj = new SysObject()
                    {
                        Id = TestObjectId,
                        Code = TestObjectCode
                    };

                    string id = obj.Id.ToString();

                    obj = repo.Create(obj);
                }

                uow.Commit();

                return obj;
            }
        }

        public static void ClearSysTestObject()
        {
            using (var uow = SysDataAccessor.NewUnitOfWork())
            {
                var repo = AppDataAccessor.GetRepository<IObjRepository>(uow.Context as SysDbContext);

                SysObject obj = repo.Find(TestObjectId);

                if (obj != null)
                {
                    repo.Delete(obj);
                }

                uow.Commit();
            }
        }
    }
}
