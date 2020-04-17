﻿// <auto-generated />
using System;
using HomeBudgetWf.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HomeBudgetWf.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HomeBudgetWf.Models.ExpenseCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ExpenseCategories");
                });

            modelBuilder.Entity("HomeBudgetWf.Models.KeyWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ExpenseCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseCategoryId");

                    b.ToTable("KeyWords");
                });

            modelBuilder.Entity("HomeBudgetWf.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("DateOfTransaction")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfregistration")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("KeyWordId")
                        .HasColumnType("int");

                    b.Property<string>("OthersDetails")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("KeyWordId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("HomeBudgetWf.Models.KeyWord", b =>
                {
                    b.HasOne("HomeBudgetWf.Models.ExpenseCategory", "ExpenseCategory")
                        .WithMany("KeyWords")
                        .HasForeignKey("ExpenseCategoryId");
                });

            modelBuilder.Entity("HomeBudgetWf.Models.Transaction", b =>
                {
                    b.HasOne("HomeBudgetWf.Models.KeyWord", "KeyWord")
                        .WithMany("Transactions")
                        .HasForeignKey("KeyWordId");
                });
#pragma warning restore 612, 618
        }
    }
}
