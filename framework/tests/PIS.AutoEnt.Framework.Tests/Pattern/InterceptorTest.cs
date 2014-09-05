using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PIS.AutoEnt.Tests.Common.Pattern
{
    [TestClass]
    public class InterceptorTest
    {
        [TestInitialize]
        public void Setup()
        {
            Sys.AppInitializer.Initialize();
        }

        [TestMethod]
        public void Framework_Interceptor_Resolve()
        {
            ObjectFactory.Container.Register(Component.For<IIntercepted>().ImplementedBy<Intercepted>());
            var instance = ObjectFactory.Resolve<IIntercepted>();

            // WindsorManager.Container.Register(Component.For<AbstractIntercepted>().ImplementedBy<Intercepted>());
            // var instance = WindsorManager.Resolve<AbstractIntercepted>();

            try
            {
                instance.Test();
            }
            catch { }

            Console.WriteLine("--------");

            try
            {
                instance.OuterTest();
            }
            catch { }
        }

        #region Support Classes

        [PISInterceptor]
        public interface IIntercepted
        {
            [PISLogging(Message = "Test Message")]
            void Test();

            [PISTransaction]
            [PISLogging(Message = "Outer Test Message")]
            void OuterTest(); 
        }

        [PISInterceptor]
        public abstract class AbstractIntercepted
        {
            public abstract void Test();

            public abstract void OuterTest();
        }

        public class Intercepted : AbstractIntercepted, IIntercepted
        {
            public override void Test()
            {
                Console.WriteLine("Intercepted Test");

                InnerTest();
                OuterTest();
                PublicTest();
            }

            public override void OuterTest()
            {
                Console.WriteLine("Intercepted OuterTest");

                throw new Exception("OuterTest Outer Exception ", new Exception("OuterTest Inner Exception "));
            }

            public void PublicTest()
            {
                Console.WriteLine("Intercepted PublicTest");
            }

            private void InnerTest()
            {
                Console.WriteLine("Intercepted InnerTest");
            }
        }

        #endregion
    }
}
