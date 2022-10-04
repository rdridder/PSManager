using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model;
using PSData.Context;
using PSDTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSServices
{
    public class ProcessService : IProcessService
    {
        private readonly ProcessContext _processContext;

        private readonly IMapper _mapper;

        private readonly int _limit;

        public ProcessService(ProcessContext processContext, IMapper mapper, int limit = 5)
        {
            _processContext = processContext;
            _mapper = mapper;
            _limit = limit;
        }

        public async Task<CreatedIdDTO> AddProcessDefinition(ProcessDefinitionCreateDTO processDefinitionCreateDTO)
        {
            var processDefinition = _mapper.Map<ProcessDefinition>(processDefinitionCreateDTO);
            await _processContext.AddAsync(processDefinition);
            await _processContext.SaveChangesAsync();
            return new CreatedIdDTO(processDefinition.Id);
        }

        public async Task<CreatedIdDTO> AddProcessTaskDefinition(ProcessTaskDefinitionCreateDTO processTaskDefinitionCreateDTO)
        {
            var processTaskDefinition = _mapper.Map<ProcessTaskDefinition>(processTaskDefinitionCreateDTO);
            await _processContext.AddAsync(processTaskDefinition);
            await _processContext.SaveChangesAsync();
            return new CreatedIdDTO(processTaskDefinition.Id);
        }

        public async Task AddTaskToProcessDefinition(AddTaskToProcessDefinitionDTO addTaskToProcessDefinition)
        {

            var processDefinition = await _processContext.ProcessDefinitions
                                        .Include(x => x.ProcessDefinitionTaskDefinitions)
                                        .ThenInclude(x => x.ProcessTaskDefinition)
                                        .SingleOrDefaultAsync(x => x.Id == addTaskToProcessDefinition.ProcessId);

            if (processDefinition == null)
            {
                // TODO fix
                throw new System.Exception("Process not found");
            }

            var postedTaskIds = addTaskToProcessDefinition.TaskIds.Select(x => x.Id).ToList();
            var postedProcessTaskDefinition = await _processContext.ProcessTaskDefinition.Where(x => postedTaskIds.Contains(x.Id)).ToListAsync();
            if (!postedProcessTaskDefinition.Any())
            {
                // TODO fix
                throw new System.Exception("No valid tasks posted");
            }

            // Filter out the tasks already assigned to the process definition
            var assignedTaskIds = processDefinition.ProcessDefinitionTaskDefinitions.Select(x => x.ProcessTaskDefinitionId).ToList();

            // Remove the already assigned tasks from the posted process task definitions
            postedProcessTaskDefinition = postedProcessTaskDefinition.Where(x => !assignedTaskIds.Contains(x.Id)).ToList();

            if (postedProcessTaskDefinition.Any())
            {
                foreach (var taskDefinition in postedProcessTaskDefinition)
                {
                    // TODO Linq statement is bit flaky, fix
                    processDefinition.ProcessDefinitionTaskDefinitions.Add(new ProcessDefinitionTaskDefinition
                    {
                        ProcessDefinitionId = processDefinition.Id,
                        ProcessTaskDefinitionId = taskDefinition.Id,
                        Order = addTaskToProcessDefinition.TaskIds.Where(x => x.Id == taskDefinition.Id).FirstOrDefault().Order
                    });
                }
                _processContext.Update(processDefinition);
                await _processContext.SaveChangesAsync();
            }
        }

        public async Task<ProcessDTO> GetProcess(long id)
        {
            var result = await _processContext.Processes
                .Where(x => x.Id == id)
                .Include(x => x.ProcessTasks)
                .FirstOrDefaultAsync();
            return _mapper.Map<ProcessDTO>(result);
        }

        public async Task<ProcessDefinitionDTO> GetProcessDefinition(long id)
        {
            var result = await _processContext.ProcessDefinitions
                .Where(x => x.Id == id)
                .Include(x => x.ProcessDefinitionTaskDefinitions)
                .ThenInclude(x => x.ProcessTaskDefinition)
                .FirstOrDefaultAsync();
            return _mapper.Map<ProcessDefinitionDTO>(result);
        }

        public async Task<List<ProcessDefinitionDTO>> GetProcessDefinitions(int page = 1)
        {
            int skip = (page - 1) * _limit;
            var result = await _processContext.ProcessDefinitions
                .Skip(skip)
                .Take(_limit)
                .Include(x => x.ProcessDefinitionTaskDefinitions)
                .ThenInclude(x => x.ProcessTaskDefinition)
                .ToListAsync();
            return _mapper.Map<List<ProcessDefinitionDTO>>(result);
        }

        public async Task<List<ProcessDTO>> GetProcesses(int page = 1)
        {
            int skip = (page - 1) * _limit;
            var result = await _processContext.Processes
                .Skip(skip)
                .Take(_limit)
                .Include(x => x.ProcessTasks)
                .ToListAsync();
            return _mapper.Map<List<ProcessDTO>>(result);
        }

        public async Task<List<ProcessListDTO>> GetProcessList(int page = 1)
        {
            int skip = (page - 1) * _limit;
            var result = await _processContext.Processes
                .Skip(skip)
                .Take(_limit)
                .ToListAsync();
            return _mapper.Map<List<ProcessListDTO>>(result);
        }

        public async Task<ProcessTaskDefinitionDTO> GetProcessTaskDefinition(long id)
        {
            var result = await _processContext.ProcessTaskDefinition
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ProcessTaskDefinitionDTO>(result);
        }

        public async Task<List<ProcessTaskDefinitionDTO>> GetProcessTaskDefinitions(int page = 1)
        {
            int skip = (page - 1) * _limit;
            var result = await _processContext.ProcessTaskDefinition
                .Skip(skip)
                .Take(_limit)
                .ToListAsync();
            return _mapper.Map<List<ProcessTaskDefinitionDTO>>(result);
        }

        public Task RemoveProcessDefinition(RemoveByIdDTO removeById)
        {
            var processDefinitionList = removeById.Ids.Select(x => new ProcessDefinition() { Id = x });
            _processContext.RemoveRange(processDefinitionList);
            return _processContext.SaveChangesAsync();
        }

        public Task RemoveTaskDefinition(RemoveByIdDTO removeById)
        {
            var taskDefinitionList = removeById.Ids.Select(x => new ProcessTaskDefinition() { Id = x });
            _processContext.RemoveRange(taskDefinitionList);
            return _processContext.SaveChangesAsync();
        }

        public async Task RemoveTaskFromProcessDefinition(RemoveTaskFromProcessDefinitionDTO removeTaskFromProcessDefinition)
        {
            var processDefinition = await _processContext.ProcessDefinitions
                                        .Include(x => x.ProcessDefinitionTaskDefinitions)
                                        .ThenInclude(x => x.ProcessTaskDefinition)
                                        .SingleOrDefaultAsync(x => x.Id == removeTaskFromProcessDefinition.ProcessId);

            if (processDefinition == null)
            {
                // TODO fix
                throw new System.Exception("Process not found");
            }

            var postedProcessTaskDefinition = await _processContext.ProcessTaskDefinition
                .Where(x => removeTaskFromProcessDefinition.TaskIds.Contains(x.Id)).ToListAsync();
            if (!postedProcessTaskDefinition.Any())
            {
                // TODO fix
                throw new System.Exception("No valid tasks posted");
            }

            // Remove the tasks from the process definition
            var postedTaskIds = postedProcessTaskDefinition.Select(x => x.Id).ToList();
            var taskCount = processDefinition.ProcessDefinitionTaskDefinitions.Count;
            processDefinition.ProcessDefinitionTaskDefinitions =
                processDefinition.ProcessDefinitionTaskDefinitions
                    .Where(x => !postedTaskIds.Contains(x.ProcessTaskDefinitionId)).ToList();

            if (processDefinition.ProcessDefinitionTaskDefinitions.Count != taskCount)
            {
                _processContext.Update(processDefinition);
                await _processContext.SaveChangesAsync();
            }
        }

        public async Task<CreatedIdDTO> StartProcess(StartProcessDTO startProcess)
        {
            var result = await _processContext.ProcessDefinitions
                .Where(x => x.Name == startProcess.Name)
                .Include(x => x.ProcessDefinitionTaskDefinitions)
                .ThenInclude(x => x.ProcessTaskDefinition).FirstOrDefaultAsync();
            var process = _mapper.Map<Process>(result);

            _processContext.Add(process);
            await _processContext.SaveChangesAsync();
            return new CreatedIdDTO(process.Id);
        }

        public Task UpdateProcessDefinition(ProcessDefinitionUpdateDTO processDefinitionUpdateDTO)
        {
            var processDefinition = _mapper.Map<ProcessDefinition>(processDefinitionUpdateDTO);
            _processContext.Update(processDefinition);
            return _processContext.SaveChangesAsync();
        }

        public Task UpdateProcessTaskDefinition(ProcessTaskDefinitionUpdateDTO processTaskDefinitionUpdateDTO)
        {
            var processTaskDefinition = _mapper.Map<ProcessTaskDefinition>(processTaskDefinitionUpdateDTO);
            _processContext.Update(processTaskDefinition);
            return _processContext.SaveChangesAsync();
        }
    }
}
