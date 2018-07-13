using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace login.Data.Migrations
{
    public partial class mi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(

                name: "IdUsuario",
                table: "PeopleData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "PeopleData",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PeopleData_UsuarioId",
                table: "PeopleData",
                column: "UsuarioId",
                unique: true,
                filter: "[UsuarioId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PeopleData_AspNetUsers_UsuarioId",
                table: "PeopleData",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PeopleData_AspNetUsers_UsuarioId",
                table: "PeopleData");

            migrationBuilder.DropIndex(
                name: "IX_PeopleData_UsuarioId",
                table: "PeopleData");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "PeopleData");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "PeopleData");
        }
    }
}
