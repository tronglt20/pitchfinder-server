﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pitch.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<float>(type: "real", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Open = table.Column<TimeSpan>(type: "time", nullable: false),
                    Close = table.Column<TimeSpan>(type: "time", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Store_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pitch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pitch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pitch_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoreAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreAttachment_Attachment_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreAttachment_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoreComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreComment_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreComment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoreRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreRating_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreRating_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PitchAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PitchId = table.Column<int>(type: "int", nullable: false),
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PitchAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PitchAttachment_Attachment_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PitchAttachment_Pitch_PitchId",
                        column: x => x.PitchId,
                        principalTable: "Pitch",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PitchVersion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<float>(type: "real", nullable: false),
                    PitchId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PitchVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PitchVersion_Pitch_PitchId",
                        column: x => x.PitchId,
                        principalTable: "Pitch",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pitch_StoreId",
                table: "Pitch",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_PitchAttachment_AttachmentId",
                table: "PitchAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PitchAttachment_PitchId",
                table: "PitchAttachment",
                column: "PitchId");

            migrationBuilder.CreateIndex(
                name: "IX_PitchVersion_PitchId",
                table: "PitchVersion",
                column: "PitchId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_OwnerId",
                table: "Store",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreAttachment_AttachmentId",
                table: "StoreAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreAttachment_StoreId",
                table: "StoreAttachment",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreComment_StoreId",
                table: "StoreComment",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreComment_UserId",
                table: "StoreComment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreRating_StoreId",
                table: "StoreRating",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreRating_UserId",
                table: "StoreRating",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PitchAttachment");

            migrationBuilder.DropTable(
                name: "PitchVersion");

            migrationBuilder.DropTable(
                name: "StoreAttachment");

            migrationBuilder.DropTable(
                name: "StoreComment");

            migrationBuilder.DropTable(
                name: "StoreRating");

            migrationBuilder.DropTable(
                name: "Pitch");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
