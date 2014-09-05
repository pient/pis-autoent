using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public static class ModuleRepositoryExtensions
    {
        #region Structed Repository

        /// <summary>
        /// 添加顶层节点
        /// </summary>
        public static SysObjWithStructure<SysModule> CreateAsRoot(this IModuleRepository repo,
            SysModule entity, string structureCode = null)
        {
            var _obj_s = RepositoryExtensions.CreateAsRoot(repo, entity, structureCode);

            CreateAuthByModule(repo, _obj_s);

            return _obj_s;
        }

        /// <summary>
        /// 添加兄弟节点(添加后排序号将比当前排序号大一个间隔)
        /// </summary>
        public static SysObjWithStructure<SysModule> CreateAsSibling(this IModuleRepository repo,
            SysModule entity, SysModule siblingEntity, string structureCode = null)
        {
            var _obj_s = RepositoryExtensions.CreateAsSibling(repo, entity, siblingEntity, structureCode);

            CreateAuthByModule(repo, _obj_s);

             return _obj_s;
        }

        /// <summary>
        /// 插入到指定父节点值定位置
        /// </summary>
        /// <param name="position"></param>
        public static SysObjWithStructure<SysModule> CreateAsChild(this IModuleRepository repo,
            SysModule entity, SysModule parent, int? position = null, string structureCode = null)
        {
            var _obj_s = RepositoryExtensions.CreateAsChild(repo, entity, parent, position, structureCode);

            CreateAuthByModule(repo, _obj_s);

            return _obj_s;
        }

        public static SysAuth GetRelatedAuth(this IModuleRepository repo, Guid moduleId)
        {
            if (moduleId == Guid.Empty)
            {
                return null;
            }

            var _mdlid = moduleId.ToString();
            var _mdlstr = SysAuth.TypeEnum.Module.ToString();

            var _auth = repo.AuthRepository.Find(e => e.Type == _mdlstr && e.AuthObjId == _mdlid);

            return _auth;
        }

        public static SysModule GetModuleByAuth(this IModuleRepository repo, SysAuth auth)
        {
            if (auth.AuthType != SysAuth.TypeEnum.Module)
            {
                return null;
            }

            var _mdlid = auth.AuthObjId.ToGuid();

            var _module = repo.Find(_mdlid.Value);

            return _module;
        }

        /// <summary>
        /// 由结构获取对象结构列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repo"></param>
        /// <returns></returns>
        public static List<SysModuleWithStructure> QueryModulesWithStructure(this IModuleRepository repo, Expression<Func<SysModule, bool>> predicate = null, string structureCode = null)
        {
            string _s_code = ModelHelper.GetStructureCode<SysModule>(structureCode);

            var ctx = repo.DbContext;

            var qry = (from e in ctx.SysModules.Where(predicate)
                       join s in ctx.SysDataStructures.Where(e=>e.StructureCode == _s_code)
                       on e.Id equals s.ObjectId
                       orderby s.SortIndex ascending, e.Id descending
                       select new { Entity = e, Structure = s });

            var list = qry.ToList().
                Select(e => new SysModuleWithStructure(e.Entity, e.Structure)).ToList();

            return list;
        }

        #endregion

        #region Support Methods

        internal static string GetAuthCode(this SysModule module)
        {
            return String.Format("AUTH_MDL_{0}", module.Code);
        }

        internal static SysAuth MapToAuth(this SysModule module, SysAuth auth = null)
        {
            auth = (auth == null ? new SysAuth() : auth);

            auth.Name = module.Name;
            auth.Code = module.GetAuthCode();
            auth.AuthType = SysAuth.TypeEnum.Module;
            auth.AuthObjId = module.Id.ToString();
            auth.Status = module.Status;
            auth.IsPublic = module.IsPublic;

            return auth;
        }

        internal static SysAuth CreateAuthByModule(this IModuleRepository repo, SysObjWithStructure<SysModule> entityStructure)
        {
            SysAuth _p_auth = null;

            var _mdl = entityStructure.Entity;
            var _mdl_s = entityStructure.Structure;

            // 获取父权限（与系统模块对应）
            if (!_mdl_s.ParentId.IsNullOrEmpty())
            {
                _p_auth = GetRelatedAuth(repo, _mdl_s.ParentId.Value);
            }

            var _auth = MapToAuth(_mdl);
            SysObjWithStructure<SysAuth> _auth_s = null;

            if (_p_auth != null)
            {
                _auth_s = repo.AuthRepository.CreateAsChild(_auth, _p_auth);
            }
            else
            {
                _auth_s = repo.AuthRepository.CreateAsRoot(_auth);
            }

            _auth_s.Structure.SortIndex = _mdl_s.SortIndex;

            repo.StructRepository.Update(_auth_s.Structure);

            repo.DbContext.SaveChanges();   // Should Save Change immediately

            return _auth;
        }

        #endregion
    }
}
