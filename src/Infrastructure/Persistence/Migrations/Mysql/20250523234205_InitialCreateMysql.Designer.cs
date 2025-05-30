﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Persistence.Migrations.Mysql
{
    [DbContext(typeof(ApplicationDbContextMySql))]
    [Migration("20250523234205_InitialCreateMysql")]
    partial class InitialCreateMysql
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Domain.Customers.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdCustomer");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Email");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("LastName");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Id");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("Domain.Customers.Customer", b =>
                {
                    b.OwnsOne("Domain.ValueObjects.AuditRecord", "AuditRecord", b1 =>
                        {
                            b1.Property<int>("CustomerId")
                                .HasColumnType("int");

                            b1.Property<string>("AppCreator")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("varchar(50)")
                                .HasColumnName("AppCreator");

                            b1.Property<string>("AppUpdater")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("varchar(50)")
                                .HasColumnName("AppUpdater");

                            b1.Property<bool>("Asset")
                                .HasColumnType("tinyint(1)")
                                .HasColumnName("Asset");

                            b1.Property<DateTime>("DateCreated")
                                .HasColumnType("datetime(6)")
                                .HasColumnName("DateCreated");

                            b1.Property<DateTime>("DateUpdate")
                                .HasColumnType("datetime(6)")
                                .HasColumnName("DateUpdate");

                            b1.Property<string>("HostCreator")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("HostCreator");

                            b1.Property<string>("HostUpdater")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("HostUpdater");

                            b1.Property<string>("UserCreator")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("varchar(30)")
                                .HasColumnName("UserCreator");

                            b1.Property<string>("UserUpdater")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("varchar(30)")
                                .HasColumnName("UserUpdater");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customer");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("AuditRecord")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
