using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PSDTO;
using PSServices;

namespace PSAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class PSController : ControllerBase
    {
        private readonly IProcessService _processService;

        private readonly IMapper _mapper;

        private readonly int _limit = 0;

        private readonly ILogger<PSController> _logger;

        public PSController(ILogger<PSController> logger, IMapper mapper,
                                IConfiguration configuration, IProcessService processService)
        {
            _logger = logger;
            _mapper = mapper;
            _limit = int.Parse(configuration["PageSize"]);
            _processService = processService;
        }

        [HttpGet("GetProcessDefinition")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProcessDefinitionDTO))]
        public async Task<ActionResult<ProcessDefinitionDTO>> GetProcessDefinition(long id)
        {
            var result = await _processService.GetProcessDefinition(id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpGet("GetProcess")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProcessDTO))]
        public async Task<ActionResult<ProcessDTO>> GetProcess(long id)
        {
            var result = await _processService.GetProcess(id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpGet("GetProcessDefinitions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProcessDefinitionDTO>))]
        public async Task<ActionResult<List<ProcessDefinitionDTO>>> GetProcessDefinitions(int page = 1)
        {
            if (page == 0)
            {
                return NotFound();
            }
            var result = await _processService.GetProcessDefinitions(page);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return result;
        }

        [HttpGet("GetProcesses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProcessDTO>))]
        public async Task<ActionResult<List<ProcessDTO>>> GetProcesses(int page = 1)
        {
            if (page == 0)
            {
                return NotFound();
            }
            var result = await _processService.GetProcesses(page);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return result;
        }

        [HttpGet("GetProcessList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProcessListDTO>))]
        public async Task<ActionResult<List<ProcessListDTO>>> GetProcessList(int page = 1)
        {
            if (page == 0)
            {
                return NotFound();
            }
            var result = await _processService.GetProcessList(page);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return result;
        }

        [HttpGet("GetProcessTaskDefinition")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProcessTaskDefinitionDTO>))]
        public async Task<ActionResult<ProcessTaskDefinitionDTO>> GetProcessTaskDefinition(long id)
        {
            var result = await _processService.GetProcessTaskDefinition(id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpGet("GetProcessTaskDefinitions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProcessTaskDefinitionDTO>))]
        public async Task<ActionResult<List<ProcessTaskDefinitionDTO>>> GetProcessTaskDefinitions(int page = 1)
        {
            var result = await _processService.GetProcessTaskDefinitions(page);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return result;
        }

        [HttpPost("AddProcessDefinition")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreatedIdDTO))]
        public async Task<ActionResult<CreatedIdDTO>> AddProcessDefinition(ProcessDefinitionCreateDTO processDefinitionCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _processService.AddProcessDefinition(processDefinitionCreateDTO);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost("AddProcessTaskDefinition")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreatedIdDTO))]
        public async Task<ActionResult<CreatedIdDTO>> AddProcessTaskDefinition(ProcessTaskDefinitionCreateDTO processTaskDefinitionCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _processService.AddProcessTaskDefinition(processTaskDefinitionCreateDTO);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost("AddTaskToProcessDefinition")]
        public async Task<IActionResult> AddTaskToProcessDefinition(AddTaskToProcessDefinition addTaskToProcessDefinition)
        {
            if (ModelState.IsValid)
            {
                // TODO fix exception handling
                try
                {
                    await _processService.AddTaskToProcessDefinition(addTaskToProcessDefinition);
                }
                catch (Exception e)
                {
                    return BadRequest();
                }
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("StartProcess")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreatedIdDTO))]
        public async Task<ActionResult<CreatedIdDTO>> StartProcess(StartProcessDTO startProcess)
        {
            if (ModelState.IsValid)
            {
                // TODO fix exception handling
                try
                {
                    return await _processService.StartProcess(startProcess);
                }
                catch (Exception e)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpPatch("UpdateProcessDefinition")]
        public async Task<IActionResult> UpdateProcessDefinition(ProcessDefinitionUpdateDTO processDefinitionUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                await _processService.UpdateProcessDefinition(processDefinitionUpdateDTO);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPatch("UpdateProcessTaskDefinition")]
        public async Task<IActionResult> UpdateProcessTaskDefinition(ProcessTaskDefinitionUpdateDTO processTaskDefinitionUpdateDTO)
        {
            if (ModelState.IsValid)
            {
                await _processService.UpdateProcessTaskDefinition(processTaskDefinitionUpdateDTO);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("RemoveTaskFromProcessDefinition")]
        public async Task<IActionResult> RemoveTaskFromProcessDefinition(RemoveTaskFromProcessDefinition removeTaskFromProcessDefinition)
        {
            if (ModelState.IsValid)
            {
                // TODO fix exception handling
                try
                {
                    await _processService.RemoveTaskFromProcessDefinition(removeTaskFromProcessDefinition);
                }
                catch (Exception e)
                {
                    BadRequest();
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
                await _processService.RemoveTaskDefinition(removeById);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("RemoveProcessDefinition")]
        public async Task<IActionResult> RemoveProcessDefinition(RemoveById removeById)
        {
            if (ModelState.IsValid)
            {
                await _processService.RemoveProcessDefinition(removeById);
                return Ok();
            }
            return BadRequest();
        }
    }
}