using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using PIS.AutoEnt.Repository.Interfaces;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.Repository
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Unit of work
            container.Register(
                Component.For<ISysUnitOfWork>().ImplementedBy<SysUnitOfWork>().LifestyleTransient());

            // Repository
            container.Register(
                Component.For<IMetaRepository>().ImplementedBy<MetaRepository>().LifestyleTransient(),
                Component.For<IDataStructureRepository>().ImplementedBy<DataStructureRepository>().LifestyleTransient(),
                Component.For<IRegRepository>().ImplementedBy<RegRepository>().LifestyleTransient(),
                Component.For<IObjRepository>().ImplementedBy<ObjRepository>().LifestyleTransient(),

                Component.For<IUserRepository>().ImplementedBy<UserRepository>().LifestyleTransient(),
                Component.For<IRoleRepository>().ImplementedBy<RoleRepository>().LifestyleTransient(),
                Component.For<IGroupRepository>().ImplementedBy<GroupRepository>().LifestyleTransient(),

                Component.For<IAuthRepository>().ImplementedBy<AuthRepository>().LifestyleTransient(),
                Component.For<IModuleRepository>().ImplementedBy<ModuleRepository>().LifestyleTransient());
        }
    }
}
