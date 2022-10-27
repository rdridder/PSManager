using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model;
using Moq;
using PSAPI.AutoMapper;
using PSAPI.Controllers;
using PSAPI.Test.Mock;
using PSData.Context;
using PSDTO.Enums;
using PSDTO.Process;
using PSServices;

namespace PSAPI.Test
{
    public abstract class DatabaseTestBase : IDisposable
    {
        protected readonly ILogger<PSController> _logger;

        protected readonly IMapper _mapper;

        protected readonly MockAZServiceBus _serviceBus;

        protected readonly MockAZMessageService _messageService;

        protected readonly IConfiguration _config;

        protected readonly SqliteConnection _connection;

        protected readonly DbContextOptions<ProcessContext> _contextOptions;

        public DatabaseTestBase()
        {
            _logger = new Mock<ILogger<PSController>>().Object;
            _mapper = new Mapper(GetMapperConfig());
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
            _serviceBus = new MockAZServiceBus();
            _messageService = new MockAZMessageService(_serviceBus);
            _config = new ConfigurationBuilder()
            .AddInMemoryCollection(GetConfig())
            .Build();

            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<ProcessContext>()
                .UseSqlite(_connection)
                .EnableSensitiveDataLogging()
                .Options;

            SetupDatabase().GetAwaiter().GetResult();
        }

        public ProcessContext CreateContext() => new ProcessContext(_contextOptions);

        public PSController CreateController(ProcessContext context)
        {
            var processService = new ProcessService(context, _mapper, _messageService);
            var psController = new PSController(_logger, _mapper, _config, processService);
            return psController;
        }

        private MapperConfiguration GetMapperConfig()
        {

            MapperConfiguration mapperConfig = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            return mapperConfig;
        }

        private Dictionary<string, string> GetConfig()
        {
            var config = new Dictionary<string, string>
            {
               {"PageSize", "5"},
            };
            return config;
        }

        private List<Status> GetStatuses()
        {
            var result = new List<Status>();
            foreach (var val in Enum.GetValues(typeof(StatusEnum)))
            {
                result.Add(new Status()
                {
                    Name = val.ToString()
                });
            }
            return result;
        }

        private List<ProcessTaskType> GetTaskTypes()
        {
            var result = new List<ProcessTaskType>();
            foreach (var val in Enum.GetValues(typeof(ProcessTaskTypeEnum)))
            {
                result.Add(new ProcessTaskType()
                {
                    Name = val.ToString()
                });
            }
            return result;
        }

        private List<ProcessDefinition> GetProcessDefinitions()
        {
            var processDefinitions = new List<ProcessDefinition>();
            for (int i = 1; i < 12; i++)
            {
                var processDefinition = new ProcessDefinition()
                {
                    Id = i,
                    Description = $"Process description {i}",
                    Name = $"Process name {i}",
                    IsEnabled = true,
                    IsReplayable = true,
                    ProcessDefinitionTaskDefinitions = new List<ProcessDefinitionTaskDefinition>() {
                        new ProcessDefinitionTaskDefinition() {
                            ProcessDefinitionId = i,
                            ProcessTaskDefinitionId = 1,
                            Order = 1
                        },
                        new ProcessDefinitionTaskDefinition() {
                            ProcessDefinitionId = i,
                            ProcessTaskDefinitionId = 2,
                            Order = 2
                        }
                    }
                };
                processDefinitions.Add(processDefinition);
            }
            var processDefinitionEmpty = new ProcessDefinition()
            {
                Id = 12,
                Description = $"Process description no tasks",
                Name = $"Process name no tasks",
                IsEnabled = true,
                IsReplayable = true
            };
            processDefinitions.Add(processDefinitionEmpty);
            return processDefinitions;
        }

        private List<ProcessTaskDefinition> GetProcessTaskDefinitions(ProcessTaskType processTaskType)
        {
            var processTaskDefintions = new List<ProcessTaskDefinition>() {
                new ProcessTaskDefinition() {
                    Id = 1,
                    Description = "Process task definition description 1",
                    IsEnabled = true,
                    Name = "Process task definition name 1",
                    Key = "process_task_1",
                    ProcessTaskType = processTaskType
                },
                new ProcessTaskDefinition() {
                    Id = 2,
                    Description = "Process task definition description 2",
                    IsEnabled = true,
                    Name = "Process task definition name 2",
                    Key = "process_task_2",
                    ProcessTaskType = processTaskType
                }
            };
            return processTaskDefintions;
        }

        private async Task SetupDatabase()
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureCreated();
                context.AddRange(GetTaskTypes());
                context.SaveChanges();
                var processTaskType = await context.ProcessTaskType.Where(x => x.Name == ProcessTaskTypeEnum.messageBus.ToString()).FirstAsync();
                context.AddRange(GetProcessTaskDefinitions(processTaskType));
                context.SaveChanges();
                context.AddRange(GetProcessDefinitions());
                context.SaveChanges();
                context.AddRange(GetStatuses());
                context.SaveChanges();

                // All process definition are in the database, create some processes
                var psController = CreateController(context);
                for (var i = 1; i < 7; i++)
                {
                    var dto = new StartProcessDTO($"Process name {i}");
                    await psController.StartProcess(dto);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection.Dispose();
            }
        }
    }
}