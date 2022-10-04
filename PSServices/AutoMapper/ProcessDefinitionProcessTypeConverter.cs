using AutoMapper;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace PSAPI.AutoMapper
{
    public class ProcessDefinitionProcessTypeConverter : ITypeConverter<ProcessDefinition, Process>
    {
        public Process Convert(ProcessDefinition source, Process destination, ResolutionContext context)
        {
            var tasks = new List<ProcessTask>();
            if (source.ProcessDefinitionTaskDefinitions != null)
            {
                // TODO, check if we can handle this ordering in the DB
                foreach (var processDefinitionTaskDefinitions in source.ProcessDefinitionTaskDefinitions.OrderBy(x => x.Order))
                {
                    tasks.Add(new ProcessTask()
                    {
                        Key = processDefinitionTaskDefinitions.ProcessTaskDefinition.Key,
                        Name = processDefinitionTaskDefinitions.ProcessTaskDefinition.Name,
                        Order = processDefinitionTaskDefinitions.Order
                    });
                }
            }
            destination = new Process()
            {
                IsReplayable = source.IsReplayable,
                Name = source.Name,
                ProcessTasks = tasks
            };
            return destination;
        }
    }
}
