using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model;
using Moq;
using PSAPI.AutoMapper;
using PSAPI.Controllers;
using PSData.Context;

namespace PSAPI.Test
{
    public abstract class DatabaseTestBase : IDisposable
    {
        protected readonly ILogger<PSController> _logger;

        protected readonly IMapper _mapper;

        protected readonly IConfiguration _config;

        protected readonly SqliteConnection _connection;

        protected readonly DbContextOptions<ProcessContext> _contextOptions;

        public DatabaseTestBase()
        {
            _logger = new Mock<ILogger<PSController>>().Object;
            _mapper = new Mapper(GetMapperConfig());
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
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

            SetupDatabase();
        }

        public ProcessContext CreateContext() => new ProcessContext(_contextOptions);

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
                        ProcessTaskDefinitionId = 1
                        },
                        new ProcessDefinitionTaskDefinition() {
                            ProcessDefinitionId = i,
                            ProcessTaskDefinitionId = 2
                        }
                    }
                };
                processDefinitions.Add(processDefinition);
            }
            return processDefinitions;
        }

        private List<ProcessTaskDefinition> GetProcessTaskDefinitions()
        {
            var processTaskDefintions = new List<ProcessTaskDefinition>() {
                new ProcessTaskDefinition() {
                    Id = 1,
                    Description = "Process task definition description 1",
                    IsEnabled = true,
                    Name = "Process task definition name 1",
                    Key = "process_task_1"
                },
                new ProcessTaskDefinition() {
                    Id = 2,
                    Description = "Process task definition description 2",
                    IsEnabled = true,
                    Name = "Process task definition name 2",
                    Key = "process_task_2"
                }
            };
            return processTaskDefintions;
        }

        private void SetupDatabase()
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureCreated();
                context.AddRange(GetProcessTaskDefinitions());
                context.SaveChanges();
                context.AddRange(GetProcessDefinitions());
                context.SaveChanges();
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