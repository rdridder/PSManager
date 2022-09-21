using Microsoft.EntityFrameworkCore;
using Model;

namespace PSData.Context
{
    public class ProcessContext : DbContext
    {
        public ProcessContext()
        {
        }

        public ProcessContext(DbContextOptions<ProcessContext> options) : base(options)
        {
        }

        public virtual DbSet<ProcessDefinition> ProcessDefinitions { get; set; } = null!;

        public virtual DbSet<ProcessTaskDefinition> ProcessTaskDefinition { get; set; } = null!;

        public virtual DbSet<Process> Processes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProcessDefinitionTaskDefinition>()
                .HasKey(pdtd => new { pdtd.ProcessDefinitionId, pdtd.ProcessTaskDefinitionId });

            modelBuilder.Entity<ProcessDefinitionTaskDefinition>()
                .HasOne(bc => bc.ProcessDefinition)
                .WithMany(b => b.ProcessDefinitionTaskDefinitions)
                .HasForeignKey(bc => bc.ProcessDefinitionId);

            modelBuilder.Entity<ProcessDefinitionTaskDefinition>()
                .HasOne(bc => bc.ProcessTaskDefinition)
                .WithMany(c => c.ProcessDefinitionTaskDefinitions)
                .HasForeignKey(bc => bc.ProcessTaskDefinitionId);
        }
    }
}
