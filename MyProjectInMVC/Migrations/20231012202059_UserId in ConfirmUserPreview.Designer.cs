﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyProjectInMVC.Data;

#nullable disable

namespace MyProjectInMVC.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231012202059_UserId in ConfirmUserPreview")]
    partial class UserIdinConfirmUserPreview
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MyProjectInMVC.Models.CategoryModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Category");
                });

            modelBuilder.Entity("MyProjectInMVC.Models.ChatModels.MessageChatModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("varchar(600)");

                    b.Property<string>("NameSenderUser")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("ReceiveUserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("SenderUserId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Chat");
                });

            modelBuilder.Entity("MyProjectInMVC.Models.ConfirmUserHomeworkPreviewModel", b =>
                {
                    b.Property<Guid>("HomeworkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("HomeworkId");

                    b.ToTable("ConfirmUserHomeworkPreview");
                });

            modelBuilder.Entity("MyProjectInMVC.Models.HomeworkModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FilePath")
                        .HasColumnType("longtext");

                    b.Property<string>("Instructions")
                        .HasMaxLength(1255)
                        .HasColumnType("varchar(1255)");

                    b.Property<int?>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Homeworks");
                });

            modelBuilder.Entity("MyProjectInMVC.Models.HomeworkUserModel", b =>
                {
                    b.Property<Guid>("HomeworkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("HomeworkId");

                    b.ToTable("HomeworkUserModel");
                });

            modelBuilder.Entity("MyProjectInMVC.Models.MessageModels.MessageHomeworkModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("HomeworkId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("varchar(600)");

                    b.Property<string>("NameSenderUser")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("ReceiveUserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("SenderUserId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("MessageHomework");
                });

            modelBuilder.Entity("MyProjectInMVC.Models.UserCategoryModel", b =>
                {
                    b.Property<Guid>("UserCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("UserCategoryId");

                    b.ToTable("UserCategory");
                });

            modelBuilder.Entity("MyProjectInMVC.Models.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("phoneNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyProjectInMVC.Models.HomeworkModel", b =>
                {
                    b.HasOne("MyProjectInMVC.Models.CategoryModel", "Category")
                        .WithMany("Homeworks")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MyProjectInMVC.Models.CategoryModel", b =>
                {
                    b.Navigation("Homeworks");
                });
#pragma warning restore 612, 618
        }
    }
}
