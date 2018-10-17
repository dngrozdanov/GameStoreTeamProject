﻿// <auto-generated />

using System;
using GameStore.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GameStore.Data.Migrations
{
    [DbContext(typeof(GameStoreContext))]
    internal class GameStoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GameStore.Data.Models.Account", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy",
                        SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("CreatedOn");

                b.Property<string>("CreditCard");

                b.Property<string>("DeletedBy");

                b.Property<string>("FirstName")
                    .HasMaxLength(20);

                b.Property<bool>("IsAdmin");

                b.Property<bool>("IsDeleted");

                b.Property<bool>("IsGuest");

                b.Property<string>("LastName")
                    .HasMaxLength(20);

                b.Property<string>("Password")
                    .IsRequired();

                b.Property<int>("ShoppingCartId");

                b.Property<string>("Username")
                    .IsRequired()
                    .HasMaxLength(20);

                b.HasKey("Id");

                b.HasIndex("ShoppingCartId");

                b.HasIndex("Username")
                    .IsUnique();

                b.ToTable("Accounts");
            });

            modelBuilder.Entity("GameStore.Data.Models.Comment", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy",
                        SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<int>("AccountId");

                b.Property<bool>("IsDeleted");

                b.Property<int>("ProductId");

                b.Property<string>("Text")
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property<DateTime>("TimeStamp");

                b.HasKey("Id");

                b.HasIndex("AccountId");

                b.HasIndex("ProductId");

                b.ToTable("Comments");
            });

            modelBuilder.Entity("GameStore.Data.Models.Genre", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy",
                        SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Name")
                    .IsRequired();

                b.Property<int>("ProductId");

                b.HasKey("Id");

                b.HasIndex("ProductId");

                b.ToTable("Genres");
            });

            modelBuilder.Entity("GameStore.Data.Models.Order", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy",
                        SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<int>("AccountId");

                b.Property<DateTime>("OrderTimestamp");

                b.HasKey("Id");

                b.HasIndex("AccountId");

                b.ToTable("Orders");
            });

            modelBuilder.Entity("GameStore.Data.Models.OrderProducts", b =>
            {
                b.Property<int>("OrderId");

                b.Property<int>("ProductId");

                b.HasKey("OrderId", "ProductId");

                b.HasIndex("ProductId");

                b.ToTable("OrdersProducts");
            });

            modelBuilder.Entity("GameStore.Data.Models.Product", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy",
                        SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("CreatedOn");

                b.Property<string>("Description")
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property<bool>("IsDeleted");

                b.Property<bool>("IsOnSale");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(20);

                b.Property<decimal>("Price");

                b.HasKey("Id");

                b.HasIndex("Name")
                    .IsUnique();

                b.ToTable("Products");
            });

            modelBuilder.Entity("GameStore.Data.Models.ShoppingCart", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy",
                        SqlServerValueGenerationStrategy.IdentityColumn);

                b.HasKey("Id");

                b.ToTable("ShoppingCarts");
            });

            modelBuilder.Entity("GameStore.Data.Models.ShoppingCartProducts", b =>
            {
                b.Property<int>("ProductId");

                b.Property<int>("ShoppingCartId");

                b.HasKey("ProductId", "ShoppingCartId");

                b.HasIndex("ShoppingCartId");

                b.ToTable("ShoppingCartProducts");
            });

            modelBuilder.Entity("GameStore.Data.Models.Account", b =>
            {
                b.HasOne("GameStore.Data.Models.ShoppingCart", "ShoppingCart")
                    .WithMany()
                    .HasForeignKey("ShoppingCartId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("GameStore.Data.Models.Comment", b =>
            {
                b.HasOne("GameStore.Data.Models.Account", "Account")
                    .WithMany("Comments")
                    .HasForeignKey("AccountId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("GameStore.Data.Models.Product", "Product")
                    .WithMany("Comments")
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("GameStore.Data.Models.Genre", b =>
            {
                b.HasOne("GameStore.Data.Models.Product", "Product")
                    .WithMany("Genre")
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("GameStore.Data.Models.Order", b =>
            {
                b.HasOne("GameStore.Data.Models.Account", "Account")
                    .WithMany("OrderProducts")
                    .HasForeignKey("AccountId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("GameStore.Data.Models.OrderProducts", b =>
            {
                b.HasOne("GameStore.Data.Models.Order", "Order")
                    .WithMany("OrderProducts")
                    .HasForeignKey("OrderId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("GameStore.Data.Models.Product", "Product")
                    .WithMany("OrderProducts")
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("GameStore.Data.Models.ShoppingCartProducts", b =>
            {
                b.HasOne("GameStore.Data.Models.Product", "Product")
                    .WithMany("ShoppingCartProducts")
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("GameStore.Data.Models.ShoppingCart", "ShoppingCart")
                    .WithMany("ShoppingCartProducts")
                    .HasForeignKey("ShoppingCartId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
#pragma warning restore 612, 618
        }
    }
}