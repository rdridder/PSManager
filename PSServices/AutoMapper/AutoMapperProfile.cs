using AutoMapper;
using Model;
using PSDTO;
using PSDTO.Process;
using PSDTO.ProcessDefinition;
using PSServices.AutoMapper;

namespace PSAPI.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProcessTaskType, string>().ConvertUsing<ProcessTaskTypeToStringConverter>();
            CreateMap<Status, string>().ConvertUsing<StatusToStringConverter>();

            CreateMap<ProcessDefinition, ProcessDefinitionDTO>().ConvertUsing<ProcessDefinitionTypeConverter>();

            CreateMap<ProcessTaskDefinition, ProcessTaskDefinitionDTO>();
            CreateMap<ProcessDefinitionCreateDTO, ProcessDefinition>(MemberList.Source);
            CreateMap<ProcessTaskDefinitionCreateDTO, ProcessTaskDefinition>(MemberList.Source)
                .ForMember(dest => dest.ProcessTaskType,
                           opt => opt.MapFrom((src, dst, dstItem, context) => (ProcessTaskType)context.Options.Items["ProcessTaskType"]));
            CreateMap<ProcessDefinitionUpdateDTO, ProcessDefinition>(MemberList.Source);
            CreateMap<ProcessTaskDefinitionUpdateDTO, ProcessTaskDefinition>(MemberList.Source)
                .ForMember(dest => dest.ProcessTaskType,
                           opt => opt.MapFrom((src, dst, dstItem, context) => (ProcessTaskType)context.Options.Items["ProcessTaskType"])); ;
            CreateMap<ProcessDefinition, Process>().ConvertUsing<ProcessDefinitionProcessTypeConverter>();
            CreateMap<Process, ProcessListDTO>();
            CreateMap<Process, ProcessDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));
            CreateMap<ProcessTask, ProcessTaskDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));
            CreateMap<ProcessTaskDefinition, ProcessTask>().ForMember(dest => dest.Order, p => p.Ignore()).ForMember(dest => dest.Status, p => p.Ignore());
        }
    }
}
