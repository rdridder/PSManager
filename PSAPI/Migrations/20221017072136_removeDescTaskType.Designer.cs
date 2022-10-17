﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PSData.Context;

#nullable disable

namespace PSAPI.Migrations
{
    [DbContext(typeof(ProcessContext))]
    [Migration("20221017072136_removeDescTaskType")]
    partial class removeDescTaskType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Model.Process", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<bool>("IsReplayable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<long>("StatusId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.ToTable("Processes");
                });

            modelBuilder.Entity("Model.ProcessDefinition", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReplayable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.ToTable("ProcessDefinitions");
                });

            modelBuilder.Entity("Model.ProcessDefinitionTaskDefinition", b =>
                {
                    b.Property<long>("ProcessDefinitionId")
                        .HasColumnType("bigint");

                    b.Property<long>("ProcessTaskDefinitionId")
                        .HasColumnType("bigint");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("ProcessDefinitionId", "ProcessTaskDefinitionId");

                    b.HasIndex("ProcessTaskDefinitionId");

                    b.ToTable("ProcessDefinitionTaskDefinition");
                });

            modelBuilder.Entity("Model.ProcessTask", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<long?>("ProcessId")
                        .HasColumnType("bigint");

                    b.Property<long>("ProcessTaskTypeId")
                        .HasColumnType("bigint");

                    b.Property<long>("StatusId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ProcessId");

                    b.HasIndex("ProcessTaskTypeId");

                    b.HasIndex("StatusId");

                    b.ToTable("ProcessTask");
                });

            modelBuilder.Entity("Model.ProcessTaskDefinition", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<long>("ProcessTaskTypeId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.HasIndex("ProcessTaskTypeId");

                    b.ToTable("ProcessTaskDefinition");
                });

            modelBuilder.Entity("Model.ProcessTaskType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Id");

                    b.ToTable("ProcessTaskType");
                });

            modelBuilder.Entity("Model.Status", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Id");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("Model.Process", b =>
                {
                    b.HasOne("Model.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Model.ProcessDefinitionTaskDefinition", b =>
                {
                    b.HasOne("Model.ProcessDefinition", "ProcessDefinition")
                        .WithMany("ProcessDefinitionTaskDefinitions")
                        .HasForeignKey("ProcessDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Model.ProcessTaskDefinition", "ProcessTaskDefinition")
                        .WithMany("ProcessDefinitionTaskDefinitions")
                        .HasForeignKey("ProcessTaskDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProcessDefinition");

                    b.Navigation("ProcessTaskDefinition");
                });

            modelBuilder.Entity("Model.ProcessTask", b =>
                {
                    b.HasOne("Model.Process", null)
                        .WithMany("ProcessTasks")
                        .HasForeignKey("ProcessId");

                    b.HasOne("Model.ProcessTaskType", "ProcessTaskType")
                        .WithMany()
                        .HasForeignKey("ProcessTaskTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Model.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProcessTaskType");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Model.ProcessTaskDefinition", b =>
                {
                    b.HasOne("Model.ProcessTaskType", "ProcessTaskType")
                        .WithMany()
                        .HasForeignKey("ProcessTaskTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProcessTaskType");
                });

            modelBuilder.Entity("Model.Process", b =>
                {
                    b.Navigation("ProcessTasks");
                });

            modelBuilder.Entity("Model.ProcessDefinition", b =>
                {
                    b.Navigation("ProcessDefinitionTaskDefinitions");
                });

            modelBuilder.Entity("Model.ProcessTaskDefinition", b =>
                {
                    b.Navigation("ProcessDefinitionTaskDefinitions");
                });
#pragma warning restore 612, 618
        }
    }
}
