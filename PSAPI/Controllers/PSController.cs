using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using PSData.Context;
using PSDTO;

namespace PSAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class PSController : ControllerBase
    {

        private readonly ProcessContext _processContext;

        private readonly IMapper _mapper;

        private readonly int _limit = 0;

        private readonly ILogger<PSController> _logger;

        public PSController(ILogger<PSController> logger, IMapper mapper, IConfiguration configuration, ProcessContext processContext)
        {
            _logger = logger;
            _processContext = processContext;
            _mapper = mapper;
            _limit = int.Parse(configuration["PageSize"]);
        }

        [HttpGet("GetProcessDefinition")]
        public async Task<ActionResult<ProcessDefinitionDTO>> GetProcessDefinition(long id)
        {
            var result = await _processContext.ProcessDefinitions
                .Where(x => x.Id == id)
                .Include(x => x.ProcessDefinitionTaskDefinitions)
                .ThenInclude(x => x.ProcessTaskDefinition).FirstOrDefaultAsync();
            if (result == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProcessDefinitionDTO>(result);
        }

        [HttpGet("GetProcessDefinitions")]
        public async Task<ActionResult<List<ProcessDefinitionDTO>>> GetProcessDefinitions(int page = 1)
        {
            int skip = (page - 1) * _limit;
            var result = await _processContext.ProcessDefinitions
                .Skip(skip)
                .Take(_limit)
                .Include(x => x.ProcessDefinitionTaskDefinitions)
                .ThenInclude(x => x.ProcessTaskDefinition)
                .ToListAsync();
            var dto = _mapper.Map<List<ProcessDefinitionDTO>>(result);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return dto;
        }

        [HttpGet("GetProcessTaskDefinition")]
        public async Task<ActionResult<ProcessTaskDefinitionDTO>> GetProcessTaskDefinition(long id)
        {
            var result = await _processContext.ProcessTaskDefinition
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            if (result == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProcessTaskDefinitionDTO>(result);
        }

        [HttpGet("GetProcessTaskDefinitions")]
        public async Task<ActionResult<List<ProcessTaskDefinitionDTO>>> GetProcessTaskDefinitions(int page = 1)
        {
            int skip = (page - 1) * _limit;
            var result = await _processContext.ProcessTaskDefinition
                .Skip(skip)
                .Take(_limit)
                .ToListAsync();
            var dto = _mapper.Map<List<ProcessTaskDefinitionDTO>>(result);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return dto;
        }

        [HttpPost("AddProcessDefinition")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreatedIdDTO))]
        public async Task<ActionResult<CreatedIdDTO>> AddProcessDefinition(ProcessDefinitionCreateDTO processDefinitionCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var processDefinition = _mapper.Map<ProcessDefinition>(processDefinitionCreateDTO);
                await _processContext.AddAsync(processDefinition);
                await _processContext.SaveChangesAsync();
                return Ok(new CreatedIdDTO(processDefinition.Id));
            }
            return BadRequest();
        }

        [HttpPost("AddProcessTaskDefinition")]
        public async Task<IActionResult> AddProcessTaskDefinition(ProcessTaskDefinitionCreateDTO processTaskDefinitionCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var processTaskDefinition = _mapper.Map<ProcessTaskDefinition>(processTaskDefinitionCreateDTO);
                await _processContext.AddAsync(processTaskDefinition);
                await _processContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("AddTaskToProcessDefinition")]
        public async Task<IActionResult> AddTaskToProcessDefinition(AddTaskToProcessDefinition addTaskToProcessDefinition)
        {
            if (ModelState.IsValid)
            {
                var processDefinition = await _processContext.ProcessDefinitions
                                            .Include(x => x.ProcessDefinitionTaskDefinitions)
                                            .ThenInclude(x => x.ProcessTaskDefinition)
                                            .SingleOrDefaultAsync(x => x.Id == addTaskToProcessDefinition.ProcessId);

                if (processDefinition == null)
                {
                    return BadRequest();
                }

                var postedProcessTaskDefinition = await _processContext.ProcessTaskDefinition.Where(x => addTaskToProcessDefinition.TaskIds.Contains(x.Id)).ToListAsync();
                if (!postedProcessTaskDefinition.Any())
                {
                    return BadRequest();
                }

                // Filter out the tasks already assigned to the process definition
                var postedTaskIds = postedProcessTaskDefinition.Select(x => x.Id).ToList();
                var assignedTaskIds = processDefinition.ProcessDefinitionTaskDefinitions.Select(x => x.ProcessTaskDefinitionId).ToList();

                // Remove the already assigned tasks from the posted process task definitions
                postedProcessTaskDefinition = postedProcessTaskDefinition.Where(x => !assignedTaskIds.Contains(x.Id)).ToList();


                if (postedProcessTaskDefinition.Any())
                {
                    foreach (var definition in postedProcessTaskDefinition)
                    {
                        processDefinition.ProcessDefinitionTaskDefinitions.Add(new ProcessDefinitionTaskDefinition
                        {
                            ProcessDefinitionId = processDefinition.Id,
                            ProcessTaskDefinitionId = definition.Id
                        });
                    }
                    _processContext.Update(processDefinition);
                    await _processContext.SaveChangesAsync();
                }
                return Ok();
            }
            return BadRequest();
        }

        [HttpPatch("UpdateProcessDefinition")]
        public async Task<IActionResult> UpdateProcessDefinition(ProcessDefinitionUpdateDTO processDefinitionUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                var processDefinition = _mapper.Map<ProcessDefinition>(processDefinitionUpdateDTO);
                _processContext.Update(processDefinition);
                await _processContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpPatch("UpdateProcessTaskDefinition")]
        public async Task<IActionResult> UpdateProcessTaskDefinition(ProcessTaskDefinitionUpdateDTO processTaskDefinitionUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                var processTaskDefinition = _mapper.Map<ProcessTaskDefinition>(processTaskDefinitionUpdateDTO);
                _processContext.Update(processTaskDefinition);
                await _processContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("RemoveTaskFromProcessDefinition")]
        public async Task<IActionResult> RemoveTaskFromProcessDefinition(RemoveTaskFromProcessDefinition removeTaskFromProcessDefinition)
        {
            if (ModelState.IsValid)
            {
                var processDefinition = await _processContext.ProcessDefinitions
                                            .Include(x => x.ProcessDefinitionTaskDefinitions)
                                            .ThenInclude(x => x.ProcessTaskDefinition)
                                            .SingleOrDefaultAsync(x => x.Id == removeTaskFromProcessDefinition.ProcessId);

                if (processDefinition == null)
                {
                    return BadRequest();
                }

                var postedProcessTaskDefinition = await _processContext.ProcessTaskDefinition
                    .Where(x => removeTaskFromProcessDefinition.TaskIds.Contains(x.Id)).ToListAsync();
                if (!postedProcessTaskDefinition.Any())
                {
                    return BadRequest();
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
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("RemoveTaskDefinition")]
        public async Task<IActionResult> RemoveTaskDefinition(RemoveById removeById)
        {
            if (ModelState.IsValid)
            {
                var taskDefinitionList = removeById.Ids.Select(x => new ProcessTaskDefinition() { Id = x });
                _processContext.RemoveRange(taskDefinitionList);
                await _processContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("RemoveProcessDefinition")]
        public async Task<IActionResult> RemoveProcessDefinition(RemoveById removeById)
        {
            if (ModelState.IsValid)
            {
                var processDefinitionList = removeById.Ids.Select(x => new ProcessDefinition() { Id = x });
                _processContext.RemoveRange(processDefinitionList);
                await _processContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }


    }
}