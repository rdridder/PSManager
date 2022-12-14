using Microsoft.AspNetCore.Mvc;

namespace PSAPI.Test
{
    public class ProcessDefinitionReadTests : DatabaseTestBase
    {
        public ProcessDefinitionReadTests() : base()
        {
        }

        [Fact]
        public async Task TestGetProcessDefinitions()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                for (int k = 0; k < 2; k++)
                {
                    var page = k * 5;
                    var definitions = await psController.GetProcessDefinitions(k + 1);
                    definitions.ShouldNotBeNull();
                    var result = definitions.Value;
                    result.Count.ShouldBe(5);

                    for (int i = 0; i < 5; i++)
                    {
                        result[i].Id.ShouldBe(i + page + 1);
                        result[i].Name.ShouldBe($"Process name {i + page + 1}");
                        result[i].Description.ShouldBe($"Process description {i + page + 1}");
                        result[i].IsEnabled.ShouldBeTrue();
                        result[i].IsReplayable.ShouldBeTrue();
                        result[i].ProcessTaskDefinitions.Count().ShouldBe(2);
                        for (int j = 0; j < 2; j++)
                        {
                            result[i].ProcessTaskDefinitions[j].Id.ShouldBe(j + 1);
                            result[i].ProcessTaskDefinitions[j].Name.ShouldBe($"Process task definition name {j + 1}");
                            result[i].ProcessTaskDefinitions[j].Description.ShouldBe($"Process task definition description {j + 1}");
                            result[i].ProcessTaskDefinitions[j].IsEnabled.ShouldBeTrue();
                            result[i].ProcessTaskDefinitions[j].Key.ShouldBe($"process_task_{j + 1}");
                        }
                    }
                }
            }
        }

        [Fact]
        public async Task TestGetProcessDefinition()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);

                var definitions = await psController.GetProcessDefinition(1);
                definitions.ShouldNotBeNull();
                var result = definitions.Value;

                for (int j = 0; j < 2; j++)
                {
                    result.ProcessTaskDefinitions[j].Id.ShouldBe(j + 1);
                    result.ProcessTaskDefinitions[j].Name.ShouldBe($"Process task definition name {j + 1}");
                    result.ProcessTaskDefinitions[j].Description.ShouldBe($"Process task definition description {j + 1}");
                    result.ProcessTaskDefinitions[j].IsEnabled.ShouldBeTrue();
                    result.ProcessTaskDefinitions[j].Key.ShouldBe($"process_task_{j + 1}");
                }
            }
        }

        [Fact]
        public async Task TestGetNonExistingProcessDefinitionPage()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var result = await psController.GetProcessDefinitions(4);
                var notFound = result.Result as NotFoundResult;
                notFound.StatusCode.ShouldBe(404);
            }
        }

        [Fact]
        public async Task TestGetNonExistingProcessDefinitionId()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var result = await psController.GetProcessDefinition(300);
                var notFound = result.Result as NotFoundResult;
                notFound.StatusCode.ShouldBe(404);
            }
        }

        [Fact]
        public async Task TestPageZeroProcessDefinitions()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var result = await psController.GetProcessDefinitions(0);
                var notFound = result.Result as NotFoundResult;
                notFound.StatusCode.ShouldBe(404);
            }
        }

        [Fact]
        public async Task TestIncompletePageProcessDefinitions()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var definitions = await psController.GetProcessDefinitions(3);
                definitions.ShouldNotBeNull();
                var result = definitions.Value;
                result.Count().ShouldBe(2);
                result[0].ProcessTaskDefinitions.Count.ShouldBe(2);
            }
        }
    }
}