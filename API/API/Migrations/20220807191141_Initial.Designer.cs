﻿// <auto-generated />
using API.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Migrations
{
    [DbContext(typeof(NFTContext))]
    [Migration("20220807191141_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("API.Data.Entities.NFT", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("ATK")
                        .HasColumnType("int");

                    b.Property<int>("ATK_SPD")
                        .HasColumnType("int");

                    b.Property<int>("ATK_SPD_C")
                        .HasColumnType("int");

                    b.Property<float>("ATK_SPD_C_XP")
                        .HasColumnType("real");

                    b.Property<int>("ATK_SPD_S")
                        .HasColumnType("int");

                    b.Property<float>("ATK_SPD_S_XP")
                        .HasColumnType("real");

                    b.Property<float>("ATK_SPD_XP")
                        .HasColumnType("real");

                    b.Property<float>("ATK_XP")
                        .HasColumnType("real");

                    b.Property<int>("CHP")
                        .HasColumnType("int");

                    b.Property<int>("DEF")
                        .HasColumnType("int");

                    b.Property<int>("DEF_SPD_D")
                        .HasColumnType("int");

                    b.Property<float>("DEF_SPD_D_XP")
                        .HasColumnType("real");

                    b.Property<float>("DEF_XP")
                        .HasColumnType("real");

                    b.Property<int>("SPD")
                        .HasColumnType("int");

                    b.Property<float>("SPD_XP")
                        .HasColumnType("real");

                    b.Property<int>("Skill")
                        .HasColumnType("int");

                    b.Property<string>("SpriteSheetLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NFTs");
                });
#pragma warning restore 612, 618
        }
    }
}