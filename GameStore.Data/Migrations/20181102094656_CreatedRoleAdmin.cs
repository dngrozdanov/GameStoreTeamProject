﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace GameStore.Data.Migrations
{
    public partial class CreatedRoleAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "752e3287-1772-4142-8a87-5f01c8e8a36e", "98e9f45a-277c-4596-baa8-ef10a9acc31d", "Admin", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "752e3287-1772-4142-8a87-5f01c8e8a36e", "98e9f45a-277c-4596-baa8-ef10a9acc31d" });
        }
    }
}