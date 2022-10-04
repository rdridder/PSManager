using AutoMapper;
using Model;
using PSDTO;
using System.Collections.Generic;
using System.Linq;

namespace PSAPI.AutoMapper
{
    public class ProcessDefinitionTypeConverter : ITypeConverter<ProcessDefinition, ProcessDefinitionDTO>
    {
        public ProcessDefinitionDTO Convert(ProcessDefinition source, ProcessDefinitionDTO destination, ResolutionContext context)
        {
            var tasks = new List<ProcessTaskDefinitionDTO>();
            if (source.ProcessDefinitionTaskDefinitions != null)
            {
                // TODO, check if we can handle this ordering in the DB
                foreach (var processDefinitionTaskDefinitions in source.ProcessDefinitionTaskDefinitions.OrderBy(x => x.Order))
                {
                    tasks.Add(context.Mapper.Map<ProcessTaskDefinitionDTO>(processDefinitionTaskDefinitions.ProcessTaskDefinition));
                }
            }
            destination = new ProcessDefinitionDTO(source.Id, source.Name, source.Description, source.IsEnabled, source.IsReplayable, tasks);
            return destination;
        }
    }
}
