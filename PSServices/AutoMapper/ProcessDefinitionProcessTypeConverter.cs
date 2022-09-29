using AutoMapper;
using Model;
using System.Collections.Generic;

namespace PSAPI.AutoMapper
{
    public class ProcessDefinitionProcessTypeConverter : ITypeConverter<ProcessDefinition, Process>
    {
        public Process Convert(ProcessDefinition source, Process destination, ResolutionContext context)
        {
            var tasks = new List<ProcessTask>();
            if (source.ProcessDefinitionTaskDefinitions != null)
            {
                foreach (var processDefinitionTaskDefinitions in source.ProcessDefinitionTaskDefinitions)
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
