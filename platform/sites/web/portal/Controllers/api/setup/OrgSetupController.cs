using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PIS.AutoEnt.App;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Portal.Models.Org;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.Repository.Interfaces;
using PIS.AutoEnt.Web.Api;
using PIS.AutoEnt.XData.DataStore;

namespace PIS.AutoEnt.Portal.WebSite.Controllers.api.setup
{
    public class OrgSetupController : PortalApiControllerBase
    {
        #region User Setup

        [RequireAuthorize]
        public object QueryUsers(DataRequest request)
        {
            var pageData = new object();

            var qryRequest = request.GetQueryRequest();

            if (request != null)
            {
                var qryExpr = qryRequest.GetExpression("OrgUser");

                if (qryExpr.Sorters.Count <= 0)
                {
                    qryExpr.Sorters.Add(new Data.Query.QryOrder("Code"));
                }

                var result = SysDataAccessor.QueryData(qryExpr);

                pageData = result;
            }

            return pageData;
        }

        [RequireAuthorize]
        public void DeleteUser(string id)
        {
            var u_id = id.ToGuid();

            if (!u_id.IsNullOrEmpty())
            {
                AppSecurity.RemoveAccount(u_id.Value);
            }
        }

        [RequireAuthorize]
        public void SaveUser(UserForm frm)
        {
            using (var repo = AppDataAccessor.GetRepository<IUserRepository>())
            {
                if (frm.Id.IsNullOrEmpty())
                {
                    var user = AutoMapper.Mapper.Map<OrgUser>(frm);
                    repo.Create(user);
                }
                else
                {
                    var user = repo.Find(frm.Id);

                    CLRHelper.MergeObject(user, frm);

                    repo.Update(user);
                }
            }
        }

        #endregion
    }
}
