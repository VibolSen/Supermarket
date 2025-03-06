﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Supermarket.Data;

#nullable disable

namespace Supermarket.Migrations
{
    [DbContext(typeof(SupermarketContext))]
    [Migration("20250306045104_UpdateSaleNotes")]
    partial class UpdateSaleNotes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Supermarket.Models.AuditLog", b =>
                {
                    b.Property<int>("LogID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LogID"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<string>("NewValues")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValues")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecordID")
                        .HasColumnType("int");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("LogID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("Supermarket.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryID"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Supermarket.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerID"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("LoyaltyPoints")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Supermarket.Models.Discount", b =>
                {
                    b.Property<int>("DiscountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DiscountID"));

                    b.Property<int?>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("DiscountName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("DiscountType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("DiscountValue")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ProductID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("DiscountID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("ProductID");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("Supermarket.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeID"));

                    b.Property<string>("Attendance")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("EmployeeID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Supermarket.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductID"));

                    b.Property<int?>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("ReorderLevel")
                        .HasColumnType("int");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("int");

                    b.Property<int>("SupplierID")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("ProductID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("SupplierID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Supermarket.Models.Sale", b =>
                {
                    b.Property<int>("SaleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SaleID"));

                    b.Property<int?>("CustomerID")
                        .HasColumnType("int");

                    b.Property<decimal?>("DiscountAmount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("SaleID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Supermarket.Models.SaleItem", b =>
                {
                    b.Property<int>("SaleItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SaleItemID"));

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("SaleID")
                        .HasColumnType("int");

                    b.Property<decimal>("Subtotal")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("SaleItemID");

                    b.HasIndex("ProductID");

                    b.HasIndex("SaleID");

                    b.ToTable("SaleItems");
                });

            modelBuilder.Entity("Supermarket.Models.Supplier", b =>
                {
                    b.Property<int>("SupplierID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SupplierID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactPerson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("SupplierID");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("Supermarket.Models.AuditLog", b =>
                {
                    b.HasOne("Supermarket.Models.Employee", "Employee")
                        .WithMany("AuditLogs")
                        .HasForeignKey("EmployeeID");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Supermarket.Models.Discount", b =>
                {
                    b.HasOne("Supermarket.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID");

                    b.HasOne("Supermarket.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Supermarket.Models.Product", b =>
                {
                    b.HasOne("Supermarket.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryID");

                    b.HasOne("Supermarket.Models.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("Supermarket.Models.Sale", b =>
                {
                    b.HasOne("Supermarket.Models.Customer", "Customer")
                        .WithMany("Sales")
                        .HasForeignKey("CustomerID");

                    b.HasOne("Supermarket.Models.Employee", "Employee")
                        .WithMany("Sales")
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Supermarket.Models.SaleItem", b =>
                {
                    b.HasOne("Supermarket.Models.Product", "Product")
                        .WithMany("SaleItems")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Supermarket.Models.Sale", "Sale")
                        .WithMany("SaleItems")
                        .HasForeignKey("SaleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("Supermarket.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Supermarket.Models.Customer", b =>
                {
                    b.Navigation("Sales");
                });

            modelBuilder.Entity("Supermarket.Models.Employee", b =>
                {
                    b.Navigation("AuditLogs");

                    b.Navigation("Sales");
                });

            modelBuilder.Entity("Supermarket.Models.Product", b =>
                {
                    b.Navigation("SaleItems");
                });

            modelBuilder.Entity("Supermarket.Models.Sale", b =>
                {
                    b.Navigation("SaleItems");
                });

            modelBuilder.Entity("Supermarket.Models.Supplier", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
