using PSDTO;

namespace PSAPI.Test
{
    public class ProcessWriteTests : DatabaseTestBase
    {
        public ProcessWriteTests() : base()
        {
        }

        [Fact]
        async Task TestStartProcess()
        {
            using (var context = CreateContext())
            {
                var processName = "Process name 1";
                var psController = CreateController(context);
                var start = new StartProcessDTO(processName);
                var actionResult = await psController.StartProcess(start);
                var startObject = actionResult.GetObjectResult();
                var process = await psController.GetProcess(startObject.Id);
                var processObject = process.GetObjectResult();
                processObject.Id.ShouldBe(7);
                processObject.Name.ShouldBe(processName);
                processObject.IsReplayable.ShouldBeTrue();
                processObject.ProcessTasks.Count().ShouldBe(2);
                var i = 1;
                foreach (var task in processObject.ProcessTasks)
                {
                    task.Name.ShouldBe($"Process task definition name {i}");
                    task.Key.ShouldBe($"process_task_{i}");
                    i++;
                }
            }
        }
    }
}