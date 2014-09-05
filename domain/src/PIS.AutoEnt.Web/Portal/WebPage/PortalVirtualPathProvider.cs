using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Caching;

namespace PIS.AutoEnt.Web
{
    public interface IPortalVirtualPathProvider
    {
        PortalVirtualPathParams Params { get; }
    }

    public class PortalVirtualPathParams
    {
        #region 属性

        /// <summary>
        /// 资源在程序集中相对路径
        /// </summary>
        public string ResourceLocation { get; set; }

        /// <summary>
        /// 虚拟路径
        /// </summary>
        public string VirtualPath { get; set; }

        /// <summary>
        /// 站点Master页面地址
        /// </summary>
        public string SiteMasterPageLocation { get; set; }

        /// <summary>
        /// 表单Master页面地址
        /// </summary>
        public string FormMasterPageLocation { get; set; }

        #endregion

        #region 构造函数

        public PortalVirtualPathParams()
        {
            ResourceLocation = "PIS.Portal";
            VirtualPath = "~/Resources";
            SiteMasterPageLocation = String.Format("{0}/Masters/site.master", VirtualPath);
            FormMasterPageLocation = String.Format("{0}/Masters/formpage.master", VirtualPath);
        }

        #endregion
    }

    /// <summary>
    /// 虚拟目录提供，用于打包公用资源
    /// </summary>
    public abstract class PortalVirtualPathProvider : VirtualPathProvider, IPortalVirtualPathProvider
    {
        #region 成员

        protected PortalVirtualPathParams _Params = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterVirtualPathProvider"/> class.
        /// </summary>
        public PortalVirtualPathProvider()
            : base()
        {
        }

        #endregion

        #region IPortalVirtualPathProvider 成员

        /// <summary>
        /// Portal虚拟路径参数
        /// </summary>
        public PortalVirtualPathParams Params
        {
            get
            {
                if (_Params == null)
                {
                    _Params = GetPortalVirtualPathParams();
                }

                return _Params;
            }
        }

        /// <summary>
        /// 初始化Portal参数
        /// </summary>
        protected virtual PortalVirtualPathParams GetPortalVirtualPathParams()
        {
            return new PortalVirtualPathParams();
        }

        #endregion

        #region VirtualPathProvider成员

        /// <summary>
        /// Gets a value that indicates whether a file exists in the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        /// true if the file exists in the virtual file system; otherwise, false.
        /// </returns>
        public override bool FileExists(string virtualPath)
        {
            if (IsVirtualPath(virtualPath))
            {
                PortalVirtualFile file = GetFile(virtualPath) as PortalVirtualFile;
                return (file == null) ? false : true;
            }
            else
            {
                return Previous.FileExists(virtualPath);
            }
        }

        /// <summary>
        /// Gets a virtual file from the virtual file system.
        /// </summary>
        /// <param name="virtualPath">The path to the virtual file.</param>
        /// <returns>
        /// A descendent of the <see cref="T:System.Web.Hosting.VirtualFile"></see> class that represents a file in the virtual file system.
        /// </returns>
        public abstract override VirtualFile GetFile(string virtualPath);

        /// <summary>
        /// Creates a cache dependency based on the specified virtual paths.
        /// </summary>
        /// <param name="virtualPath">The path to the primary virtual resource.</param>
        /// <param name="virtualPathDependencies">An array of paths to other resources required by the primary virtual resource.</param>
        /// <param name="utcStart">The UTC time at which the virtual resources were read.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Caching.CacheDependency"></see> object for the specified virtual resources.
        /// </returns>
        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return null;
        }

        /// <summary>
        /// Determines whether [is path virtual] [the specified virtual path].
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>
        /// 	<c>true</c> if [is path virtual] [the specified virtual path]; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsVirtualPath(string virtualPath)
        {
            String checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith(Params.VirtualPath, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion
    }
}
