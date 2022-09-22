using Microsoft.AspNetCore.Mvc;
using PSAPI.Controllers;

namespace PSAPI.Test
{
    public class ProcessDefinitionReadTests : DatabaseTestBase
    {
        public ProcessDefinitionReadTests() : base()
        {
        }

        [Fact]
        public async Task TestGetProcessDefinitionPage()
        {
            using (var context = CreateContext())
            {
                var psController = new PSController(_logger, _mapper, _config, context);
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
        public async Task TestGetNonExistingProcessDefinitionPage()
        {
            using (var context = CreateContext())
            {
                var psController = new PSController(_logger, _mapper, _config, context);
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
                var psController = new PSController(_logger, _mapper, _config, context);
                var result = await psController.GetProcessDefinition(300);
                var notFound = result.Result as NotFoundResult;
                notFound.StatusCode.ShouldBe(404);
            }
        }

        [Fact]
        public async Task TestPageZero()
        {
            using (var context = CreateContext())
            {
                var psController = new PSController(_logger, _mapper, _config, context);
                var result = await psController.GetProcessDefinitions(0);
                var notFound = result.Result as NotFoundResult;
                notFound.StatusCode.ShouldBe(404);
            }
        }

        [Fact]
        public async Task TestIncompletePage()
        {
            using (var context = CreateContext())
            {
                var psController = new PSController(_logger, _mapper, _config, context);
                var definitions = await psController.GetProcessDefinitions(3);
                definitions.ShouldNotBeNull();
                var result = definitions.Value;
                result.Count().ShouldBe(2);
                result[0].ProcessTaskDefinitions.Count.ShouldBe(2);
            }
        }
    }
}