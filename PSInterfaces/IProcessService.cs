using PSDTO;
using PSDTO.Process;
using PSDTO.ProcessDefinition;

namespace PSInterfaces
{
    public interface IProcessService
    {
        public Task<ProcessDefinitionDTO> GetProcessDefinition(long id);

        public Task<List<ProcessDefinitionDTO>> GetProcessDefinitions(int page = 1);

        public Task<ProcessDTO> GetProcess(long id);

        public Task<List<ProcessDTO>> GetProcesses(int page = 1);

        public Task<List<ProcessListDTO>> GetProcessList(int page = 1);

        public Task<ProcessTaskDefinitionDTO> GetProcessTaskDefinition(long id);

        public Task<List<ProcessTaskDefinitionDTO>> GetProcessTaskDefinitions(int page = 1);

        public Task<CreatedIdDTO> AddProcessDefinition(ProcessDefinitionCreateDTO processDefinitionCreateDTO);

        public Task<CreatedIdDTO> AddProcessTaskDefinition(ProcessTaskDefinitionCreateDTO processTaskDefinitionCreateDTO);

        public Task AddTaskToProcessDefinition(AddTaskToProcessDefinitionDTO addTaskToProcessDefinition);

        public Task UpdateProcessDefinition(ProcessDefinitionUpdateDTO processDefinitionUpdateDTO);

        public Task UpdateProcessTaskDefinition(ProcessTaskDefinitionUpdateDTO processTaskDefinitionUpdateDTO);

        public Task RemoveTaskFromProcessDefinition(RemoveTaskFromProcessDefinitionDTO removeTaskFromProcessDefinition);

        public Task RemoveTaskDefinition(RemoveByIdDTO removeById);

        public Task RemoveProcessDefinition(RemoveByIdDTO removeById);

        public Task<CreatedIdDTO> StartProcess(StartProcessDTO startProcess);

        public Task<ProcessStatusDTO> ContinueProcess(ContinueProcessDTO continueProcess);

        public Task<ProcessStatusDTO> FinishProcessTask(FinishProcessTaskDTO finishProcessTaskDTO);
    }
}
