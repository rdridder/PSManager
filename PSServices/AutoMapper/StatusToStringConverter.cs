using AutoMapper;
using Model;

namespace PSServices.AutoMapper
{
    public class StatusToStringConverter : ITypeConverter<Status, string>
    {
        public string Convert(Status source, string destination, ResolutionContext context)
        {
            return source.Name;
        }
    }
}
