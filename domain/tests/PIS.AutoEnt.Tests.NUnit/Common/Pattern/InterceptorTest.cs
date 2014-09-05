using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using NUnit.Framework;

namespace PIS.Framework.Tests.Common.Pattern
{
    [TestFixture]
    public class InterceptorTest
    {
        [SetUp]
        public void Setup()
        {
            AppSystem.Initialize();

        }

        [Test]
        public void Interceptor()
        {
            ObjectManager.Container.Register(Component.For<IIntercepted>().ImplementedBy<Intercepted>());
            var instance = ObjectManager.Resolve<IIntercepted>();

            // WindsorManager.Container.Register(Component.For<AbstractIntercepted>().ImplementedBy<Intercepted>());
            // var instance = WindsorManager.Resolve<AbstractIntercepted>();
            
            instance.Test();

            Console.WriteLine("--------");

            instance.OuterTest();
        }

        [Interceptor("General")]
        public interface IIntercepted
        {
            [PISLogging(Message = "Test Message")]
            void Test();

            [PISTransaction]
            [PISLogging(Message="Outer Test Message")]
            void OuterTest(); 
        }

        [Interceptor("General")]
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
    }
}
