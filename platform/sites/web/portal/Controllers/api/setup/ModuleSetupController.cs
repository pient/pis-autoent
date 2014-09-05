using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Portal.Models.Sys;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.Repository.Interfaces;
using PIS.AutoEnt.Web;
using PIS.AutoEnt.Web.Api;

namespace PIS.AutoEnt.Portal.WebSite.Controllers.api.setup
{
    public class ModuleSetupController : PortalApiControllerBase
    {
        #region Module Setup

        [RequireAuthorize]
        public void DeleteModules(string node)
        {
            var id = node.ToGuid();

            if (!id.IsNullOrEmpty())
            {
                using (var repo = AppDataAccessor.GetRepository<IModuleRepository>())
                {
                    var m = repo.Find(id.Value);

                    repo.Delete(m);
                }
            }
        }

        [RequireAuthorize]
        public void SaveModule(ModuleForm mdlForm)
        {
            using (var repo = AppDataAccessor.GetRepository<IModuleRepository>())
            {
                if (mdlForm.Id.IsNullOrEmpty())
                {
                    var mdl = AutoMapper.Mapper.Map<SysModule>(mdlForm);

                    if (!mdlForm.ParentId.IsNullOrEmpty())
                    {
                        var _p_mdl = repo.Find(mdlForm.ParentId);

                        repo.CreateAsChild(mdl, _p_mdl);
                    }
                }
                else
                {
                    var mdl = repo.Find(mdlForm.Id);
                    var mdl_struc = repo.GetStructure(mdl);

                    CLRHelper.MergeObject(mdl, mdlForm);

                    if (mdl_struc != null)
                    {
                        if (mdlForm.SortIndex != mdl_struc.SortIndex)
                        {
                            mdl_struc.SortIndex = mdlForm.SortIndex;

                            repo.StructRepository.Update(mdl_struc);
                        }
                    }

                    repo.Update(mdl);
                }
            }
        }

        #endregion
    }
}
