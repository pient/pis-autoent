using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;

namespace PIS.AutoEnt.Pattern
{
    public class FrameworkInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //  Register interceptors
            container.Register(
                Component.For<GeneralInterceptor>().LifeStyle.Singleton.Named(InterceptorTypes.General),
                Component.For<PISLoggingInterceptor>().LifeStyle.Singleton.Named(InterceptorTypes.Logging),
                Component.For<PISTransactionInterceptor>().LifeStyle.Singleton.Named(InterceptorTypes.Transaction),
                Component.For<PISExceptionInterceptor>().LifeStyle.Singleton.Named(InterceptorTypes.Exception));
        }
    }
}
