using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using PIS.AutoEnt.App.Interfaces;
using PIS.AutoEnt.Framework.Tests;

namespace PIS.AutoEnt.Domain.Tests
{
    public class TestProvidersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IAppProvider>().ImplementedBy<TestAppProvider>());
        }
    }
}
