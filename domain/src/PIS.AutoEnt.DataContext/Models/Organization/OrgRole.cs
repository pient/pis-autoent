using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.DataContext
{
    /// <summary>
    /// 角色分以下几类，系统管理、行政组织、项目管理、自定义（基本与角色类型对应）
    /// 系统管理职能一般赋予系统管理员和开发部署人员，包括管理员角色，开发角色和部署角色等
    /// 行政组织赋予系统用户，表示组织结构
    /// </summary>
    public partial class OrgRole : ISysStdObject
    {
    }
}
