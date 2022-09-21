using Microsoft.AspNetCore.Mvc;
using PSAPI.Controllers;
using PSDTO;

namespace PSAPI.Test
{
    public class ProcessDefinitionWriteTests : DatabaseTestBase
    {
        public ProcessDefinitionWriteTests() : base()
        {
        }

        [Fact]
        async Task TestCreateProcessDefinition()
        {
            using (var context = CreateContext())
            {
                var psController = new PSController(_logger, _mapper, _config, context);
                var processDefinitionCreateDTO = new ProcessDefinitionCreateDTO("my name", "my description", false, false);
                var result = await psController.AddProcessDefinition(processDefinitionCreateDTO);
                result.GetObjectResult().Id.ShouldBe(13);
            }
        }

        [Fact]
        async Task TestCreateTaskDefinitions()
        {
            using (var context = CreateContext())
            {
                var psController = new PSController(_logger, _mapper, _config, context);
                var task = new ProcessTaskDefinitionCreateDTO("myTask1", "myTaskDesc1", "myTaskKey1", true);
                var result = await psController.AddProcessTaskDefinition(task);
                result.GetObjectResult().Id.ShouldBe(3);
            }
        }

        [Fact]
        async Task TestAddTaskDefinitions()
        {
            using (var context = CreateContext())
            {
                var psController = new PSController(_logger, _mapper, _config, context);
                var add = new AddTaskToProcessDefinition(12, new List<long> { 1 });
                var actionResult = await psController.AddTaskToProcessDefinition(add);
                actionResult.ShouldBeOfType<OkResult>();
                var definition = await psController.GetProcessDefinition(12);
                var def = definition.GetObjectResult();
                def.ProcessTaskDefinitions[0].Name.ShouldBe("Process task definition name 1");
            }
        }
    }
}