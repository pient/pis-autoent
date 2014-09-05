using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.Core.Resource;
using Castle.Windsor.Configuration.Interpreters;
using PIS.AutoEnt.Pattern;
using System.Collections;

namespace PIS.AutoEnt
{
    [PISInterceptor]
    public class ObjectFactory : IObjectFactory, IDisposable
    {
        #region 成员属性

        public static object locker = new object(); // 添加一个对象作为UserContextList的锁

        public static IWindsorContainer Container
        {
            get
            {
                return Instance.container;
            }
        }

        #endregion

        #region 构造函数

        protected IWindsorContainer container = null;

        private static ObjectFactory instance;

        internal static ObjectFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ObjectFactory();
                }

                return instance;
            }
        }

        protected ObjectFactory()
        {

        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
            }
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 为Windsor加载配置文件并初始化Container
        /// </summary>
        /// <returns></returns>
        [PISLogging]
        public static void Configure()
        {
            IResource resource = new FrameworkConfigResource();

            lock (locker)
            {
                Instance.container = new WindsorContainer(new XmlInterpreter(resource));

                // 手动调用installer
                FrameworkInstaller finstaller = new FrameworkInstaller();
                finstaller.Install(Instance.container, null);
            }
        }

        /// <summary>
        /// 获取代理对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return Instance.container.Resolve<T>();
        }

        public static T Resolve<T>(IDictionary args)
        {
            return Instance.container.Resolve<T>(args);
        }

        #endregion
    }
}
