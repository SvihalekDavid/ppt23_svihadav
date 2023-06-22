﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PPT23.API.Data;

#nullable disable

namespace PPT23.API.Migrations
{
    [DbContext(typeof(PptDbContext))]
    partial class PptDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("PPT23.API.Data.Pracovnik", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Povolani")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Pracovniks");
                });

            modelBuilder.Entity("PPT23.API.Data.Revize", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("VybaveniId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("VybaveniId");

                    b.ToTable("Revizes");
                });

            modelBuilder.Entity("PPT23.API.Data.Ukon", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Detail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Kod")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PracovnikId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("VybaveniId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PracovnikId");

                    b.HasIndex("VybaveniId");

                    b.ToTable("Ukons");
                });

            modelBuilder.Entity("PPT23.API.Data.Vybaveni", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("BoughtDateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Cena")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Vybavenis");
                });

            modelBuilder.Entity("PPT23.API.Data.Revize", b =>
                {
                    b.HasOne("PPT23.API.Data.Vybaveni", "Vybaveni")
                        .WithMany("Revizes")
                        .HasForeignKey("VybaveniId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vybaveni");
                });

            modelBuilder.Entity("PPT23.API.Data.Ukon", b =>
                {
                    b.HasOne("PPT23.API.Data.Pracovnik", "Pracovnik")
                        .WithMany("Ukons")
                        .HasForeignKey("PracovnikId");

                    b.HasOne("PPT23.API.Data.Vybaveni", "Vybaveni")
                        .WithMany("Ukons")
                        .HasForeignKey("VybaveniId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pracovnik");

                    b.Navigation("Vybaveni");
                });

            modelBuilder.Entity("PPT23.API.Data.Pracovnik", b =>
                {
                    b.Navigation("Ukons");
                });

            modelBuilder.Entity("PPT23.API.Data.Vybaveni", b =>
                {
                    b.Navigation("Revizes");

                    b.Navigation("Ukons");
                });
#pragma warning restore 612, 618
        }
    }
}