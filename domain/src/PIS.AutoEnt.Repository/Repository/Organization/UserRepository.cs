using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.DataContext;
using PIS.AutoEnt.Repository.Interfaces;

namespace PIS.AutoEnt.Repository
{
    public class UserRepository : OrgRepository<OrgUser>, IUserRepository
    {
        #region Constructors

        public UserRepository()
        {
        }

        public UserRepository(SysDbContext ctx)
            : base(ctx)
        {
        }

        #endregion

        #region IUserRepository Members

        public OrgUser FindByLoginName(string loginName)
        {
            var usr = this.Find(e => e.LoginName == loginName);

            return usr;
        }

        public bool VerifyPassword(OrgUser user, string password)
        {
            var meta = this.GetMetadata(user);

            var tag = this.RetrieveTag(meta);

            bool result = (password == tag.Password);

            return result;
        }

        public void ChangePassword(OrgUser user, string password)
        {
            var meta = this.GetMetadata(user);

            var tag = this.RetrieveTag(meta);

            tag.Password = password;

            meta.XMetaTag = CLRHelper.SerializeToXmlString(tag);

            MetaRepository.Update(meta);
        }

        #endregion

        #region Support Methods

        private OrgUser.UserTag RetrieveTag(SysMetadata meta)
        {
            OrgUser.UserTag tag = null;

            var tagstr = meta.XMetaTag;

            if (!String.IsNullOrEmpty(tagstr))
            {
                tag = CLRHelper.DeserializeFromXmlString<OrgUser.UserTag>(tagstr);
            }

            if (tag == null)
            {
                tag = new OrgUser.UserTag();
            }

            return tag;
        }

        #endregion
    }
}
