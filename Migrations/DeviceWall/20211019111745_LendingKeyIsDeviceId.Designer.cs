﻿// <auto-generated />

using device_wall_backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace device_wall_backend.Migrations.DeviceWall
{
    [DbContext(typeof(DeviceWallContext))]
    [Migration("20211019111745_LendingKeyIsDeviceId")]
    partial class LendingKeyIsDeviceId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("device_wall_backend.Models.Device", b =>
                {
                    b.Property<int>("DeviceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Dpi")
                        .HasColumnType("integer");

                    b.Property<bool>("HasSIM")
                        .HasColumnType("boolean");

                    b.Property<int>("HorizontalSize")
                        .HasColumnType("integer");

                    b.Property<bool>("IsTablet")
                        .HasColumnType("boolean");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("OperatingSystem")
                        .HasColumnType("text");

                    b.Property<string>("Port")
                        .HasColumnType("text");

                    b.Property<string>("Version")
                        .HasColumnType("text");

                    b.Property<int>("VerticalSize")
                        .HasColumnType("integer");

                    b.HasKey("DeviceID");

                    b.ToTable("Device");
                });

            modelBuilder.Entity("device_wall_backend.Models.Lending", b =>
                {
                    b.Property<int>("DeviceID")
                        .HasColumnType("integer");

                    b.Property<bool>("IsLongterm")
                        .HasColumnType("boolean");

                    b.Property<int>("UserID")
                        .HasColumnType("integer");

                    b.HasKey("DeviceID");

                    b.HasIndex("UserID");

                    b.ToTable("Lending");
                });

            modelBuilder.Entity("device_wall_backend.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("UserID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("device_wall_backend.Models.Lending", b =>
                {
                    b.HasOne("device_wall_backend.Models.Device", "Device")
                        .WithOne("currentLending")
                        .HasForeignKey("device_wall_backend.Models.Lending", "DeviceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("device_wall_backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("User");
                });

            modelBuilder.Entity("device_wall_backend.Models.Device", b =>
                {
                    b.Navigation("currentLending");
                });
#pragma warning restore 612, 618
        }
    }
}
