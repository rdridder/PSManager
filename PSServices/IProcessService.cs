using PSDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSServices
{
    public interface IProcessService
    {
        public Task<ProcessDefinitionDTO> GetProcessDefinition(long id);

        public Task<List<ProcessDefinitionDTO>> GetProcessDefinitions(int page = 1);

        public Task<ProcessTaskDefinitionDTO> GetProcessTaskDefinition(long id);

        public Task<List<ProcessTaskDefinitionDTO>> GetProcessTaskDefinitions(int page = 1);

        public Task<CreatedIdDTO> AddProcessDefinition(ProcessDefinitionCreateDTO processDefinitionCreateDTO);

        public Task<CreatedIdDTO> AddProcessTaskDefinition(ProcessTaskDefinitionCreateDTO processTaskDefinitionCreateDTO);

        public Task AddTaskToProcessDefinition(AddTaskToProcessDefinition addTaskToProcessDefinition);

        public Task UpdateProcessDefinition(ProcessDefinitionUpdateDTO processDefinitionUpdateDTO);

        public Task UpdateProcessTaskDefinition(ProcessTaskDefinitionUpdateDTO processTaskDefinitionUpdateDTO);

        public Task RemoveTaskFromProcessDefinition(RemoveTaskFromProcessDefinition removeTaskFromProcessDefinition);

        public Task RemoveTaskDefinition(RemoveById removeById);

        public Task RemoveProcessDefinition(RemoveById removeById);
    }
}
