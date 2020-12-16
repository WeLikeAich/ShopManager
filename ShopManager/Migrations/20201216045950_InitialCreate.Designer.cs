﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShopManager;

namespace ShopManager.Migrations
{
    [DbContext(typeof(ShopContext))]
    [Migration("20201216045950_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("ShopManager.Entities.Color", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ColorCode")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Cost")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MaterialId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId");

                    b.ToTable("colors");
                });

            modelBuilder.Entity("ShopManager.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("items");
                });

            modelBuilder.Entity("ShopManager.Entities.Material", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("FriendlyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FullMaterialName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasAlternateKey("FriendlyName")
                        .HasName("AlternateKey_FriendlyName");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("ShopManager.Entities.MaterialCount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MaterialId")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaterialUnitCount")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("SizeOptionId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId");

                    b.HasIndex("SizeOptionId");

                    b.ToTable("MaterialCounts");
                });

            modelBuilder.Entity("ShopManager.Entities.SizeOption", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TimeToMakeInHours")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("size_options");
                });

            modelBuilder.Entity("ShopManager.Entities.Color", b =>
                {
                    b.HasOne("ShopManager.Entities.Material", null)
                        .WithMany("Colors")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShopManager.Entities.MaterialCount", b =>
                {
                    b.HasOne("ShopManager.Entities.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShopManager.Entities.SizeOption", null)
                        .WithMany("MaterialCounts")
                        .HasForeignKey("SizeOptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");
                });

            modelBuilder.Entity("ShopManager.Entities.SizeOption", b =>
                {
                    b.HasOne("ShopManager.Entities.Item", null)
                        .WithMany("SizeOptions")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShopManager.Entities.Item", b =>
                {
                    b.Navigation("SizeOptions");
                });

            modelBuilder.Entity("ShopManager.Entities.Material", b =>
                {
                    b.Navigation("Colors");
                });

            modelBuilder.Entity("ShopManager.Entities.SizeOption", b =>
                {
                    b.Navigation("MaterialCounts");
                });
#pragma warning restore 612, 618
        }
    }
}
