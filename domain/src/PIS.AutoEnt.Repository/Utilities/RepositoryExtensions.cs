using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public static class RepositoryExtensions
    {
        #region MetaObject Repository

        public static SysMetadata GetMetadata<T>(this IMetaObjectRepository<T> repo, T entity)
            where T : class, ISysMetaObject
        {
            var metadata = repo.DbContext.SysMetadatas.Find(entity.MetadataId);

            return metadata;
        }

        public static SysMetadata GetMetadata(this ISysMetaObject metaObject, IMetaRepository repo = null)
        {
            SysMetadata metadata = null;

            if (repo == null)
            {
                using (repo = ObjectFactory.Resolve<IMetaRepository>())
                {
                    metadata = repo.Find(metaObject.MetadataId);
                }
            }
            else
            {
                metadata = repo.Find(metaObject.MetadataId);
            }

            return metadata;
        }

        #endregion

        #region StdObject Repository

        public static T FindByCode<T>(this IStdObjRepository<T> repo, string code)
            where T : class, ISysStdObject
        {
            var entity = repo.DbContext.Set<T>().Where(e => e.Code == code).FirstOrDefault();

           return entity;
        }

        #endregion

        #region Structed Repository

        private const char StructSeparator = '.';

        private static string CombineStructPath(string path1, string path2)
        {
            if (String.IsNullOrEmpty(path1))
            {
                return path2;
            }
            else if (String.IsNullOrEmpty(path2))
            {
                return path1;
            }
            else
            {
                return String.Format("{0}{1}{2}", path1.Trim(StructSeparator), StructSeparator, path2.Trim(StructSeparator));
            }
        }

        public static IQueryable<SysDataStructure> GetStructureQry<T>(IStructedRepository<T> repo, string structureCode = null)
            where T : class, ISysStructedObject
        {
            string _s_code = ModelHelper.GetStructureCode<T>(structureCode);

            var sQry = repo.DbContext.SysDataStructures.Where(s => s.StructureCode == _s_code)
                .OrderBy(s => s.SortIndex).OrderByDescending(s => s.ObjectId);

            return sQry;
        }
        
        /// <summary>
        /// 由结构获取对象结构列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="structureQuery"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static List<SysObjWithStructure<T>> QueryObjsWithStructure<T>(this IStructedRepository<T> repo, IQueryable<SysDataStructure> structureQuery)
            where T : class, ISysStructedObject
        {
            var ctx = repo.DbContext;

            var qry = (from e in ctx.Set<T>()
                       join s in structureQuery
                       on e.Id equals s.ObjectId
                       orderby s.SortIndex ascending, e.Id descending
                       select new { Entity = e, Structure = s });

            var list = qry.ToList().
                Select(e => new SysObjWithStructure<T>(e.Entity, e.Structure)).ToList();

            return list;
        }

        /// <summary>
        /// 获取结构实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repo"></param>
        /// <param name="entity"></param>
        /// <param name="structureCode"></param>
        /// <returns></returns>
        public static SysDataStructure GetStructure<T>(this IStructedRepository<T> repo, T entity, string structureCode = null)
            where T : class, ISysStructedObject
        {
            var sQry = GetStructureQry(repo, structureCode)
                .Where(s => s.ObjectId == entity.Id);

            var obj = sQry.FirstOrDefault();

            return obj;
        }

        /// <summary>
        /// 获取所有根节点
        /// </summary>
        /// <returns></returns>
        public static IList<SysObjWithStructure<T>> GetRoots<T>(this IStructedRepository<T> repo, int? rootLevel = null, string structureCode = null)
            where T : class, ISysStructedObject
        {
            int _rootLevel = (rootLevel == null ? ModelHelper.GetRootLevel<T>() : rootLevel.Value);

            IQueryable<SysDataStructure> sQry = null;

            if (rootLevel == null)
            {
                sQry = GetStructureQry(repo, structureCode)
                   .Where(s => (s.ParentId == null));
            }
            else
            {
                sQry = GetStructureQry(repo, structureCode)
                   .Where(s => (s.PathLevel == _rootLevel));
            }

            var roots = repo.QueryObjsWithStructure(sQry);

            return roots;
        }

        /// <summary>
        /// 获取所有祖先级节点
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static IList<SysObjWithStructure<T>> GetAncestors<T>(this IStructedRepository<T> repo, T entity, string structureCode = null)
            where T : class, ISysStructedObject
        {
            var _s = repo.GetStructure(entity, structureCode);

            var _a_ids = _s.Path.Split(StructSeparator).Where(p => !String.IsNullOrEmpty(p)).Select(p => Guid.Parse(p));

            var sQry = GetStructureQry(repo, structureCode).Where(s => _a_ids.Contains(s.ObjectId));

            var list = repo.QueryObjsWithStructure(sQry);

            return list;
        }

        /// <summary>
        /// 获取相应层级祖先级节点
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static SysObjWithStructure<T> GetAncestor<T>(this IStructedRepository<T> repo, T entity, int level, string structureCode = null)
            where T : class, ISysStructedObject
        {
            var _s = repo.GetStructure(entity, structureCode);

            if (level >= _s.PathLevel || String.IsNullOrEmpty(_s.Path))
            {
                return null;
            }
            else
            {
                var _a_id = _s.Path.Split(StructSeparator)[level];

                if (!String.IsNullOrEmpty(_a_id))
                {
                    var _obj_id = Guid.Parse(_a_id);

                    if (_obj_id != Guid.Empty)
                    {
                        var sQry = GetStructureQry(repo, structureCode).Where(s => s.ObjectId == _obj_id);

                        var obj = repo.QueryObjsWithStructure(sQry).FirstOrDefault();

                        return obj;
                    }
                }

                return null;
            }
        }
        
        /// <summary>
        /// 获取相应层级子孙裔节点, level为空获取所有子孙裔节点
        /// </summary>
        /// <param name="level">当前层级向下层级</param>
        /// <returns></returns>
        public static IList<SysObjWithStructure<T>> GetDescendants<T>(this IStructedRepository<T> repo, T entity, int? level = null, string structureCode = null)
            where T : class, ISysStructedObject
        {
            var _s = repo.GetStructure(entity, structureCode);

            if (level <= _s.PathLevel)
            {
                return null;
            }
            else
            {
                string idStr = entity.Id.ToString();

                var sQry = GetStructureQry(repo, structureCode).Where(s => s.Path.Contains(idStr));

                if (level != null)
                {
                    sQry = sQry.Where(s => s.PathLevel == level);
                }

                var list = repo.QueryObjsWithStructure(sQry);

                return list;
            }
        }

        /// <summary>
        /// 获取相应层级祖先级节点
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static SysObjWithStructure<T> GetParent<T>(this IStructedRepository<T> repo, T entity, string structureCode = null)
            where T : class, ISysStructedObject
        {
            var _s = repo.GetStructure(entity, structureCode);

            if (_s.ParentId.IsNullOrEmpty())
            {
                return null;
            }
            else
            {
                var sQry = GetStructureQry(repo, structureCode).Where(s => s.ObjectId == _s.ParentId);

                var obj = QueryObjsWithStructure(repo, sQry).FirstOrDefault();

                return obj;
            }
        }

        /// <summary>
        /// 获取所有子节点
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static IList<SysObjWithStructure<T>> GetChildren<T>(this IStructedRepository<T> repo, T entity, string structureCode = null)
            where T : class, ISysStructedObject
        {
            var _s = repo.GetStructure(entity, structureCode);

            if (_s.IsLeaf == true)
            {
                return null;
            }
            else
            {
                var sQry = GetStructureQry(repo, structureCode).Where(s => s.ParentId == _s.ObjectId);
                var list = QueryObjsWithStructure(repo, sQry);

                return list;
            }
        }

        /// <summary>
        /// 由子节点位置获取子节点
        /// </summary>
        /// <param name="position">从1开始</param>
        /// <returns></returns>
        private static SysObjWithStructure<T> GetChildByPosition<T>(this IStructedRepository<T> repo, T entity, int position, string structureCode = null)
            where T : class, ISysStructedObject
        {
            var _s = repo.GetStructure(entity, structureCode);

            if (_s.IsLeaf == true)
            {
                return null;
            }
            else
            {
                var sQry = GetStructureQry(repo, structureCode).Where(s => s.ParentId == entity.Id).Skip(position).Take(1);

                var node = repo.QueryObjsWithStructure(sQry).FirstOrDefault();

                return node;
            }
        }

        /// <summary>
        /// 添加顶层节点
        /// </summary>
        public static SysObjWithStructure<T> CreateAsRoot<T>(this IStructedRepository<T> repo,
            T entity, string structureCode = null, Func<SysObjWithStructure<T>, bool> OnCreate = null)
            where T : class, ISysStructedObject
        {
            repo.Create(entity);

            var _s = new SysDataStructure(entity.Id, ModelHelper.GetStructureCode<T>(structureCode))
            {
                ParentId = null,
                Path = null,
                PathLevel = ModelHelper.GetRootLevel<T>(),
                SortIndex = 1,
                IsLeaf = true
            };

            repo.StructRepository.Create(_s);

            var _obj_s = new SysObjWithStructure<T>(entity, _s);

            if (OnCreate != null)
            {
                OnCreate(_obj_s);
            }

            repo.DbContext.SaveChanges();   // Must Save result here

            return _obj_s;
        }

        /// <summary>
        /// 添加兄弟节点(添加后排序号将比当前排序号大一个间隔)
        /// </summary>
        public static SysObjWithStructure<T> CreateAsSibling<T>(this IStructedRepository<T> repo,
            T entity, T siblingEntity, string structureCode = null, Func<SysObjWithStructure<T>, bool> OnCreate = null)
            where T : class, ISysStructedObject
        {
            repo.Create(entity);

            var _s_code = ModelHelper.GetStructureCode<T>(structureCode);
            var _s_s = repo.GetStructure<T>(siblingEntity, _s_code);

            var _s = new SysDataStructure(entity.Id, _s_code)
            {
                ParentId = _s_s.ParentId,
                Path = _s_s.Path,
                PathLevel = _s_s.PathLevel,
                SortIndex = (_s_s.SortIndex + 1)
            };

            var _n_s = repo.StructRepository.GetNextSiblings(_s_s);
            if (_n_s != null && _n_s.Count > 0)
            {
                _n_s.All((e) =>
                {
                    e.SortIndex += 1;
                    repo.StructRepository.Update(e);

                    return true;
                });
            }

            repo.StructRepository.Create(_s);

            var _obj_s = new SysObjWithStructure<T>(entity, _s);

            if (OnCreate != null)
            {
                OnCreate(_obj_s);
            }

            repo.DbContext.SaveChanges();   // Must Save result here

            return new SysObjWithStructure<T>(entity, _s);
        }

        /// <summary>
        /// 插入到指定父节点值定位置
        /// </summary>
        /// <param name="position"></param>
        public static SysObjWithStructure<T> CreateAsChild<T>(this IStructedRepository<T> repo,
            T entity, T parent, int? sortIndex = null, string structureCode = null, Func<SysObjWithStructure<T>, bool> OnCreate = null)
            where T : class, ISysStructedObject
        {
            SysObjWithStructure<T> _rtn_s = null;

            var _s_code = ModelHelper.GetStructureCode<T>(structureCode);
            var _s_parent = repo.GetStructure<T>(parent, _s_code);
            
            var _s = new SysDataStructure(entity.Id, _s_code)
            {
                ParentId = _s_parent.ObjectId,
                Path = CombineStructPath(_s_parent.Path, _s_parent.ObjectId.ToString()),
                PathLevel = _s_parent.PathLevel + 1
            };

            sortIndex = (sortIndex < 1 ? 1 : sortIndex);   // 位置从1开始

            SysDataStructure _s_last = null;

            if (_s_parent.IsLeaf != true)
            {
                _s_last = repo.StructRepository.GetLastChild(_s_parent);
            }

            if (_s_parent.IsLeaf == true || _s_last == null)    // 不存在子节点时
            {
                repo.Create(entity);

                _s_parent.IsLeaf = false;

                repo.StructRepository.Update(_s_parent);

                _s.Id = entity.Id;
                _s.ObjectId = entity.Id;
                _s.SortIndex = sortIndex;
                repo.StructRepository.Create(_s);

                _rtn_s = new SysObjWithStructure<T>(entity, _s);
            }
            else
            {
                if (sortIndex == null)
                {
                    sortIndex = (_s_last.SortIndex == null ? 1 : (_s_last.SortIndex.Value + 1));
                }

                repo.Create(entity);

                _s.Id = entity.Id;
                _s.ObjectId = entity.Id;
                _s.SortIndex = sortIndex;

                repo.StructRepository.Create(_s);

                _rtn_s = new SysObjWithStructure<T>(entity, _s);
            }

            if (OnCreate != null)
            {
                OnCreate(_rtn_s);
            }

            repo.DbContext.SaveChanges();   // Must Save result here

            return _rtn_s;
        }

        /// <summary>
        /// 交换同级节点位置
        /// </summary>
        public static void ExchangeSiblingPosition<T>(this IStructedRepository<T> repo, T entity, T target, string structureCode = null)
            where T : class, ISysStructedObject
        {
            var _e_s = repo.GetStructure(entity, structureCode);
            var _t_s = repo.GetStructure(target, structureCode);

            if (_e_s.ParentId != _t_s.ParentId)
            {
                throw new InvalidOperationException("the entities are not sibling nodes");
            }

            int? s_index = _e_s.SortIndex;

            _e_s.SortIndex = _t_s.SortIndex;
            _t_s.SortIndex = s_index;

            repo.StructRepository.Update(_e_s);
            repo.StructRepository.Update(_t_s);
        }

        /// <summary>
        /// 改变当前节点的父节点
        /// </summary>
        /// <param name="targetID"></param>
        /// <param name="position">当前节点在新父节点的位置</param>
        public static T MoveAsChild<T>(this IStructedRepository<T> repo, T entity, T target, int? sortIndex = null, string structureCode = null)
            where T : class, ISysStructedObject
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移动为目标节点兄弟节点
        /// </summary>
        public static T MoveAsSibling<T>(this IStructedRepository<T> repo, T entity, T target, string structureCode = null)
            where T : class, ISysStructedObject
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 复制为目标节点子节点
        /// </summary>
        public static T CopyAsChild<T>(this IStructedRepository<T> repo, T entity, T target, string structureCode = null)
            where T : class, ISysStructedObject
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 复制为目标节点兄弟节点
        /// </summary>
        public static T CopyAsSibling<T>(this IStructedRepository<T> repo, T entity, T target, string structureCode = null)
            where T : class, ISysStructedObject
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        public static T ClearChildren<T>(this IStructedRepository<T> repo, T entity, string structureCode = null)
            where T : class, ISysStructedObject
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
