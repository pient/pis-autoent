using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.Tests.DataAccess.EF.Contexts
{
    [TestClass]
    public class SysDbContextTest
    {
        [TestMethod]
        public void Domain_SysDbContext_InheritanceTest()
        {
            Guid objId = SystemHelper.NewCombId();

            using (var ctx = new SysDbContext())
            {
                SysMetadata metadata = new SysMetadata()
                {
                    ObjectId = objId,
                    ObjectCode = "obj.FUNC_CODE"
                };

                ctx.SysMetadatas.Add(metadata);

                SysObject obj = new SysObject()
                {
                    Id = objId,
                    Code = "C_OBJ",
                    Name = "SysObject"
                };

                obj.MetadataId = metadata.Id;

                ctx.SysObjects.Add(obj);
                ctx.SaveChanges();
            }

            using (var ctx = new SysDbContext())
            {
                SysObject obj = ctx.SysObjects.FirstOrDefault(o => o.Id == objId);

                if (obj != null)
                {
                    SysMetadata metadata = ctx.SysMetadatas.Find(obj.MetadataId);
                    ctx.SysMetadatas.Remove(metadata);

                    ctx.SysObjects.Remove(obj);
                }

                ctx.SaveChanges();
            }
        }
    }
}
