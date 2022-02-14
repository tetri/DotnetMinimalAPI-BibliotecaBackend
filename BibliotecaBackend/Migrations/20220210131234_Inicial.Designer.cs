﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BibliotecaBackend.Migrations
{
    [DbContext(typeof(BibliotecaDB))]
    [Migration("20220210131234_Inicial")]
    partial class Inicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.2");

            modelBuilder.Entity("Obra", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("autores")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("editora")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("foto")
                        .HasColumnType("TEXT");

                    b.Property<string>("titulo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Obras");
                });
#pragma warning restore 612, 618
        }
    }
}
