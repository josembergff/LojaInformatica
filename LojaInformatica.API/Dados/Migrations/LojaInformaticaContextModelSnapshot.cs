﻿// <auto-generated />
using LojaInformatica.API.Dados;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace LojaInformatica.API.Dados.Migrations
{
    [DbContext(typeof(ContextoLojaInformatica))]
    partial class LojaInformaticaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("LojaInformatica.API.Entidades.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nome");

                    b.HasKey("Id");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("LojaInformatica.API.Entidades.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ChaveDeAcesso");

                    b.Property<string>("Email");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("LojaInformatica.API.Entidades.Compra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DataDaCompra");

                    b.HasKey("Id");

                    b.ToTable("Compras");
                });

            modelBuilder.Entity("LojaInformatica.API.Entidades.Imagem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("ImagemPrincipal");

                    b.Property<int>("ProdutoId");

                    b.Property<string>("URL");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId");

                    b.ToTable("Imagem");
                });

            modelBuilder.Entity("LojaInformatica.API.Entidades.ItemDaCompra", b =>
                {
                    b.Property<int>("CompraId");

                    b.Property<int>("ProdutoId");

                    b.Property<decimal>("PrecoUnitario");

                    b.Property<int>("Quantidade");

                    b.HasKey("CompraId", "ProdutoId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("ItemDaCompra");
                });

            modelBuilder.Entity("LojaInformatica.API.Entidades.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CategoriaId");

                    b.Property<string>("Descricao");

                    b.Property<string>("Nome");

                    b.Property<decimal>("Preco");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("LojaInformatica.API.Entidades.Imagem", b =>
                {
                    b.HasOne("LojaInformatica.API.Entidades.Produto")
                        .WithMany("Imagens")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LojaInformatica.API.Entidades.ItemDaCompra", b =>
                {
                    b.HasOne("LojaInformatica.API.Entidades.Compra", "Compra")
                        .WithMany("ItensDaCompra")
                        .HasForeignKey("CompraId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LojaInformatica.API.Entidades.Produto", "Produto")
                        .WithMany("ItemComprados")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LojaInformatica.API.Entidades.Produto", b =>
                {
                    b.HasOne("LojaInformatica.API.Entidades.Categoria", "Categoria")
                        .WithMany("Produtos")
                        .HasForeignKey("CategoriaId");
                });
#pragma warning restore 612, 618
        }
    }
}
