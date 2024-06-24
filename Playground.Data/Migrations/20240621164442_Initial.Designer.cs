﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Playground.Data.Contexts;

#nullable disable

namespace Playground.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240621164442_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("Playground.Domain.Entities.Asset", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AssetTypeId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Quantity")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Guid");

                    b.HasIndex("AssetTypeId");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("Playground.Domain.Entities.AssetType", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Guid");

                    b.ToTable("AssetTypes");
                });

            modelBuilder.Entity("Playground.Domain.Entities.Portfolio", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Guid");

                    b.ToTable("Portfolios");
                });

            modelBuilder.Entity("Playground.Domain.Entities.PortfolioAsset", b =>
                {
                    b.Property<Guid>("PortfolioId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AssetId")
                        .HasColumnType("TEXT");

                    b.HasKey("PortfolioId", "AssetId");

                    b.HasIndex("AssetId");

                    b.ToTable("PortfolioAssets");
                });

            modelBuilder.Entity("Playground.Domain.Entities.Asset", b =>
                {
                    b.HasOne("Playground.Domain.Entities.AssetType", "AssetType")
                        .WithMany()
                        .HasForeignKey("AssetTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssetType");
                });

            modelBuilder.Entity("Playground.Domain.Entities.PortfolioAsset", b =>
                {
                    b.HasOne("Playground.Domain.Entities.Asset", "Asset")
                        .WithMany("PortfolioAssets")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Playground.Domain.Entities.Portfolio", "Portfolio")
                        .WithMany("PortfolioAssets")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asset");

                    b.Navigation("Portfolio");
                });

            modelBuilder.Entity("Playground.Domain.Entities.Asset", b =>
                {
                    b.Navigation("PortfolioAssets");
                });

            modelBuilder.Entity("Playground.Domain.Entities.Portfolio", b =>
                {
                    b.Navigation("PortfolioAssets");
                });
#pragma warning restore 612, 618
        }
    }
}
