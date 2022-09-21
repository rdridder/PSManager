using AutoMapper;
using Model;
using PSDTO;

namespace PSAPI.AutoMapper
{
    public class ProcessDefinitionTypeConverter : ITypeConverter<ProcessDefinition, ProcessDefinitionDTO>
    {
        public ProcessDefinitionDTO Convert(ProcessDefinition source, ProcessDefinitionDTO destination, ResolutionContext context)
        {
            var tasks = new List<ProcessTaskDefinitionDTO>();
            if (source.ProcessDefinitionTaskDefinitions != null)
            {
                foreach (var processDefinitionTaskDefinitions in source.ProcessDefinitionTaskDefinitions)
                {
                    tasks.Add(context.Mapper.Map<ProcessTaskDefinitionDTO>(processDefinitionTaskDefinitions.ProcessTaskDefinition));
                }
            }
            destination = new ProcessDefinitionDTO(source.Id, source.Name, source.Description, source.IsEnabled, source.IsReplayable, tasks);
            return destination;
        }
    }
}
