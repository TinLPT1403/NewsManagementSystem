﻿// <auto-generated />
using System;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(NewsContext))]
    [Migration("20250215034245_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("int");

                    b.HasKey("CategoryId");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DAL.Entities.NewsArticle", b =>
                {
                    b.Property<string>("NewsArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Headline")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NewsContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewsSource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewsTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("int");

                    b.HasKey("NewsArticleId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("NewsArticles");
                });

            modelBuilder.Entity("DAL.Entities.NewsTag", b =>
                {
                    b.Property<string>("NewsArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("NewsArticleId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("NewsTags");
                });

            modelBuilder.Entity("DAL.Entities.SystemAccount", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<string>("AccountEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("AccountRole")
                        .HasColumnType("int");

                    b.HasKey("AccountId");

                    b.ToTable("SystemAccounts");
                });

            modelBuilder.Entity("DAL.Entities.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"));

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TagName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("DAL.Entities.Category", b =>
                {
                    b.HasOne("DAL.Entities.Category", "ParentCategory")
                        .WithMany()
                        .HasForeignKey("ParentCategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("DAL.Entities.NewsArticle", b =>
                {
                    b.HasOne("DAL.Entities.SystemAccount", "CreatedBy")
                        .WithMany("CreatedArticles")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DAL.Entities.SystemAccount", "UpdatedBy")
                        .WithMany("UpdatedArticles")
                        .HasForeignKey("UpdatedById")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CreatedBy");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("DAL.Entities.NewsTag", b =>
                {
                    b.HasOne("DAL.Entities.NewsArticle", "NewsArticle")
                        .WithMany("NewsTags")
                        .HasForeignKey("NewsArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Tag", "Tag")
                        .WithMany("NewsTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("NewsArticle");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("DAL.Entities.NewsArticle", b =>
                {
                    b.Navigation("NewsTags");
                });

            modelBuilder.Entity("DAL.Entities.SystemAccount", b =>
                {
                    b.Navigation("CreatedArticles");

                    b.Navigation("UpdatedArticles");
                });

            modelBuilder.Entity("DAL.Entities.Tag", b =>
                {
                    b.Navigation("NewsTags");
                });
#pragma warning restore 612, 618
        }
    }
}
