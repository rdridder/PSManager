using Microsoft.AspNetCore.Mvc;
using PSAPI.Controllers;
using PSDTO;
using Shouldly;

namespace PSAPI.Test
{
    public class ProcessDefinitionTests : DatabaseTestBase
    {
        public ProcessDefinitionTests() : base()
        {
        }

        [Fact]
        public async Task TestGetProcessDefinition()
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
        public async Task TestGetNonExistingPage()
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
        async Task TestCreateProcessDefinition()
        {
            using (var context = CreateContext())
            {
                var psController = new PSController(_logger, _mapper, _config, context);
                var processDefinitionCreateDTO = new ProcessDefinitionCreateDTO("my name", "my description", false, false);
                var result = await psController.AddProcessDefinition(processDefinitionCreateDTO);
                result.GetObjectResult().Id.ShouldBe(12);
            }
        }
    }
}