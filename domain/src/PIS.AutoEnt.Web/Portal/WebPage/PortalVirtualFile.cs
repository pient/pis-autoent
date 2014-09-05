using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace PIS.AutoEnt.Web
{
    /// <summary>
    /// 虚拟目录文件
    /// </summary>
    public class PortalVirtualFile : VirtualFile
    {
        protected PortalVirtualPathProvider provider;
        protected string virtualPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortalVirtualFile"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path to the resource represented by this instance.</param>
        public PortalVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            this.virtualPath = virtualPath;
        }

        public PortalVirtualFile(string virtualPath, PortalVirtualPathProvider provider)
            : this(virtualPath)
        {
            this.provider = provider;
        }

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>A read-only stream to the virtual file.</returns>
        public override Stream Open()
        {
            if (!(HttpContext.Current == null))
            {
                if (HttpContext.Current.Cache[virtualPath] == null)
                {
                    HttpContext.Current.Cache.Insert(virtualPath, ReadResource(virtualPath));
                }
                return (Stream)HttpContext.Current.Cache[virtualPath];
            }
            else
            {
                return ReadResource(virtualPath);
            }
        }

        protected virtual Stream ReadResource(string embeddedFileName)
        {
            string resourceDirectory = VirtualPathUtility.GetDirectory(embeddedFileName);

            if (!String.IsNullOrEmpty(resourceDirectory))
            {
                resourceDirectory = resourceDirectory.TrimStart("/" + SystemHelper.GetApplicationBaseName()).Replace('/', '.');

                if (String.IsNullOrEmpty(resourceDirectory))
                {
                    resourceDirectory = ".";
                }
            }

            string resourceFileName = VirtualPathUtility.GetFileName(embeddedFileName);

            Assembly assembly = null;
            string resourceLocation = String.Empty;

            if (this.provider != null)
            {
                assembly = provider.GetType().Assembly;
                resourceLocation = provider.Params.ResourceLocation;
            }
            else
            {
                assembly = Assembly.GetExecutingAssembly();
            }

            if (String.IsNullOrEmpty(this.provider.Params.ResourceLocation))
            {
                resourceLocation = assembly.GetName().Name;
            }

            Stream stream = assembly.GetManifestResourceStream(resourceLocation + resourceDirectory + resourceFileName);

            return stream;
        }
    }
}
