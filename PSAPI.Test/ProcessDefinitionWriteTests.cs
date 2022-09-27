using Microsoft.AspNetCore.Mvc;
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
                var psController = CreateController(context);
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
                var psController = CreateController(context);
                var task = new ProcessTaskDefinitionCreateDTO("myTask1", "myTaskDesc1", "myTaskKey1", true);
                var result = await psController.AddProcessTaskDefinition(task);
                var objectResult = result.GetObjectResult();
                objectResult.Id.ShouldBe(3);

                var taskResult = await psController.GetProcessTaskDefinition(3);
                var taskObject = taskResult.GetObjectResult();
                taskObject.Id.ShouldBe(3);
                taskObject.Name.ShouldBe("myTask1");
                taskObject.Description.ShouldBe("myTaskDesc1");
                taskObject.Key.ShouldBe("myTaskKey1");
                taskObject.IsEnabled.ShouldBeTrue();
            }
        }

        [Fact]
        async Task TestAddTaskDefinitionToProcess()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var add = new AddTaskToProcessDefinition(12, new List<long> { 1 });
                var actionResult = await psController.AddTaskToProcessDefinition(add);
                actionResult.ShouldBeOfType<OkResult>();
                var definition = await psController.GetProcessDefinition(12);
                var def = definition.GetObjectResult();
                def.ProcessTaskDefinitions.Count.ShouldBe(1);
                def.ProcessTaskDefinitions[0].Name.ShouldBe("Process task definition name 1");
            }
        }

        [Fact]
        async Task TestStartProcess()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var start = new StartProcessDTO("Process name 1");
                var actionResult = await psController.StartProcess(start);
                var taskObject = actionResult.GetObjectResult();
                taskObject.Id.ShouldBe(1);
            }
        }
    }
}