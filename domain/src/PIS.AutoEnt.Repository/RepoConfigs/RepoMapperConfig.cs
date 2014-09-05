using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.Repository
{
    public class RepoMapperConfig
    {
        public static void Register()
        {
            Mapper.CreateMap<SysModule, SysAuth>()
                .ForMember(d => d.Id, (opt) => opt.Ignore())
                .ForMember(d => d.Code, (opt) => opt.MapFrom(s => s.GetAuthCode()))
                .ForMember(d => d.AuthType, (opt) => opt.UseValue(SysAuth.TypeEnum.Module))
                .ForMember(d => d.AuthObjId, (opt) => opt.MapFrom(s => s.Id.ToString()));
        }
    }
}
