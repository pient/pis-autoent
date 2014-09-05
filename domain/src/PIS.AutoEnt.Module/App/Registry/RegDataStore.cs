using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.Repository;
using PIS.AutoEnt.Repository.Interfaces;
using PIS.AutoEnt.XData.DataStore;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.App
{
    public class RegDataStore : XDataStore, IDataStore
    {
        #region Properties

        public static object lockObj = new object();

        private SysRegDataRegion sysRegion;

        public SysRegDataRegion SysRegion
        {
            get
            {
                if (sysRegion == null)
                {
                    lock (lockObj)
                    {
                        sysRegion = this.GetSysRegion();
                    }
                }

                return sysRegion;
            }
        }

        public bool IsAppInitialized
        {
            get
            {
                bool isInitialized = (SysRegion.SystemStatus != SysRegDataRegion.SystemStatusEnum.UnInitialized
                    && SysRegion.SystemStatus != SysRegDataRegion.SystemStatusEnum.Unknown);

                return isInitialized;
            }

            internal set
            {
                if (true == value)
                {
                    if (!IsAppInitialized)
                    {
                        SysRegion.SystemStatus = SysRegDataRegion.SystemStatusEnum.Initialized;
                    }
                }
                else
                {
                    SysRegion.SystemStatus = SysRegDataRegion.SystemStatusEnum.UnInitialized;
                }
            }
        }

        #endregion

        #region Constructors

        internal RegDataStore()
        {
        }

        #endregion

        #region Public Methods

        public void PersistSysRegion()
        {
            this.PersistRegion(SysRegistry.REG_SYS_CODE);
        }

        public override XDataRegion GetRegion(string name)
        {
            XDataRegion region = null;

            if (!this.Regions.ContainsKey(name))
            {
                SysRegistry reg = null;

                using (var repo = AppDataAccessor.GetRepository<IRegRepository>())
                {
                    reg = repo.FindByCode(name);
                }

                if (!String.IsNullOrEmpty(reg.AssemblyType))
                {
                    if (name == SysRegistry.REG_SYS_CODE)
                    {
                        region = new SysRegDataRegion(reg);
                    }
                    else
                    {
                        region = CLRHelper.CreateInstance<RegDataRegion>(reg.AssemblyType, reg);
                    }
                }
                else
                {
                    region = CLRHelper.CreateInstance<RegDataRegion>(reg);
                }

                if (region != null)
                {
                    this.Regions.Set(name, region);
                }
            }

            region = this.Regions.Get(name);

            return region;
        }

        public void PersistRegion(string name)
        {
            var region = this.GetRegion(name) as RegDataRegion;

            if (region != null)
            {
                using (var repo = AppDataAccessor.GetRepository<IRegRepository>())
                {
                    var regRecord = region.Persist();

                    repo.Update(regRecord);
                }
            }
        }

        internal void Reset()
        {
            this.Regions.Clear();
        }

        #endregion

        #region Support Methods

        private SysRegDataRegion GetSysRegion()
        {
            if (sysRegion == null)
            {
                sysRegion = this.GetRegion(SysRegistry.REG_SYS_CODE) as SysRegDataRegion;
            }

            if (sysRegion == null)
            {
                using (var repo = AppDataAccessor.GetRepository<IRegRepository>())
                {
                    var rec = repo.GetSysRegistry();

                    sysRegion = new SysRegDataRegion(rec);

                    this.Regions.Set(sysRegion.Name, sysRegion);
                }
            }

            return sysRegion;
        }

        #endregion
    }
}
