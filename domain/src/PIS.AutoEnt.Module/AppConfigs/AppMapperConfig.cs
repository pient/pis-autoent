using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt
{
    public class AppMapperConfig
    {
        public static void Register()
        {
            RegisterDALModels();

            RegisterDTOModels();
        }

        private static void RegisterDALModels()
        {
        }

        private static void RegisterDTOModels()
        {
            Mapper.CreateMap<DtoModels.UserAccount, OrgUser>();
        }
    }
}
