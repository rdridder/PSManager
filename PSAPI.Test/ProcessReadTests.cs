using Microsoft.AspNetCore.Mvc;

namespace PSAPI.Test
{
    public class ProcessReadTests : DatabaseTestBase
    {
        public ProcessReadTests() : base()
        {
        }

        [Fact]
        async Task TestGetProcessList()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var listResult = await psController.GetProcessList(1);
                var list = listResult.GetObjectResult();
                list.Count().ShouldBe(5);
                for (var i = 1; i < 6; i++)
                {
                    list[i - 1].Id.ShouldBe(i);
                    list[i - 1].Name.ShouldBe($"Process name {i}");
                    list[i - 1].IsReplayable.ShouldBeTrue();
                }
            }
        }

        [Fact]
        public async Task TestGetProcess()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);

                var definitions = await psController.GetProcess(1);
                definitions.ShouldNotBeNull();
                var result = definitions.Value;

                for (int j = 0; j < 2; j++)
                {
                    result.ProcessTasks[j].Id.ShouldBe(j + 1);
                    result.ProcessTasks[j].Name.ShouldBe($"Process task definition name {j + 1}");
                    result.ProcessTasks[j].Key.ShouldBe($"process_task_{j + 1}");
                }
            }
        }

        [Fact]
        public async Task TestGetNonExistingProcess()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var result = await psController.GetProcess(300);
                var notFound = result.Result as NotFoundResult;
                notFound.StatusCode.ShouldBe(404);
            }
        }

        [Fact]
        public async Task TestPageZeroProcessList()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var result = await psController.GetProcessList(0);
                var notFound = result.Result as NotFoundResult;
                notFound.StatusCode.ShouldBe(404);
            }
        }

        [Fact]
        public async Task TestIncompletePageProcessList()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var definitions = await psController.GetProcessList(2);
                definitions.ShouldNotBeNull();
                var result = definitions.Value;
                result.Count().ShouldBe(1);
            }
        }

        [Fact]
        async Task TestGetProcesses()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var listResult = await psController.GetProcesses(1);
                var list = listResult.GetObjectResult();
                list.Count().ShouldBe(5);
                for (var i = 1; i < 6; i++)
                {
                    list[i - 1].Id.ShouldBe(i);
                    list[i - 1].Name.ShouldBe($"Process name {i}");
                    list[i - 1].IsReplayable.ShouldBeTrue();
                    for (var j = 1; i < 3; i++)
                    {
                        list[i - 1].ProcessTasks[j - 1].Name.ShouldBe($"Process task definition name {j}");
                        list[i - 1].ProcessTasks[j - 1].Key.ShouldBe($"process_task_{j}");
                    }
                }
            }
        }

        [Fact]
        public async Task TestPageZeroProcesses()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var result = await psController.GetProcesses(0);
                var notFound = result.Result as NotFoundResult;
                notFound.StatusCode.ShouldBe(404);
            }
        }

        [Fact]
        public async Task TestIncompletePageProcesses()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var definitions = await psController.GetProcesses(2);
                definitions.ShouldNotBeNull();
                var result = definitions.Value;
                result.Count().ShouldBe(1);
            }
        }

        [Fact]
        public async Task TestNonExistingPageProcesses()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var result = await psController.GetProcesses(3);
                var notFound = result.Result as NotFoundResult;
                notFound.StatusCode.ShouldBe(404);
            }
        }

        [Fact]
        public async Task TestNonExistingPagePageProcessList()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var result = await psController.GetProcessList(3);
                var notFound = result.Result as NotFoundResult;
                notFound.StatusCode.ShouldBe(404);
            }
        }
    }
}