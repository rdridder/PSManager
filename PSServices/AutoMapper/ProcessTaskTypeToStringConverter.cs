using AutoMapper;
using Model;

namespace PSServices.AutoMapper
{
    public class ProcessTaskTypeToStringConverter : ITypeConverter<ProcessTaskType, string>
    {
        public string Convert(ProcessTaskType source, string destination, ResolutionContext context)
        {
            return source.Name;
        }
    }
}
