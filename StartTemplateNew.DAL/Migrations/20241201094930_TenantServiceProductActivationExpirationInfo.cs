using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartTemplateNew.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TenantServiceProductActivationExpirationInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ActivationDate",
                table: "TenantServices",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivationSetById",
                table: "TenantServices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpirationDate",
                table: "TenantServices",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExpirationSetById",
                table: "TenantServices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ActivationDate",
                table: "TenantProducts",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivationSetById",
                table: "TenantProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpirationDate",
                table: "TenantProducts",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExpirationSetById",
                table: "TenantProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantServices_ActivationSetById",
                table: "TenantServices",
                column: "ActivationSetById");

            migrationBuilder.CreateIndex(
                name: "IX_TenantServices_ExpirationSetById",
                table: "TenantServices",
                column: "ExpirationSetById");

            migrationBuilder.CreateIndex(
                name: "IX_TenantProducts_ActivationSetById",
                table: "TenantProducts",
                column: "ActivationSetById");

            migrationBuilder.CreateIndex(
                name: "IX_TenantProducts_ExpirationSetById",
                table: "TenantProducts",
                column: "ExpirationSetById");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantProducts_AspNetUsers_ActivationSetById",
                table: "TenantProducts",
                column: "ActivationSetById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantProducts_AspNetUsers_ExpirationSetById",
                table: "TenantProducts",
                column: "ExpirationSetById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantServices_AspNetUsers_ActivationSetById",
                table: "TenantServices",
                column: "ActivationSetById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantServices_AspNetUsers_ExpirationSetById",
                table: "TenantServices",
                column: "ExpirationSetById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantProducts_AspNetUsers_ActivationSetById",
                table: "TenantProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantProducts_AspNetUsers_ExpirationSetById",
                table: "TenantProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantServices_AspNetUsers_ActivationSetById",
                table: "TenantServices");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantServices_AspNetUsers_ExpirationSetById",
                table: "TenantServices");

            migrationBuilder.DropIndex(
                name: "IX_TenantServices_ActivationSetById",
                table: "TenantServices");

            migrationBuilder.DropIndex(
                name: "IX_TenantServices_ExpirationSetById",
                table: "TenantServices");

            migrationBuilder.DropIndex(
                name: "IX_TenantProducts_ActivationSetById",
                table: "TenantProducts");

            migrationBuilder.DropIndex(
                name: "IX_TenantProducts_ExpirationSetById",
                table: "TenantProducts");

            migrationBuilder.DropColumn(
                name: "ActivationDate",
                table: "TenantServices");

            migrationBuilder.DropColumn(
                name: "ActivationSetById",
                table: "TenantServices");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "TenantServices");

            migrationBuilder.DropColumn(
                name: "ExpirationSetById",
                table: "TenantServices");

            migrationBuilder.DropColumn(
                name: "ActivationDate",
                table: "TenantProducts");

            migrationBuilder.DropColumn(
                name: "ActivationSetById",
                table: "TenantProducts");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "TenantProducts");

            migrationBuilder.DropColumn(
                name: "ExpirationSetById",
                table: "TenantProducts");
        }
    }
}
