using AutoMapper;
using Model;
using PSDTO;

namespace PSAPI.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProcessDefinition, ProcessDefinitionDTO>().ConvertUsing<ProcessDefinitionTypeConverter>();
            CreateMap<ProcessTaskDefinition, ProcessTaskDefinitionDTO>();
            CreateMap<ProcessDefinitionCreateDTO, ProcessDefinition>(MemberList.Source);
            CreateMap<ProcessTaskDefinitionCreateDTO, ProcessTaskDefinition>(MemberList.Source);
            CreateMap<ProcessDefinitionUpdateDTO, ProcessDefinition>(MemberList.Source);
            CreateMap<ProcessTaskDefinitionUpdateDTO, ProcessTaskDefinition>(MemberList.Source);
            CreateMap<ProcessDefinition, Process>().ConvertUsing<ProcessDefinitionProcessTypeConverter>();
            CreateMap<Process, ProcessListDTO>();
            CreateMap<Process, ProcessDTO>().ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name)); ;
            CreateMap<ProcessTask, ProcessTaskDTO>().ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));
            CreateMap<ProcessTaskDefinition, ProcessTask>().ForMember(dest => dest.Order, p => p.Ignore()).ForMember(dest => dest.Status, p => p.Ignore());
        }
    }
}
