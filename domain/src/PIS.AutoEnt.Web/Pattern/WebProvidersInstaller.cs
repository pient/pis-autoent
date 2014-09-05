using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using PIS.AutoEnt.App.Interfaces;

namespace PIS.AutoEnt.Web.Pattern
{
    public class WebProvidersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IAppProvider>().ImplementedBy<WebAppProvider>());
        }
    }
}
