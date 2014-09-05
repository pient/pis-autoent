using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIS.AutoEnt.App
{
    public static class ModuleInitializer
    {
        public static bool Initialize()
        {
            if (Sys.AppInitializer.Initialize())
            {
                AppMapperConfig.Register();

                return true;
            }

            return false;
        }
    }
}
