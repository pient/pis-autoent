using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository;

namespace PIS.AutoEnt.Tests.DataAccess.EF.Contexts
{
    [TestClass]
    public class OrgDbContextTest
    {
        [TestMethod]
        public void Domain_OrgDbContext_RelationTest()
        {
            OrgRole role = null;

            OrgFunction f = new OrgFunction()
            {
                Code = "OrgFunction",
                Name = "TestFunction"
            };

            role = new OrgRole()
            {
                Code = "OrgRole",
                Name = "TestRole",
                OrgFunction = f
            };

            using (var ctx = new SysDbContext())
            {
                OrgRole t_r = ctx.OrgRoles.FirstOrDefault(m => m.Code == role.Code);

                if (t_r != null)
                {
                    ctx.Entry(t_r).State = EntityState.Deleted;
                }

                OrgFunction t_f = ctx.OrgFunctions.FirstOrDefault(m => m.Code == f.Code);

                if (t_f != null)
                {
                    ctx.Entry(t_f).State = EntityState.Deleted;
                }

                ctx.SaveChanges();
            }

            using (var ctx = new SysDbContext())
            {
                role.OrgFunctions.Add(f);

                ctx.OrgRoles.Add(role);
                ctx.SaveChanges();

                role = ctx.OrgRoles.FirstOrDefault();
            }

            using (var ctx = new SysDbContext())
            {
                if (role != null)
                {
                    ctx.Entry(role).State = EntityState.Unchanged;
                    role.OrgFunctions.Count();
                }
            }

            using (var ctx = new SysDbContext())
            {
                OrgRole t_r = ctx.OrgRoles.FirstOrDefault(m => m.Code == role.Code);

                if (t_r != null)
                {
                    ctx.Entry(t_r).State = EntityState.Deleted;
                }

                OrgFunction t_f = ctx.OrgFunctions.FirstOrDefault(m => m.Code == f.Code);

                if (t_f != null)
                {
                    ctx.Entry(t_f).State = EntityState.Deleted;
                }

                ctx.SaveChanges();
            }
        }
    }
}
