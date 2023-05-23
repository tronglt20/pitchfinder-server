﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pitch.Infrastructure;

#nullable disable

namespace Pitch.Infrastructure.Migrations
{
    [DbContext(typeof(PitchDbContext))]
    partial class PitchDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Pitch.Domain.Entities.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KeyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Size")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Attachment", (string)null);
                });

            modelBuilder.Entity("Pitch.Domain.Entities.Pitch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.ToTable("Pitch", (string)null);
                });

            modelBuilder.Entity("Pitch.Domain.Entities.PitchAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AttachmentId")
                        .HasColumnType("int");

                    b.Property<int>("PitchId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("PitchId");

                    b.ToTable("PitchAttachment", (string)null);
                });

            modelBuilder.Entity("Pitch.Domain.Entities.PitchVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<float>("Duration")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PitchId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("PitchId");

                    b.ToTable("PitchVersion", (string)null);
                });

            modelBuilder.Entity("Pitch.Domain.Entities.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Close")
                        .HasColumnType("time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Open")
                        .HasColumnType("time");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Store", (string)null);
                });

            modelBuilder.Entity("Pitch.Domain.Entities.StoreAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AttachmentId")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("StoreId");

                    b.ToTable("StoreAttachment", (string)null);
                });

            modelBuilder.Entity("Pitch.Domain.Entities.StoreComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Content")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.HasIndex("UserId");

                    b.ToTable("StoreComment", (string)null);
                });

            modelBuilder.Entity("Pitch.Domain.Entities.StoreRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.HasIndex("UserId");

                    b.ToTable("StoreRating", (string)null);
                });

            modelBuilder.Entity("Pitch.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Pitch.Domain.Entities.Pitch", b =>
                {
                    b.HasOne("Pitch.Domain.Entities.Store", "Store")
                        .WithMany("Pitchs")
                        .HasForeignKey("StoreId")
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Pitch.Domain.Entities.PitchAttachment", b =>
                {
                    b.HasOne("Pitch.Domain.Entities.Attachment", "Attachment")
                        .WithMany("PitchAttachments")
                        .HasForeignKey("AttachmentId")
                        .IsRequired();

                    b.HasOne("Pitch.Domain.Entities.Pitch", "Pitch")
                        .WithMany("PitchAttachments")
                        .HasForeignKey("PitchId")
                        .IsRequired();

                    b.Navigation("Attachment");

                    b.Navigation("Pitch");
                });

            modelBuilder.Entity("Pitch.Domain.Entities.PitchVersion", b =>
                {
                    b.HasOne("Pitch.Domain.Entities.Pitch", "Pitch")
                        .WithMany("PitchVersions")
                        .HasForeignKey("PitchId")
                        .IsRequired();

                    b.Navigation("Pitch");
                });

            modelBuilder.Entity("Pitch.Domain.Entities.Store", b =>
                {
                    b.HasOne("Pitch.Domain.Entities.User", "Owner")
                        .WithMany("Stores")
                        .HasForeignKey("OwnerId")
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Pitch.Domain.Entities.StoreAttachment", b =>
                {
                    b.HasOne("Pitch.Domain.Entities.Attachment", "Attachment")
                        .WithMany("StoreAttachments")
                        .HasForeignKey("AttachmentId")
                        .IsRequired();

                    b.HasOne("Pitch.Domain.Entities.Store", "Store")
                        .WithMany("StoreAttachments")
                        .HasForeignKey("StoreId")
                        .IsRequired();

                    b.Navigation("Attachment");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Pitch.Domain.Entities.StoreComment", b =>
                {
                    b.HasOne("Pitch.Domain.Entities.Store", "Store")
                        .WithMany("StoreComments")
                        .HasForeignKey("StoreId")
                        .IsRequired();

                    b.HasOne("Pitch.Domain.Entities.User", "User")
                        .WithMany("StoreComments")
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("Store");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pitch.Domain.Entities.StoreRating", b =>
                {
                    b.HasOne("Pitch.Domain.Entities.Store", "Store")
                        .WithMany("StoreRatings")
                        .HasForeignKey("StoreId")
                        .IsRequired();

                    b.HasOne("Pitch.Domain.Entities.User", "User")
                        .WithMany("StoreRatings")
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("Store");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pitch.Domain.Entities.Attachment", b =>
                {
                    b.Navigation("PitchAttachments");

                    b.Navigation("StoreAttachments");
                });

            modelBuilder.Entity("Pitch.Domain.Entities.Pitch", b =>
                {
                    b.Navigation("PitchAttachments");

                    b.Navigation("PitchVersions");
                });

            modelBuilder.Entity("Pitch.Domain.Entities.Store", b =>
                {
                    b.Navigation("Pitchs");

                    b.Navigation("StoreAttachments");

                    b.Navigation("StoreComments");

                    b.Navigation("StoreRatings");
                });

            modelBuilder.Entity("Pitch.Domain.Entities.User", b =>
                {
                    b.Navigation("StoreComments");

                    b.Navigation("StoreRatings");

                    b.Navigation("Stores");
                });
#pragma warning restore 612, 618
        }
    }
}
