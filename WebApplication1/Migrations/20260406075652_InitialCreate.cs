using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[dbo].[Expenses]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[Expenses] (
                        [Id] int NOT NULL IDENTITY,
                        [Code] nvarchar(max) NOT NULL,
                        [Description] nvarchar(max) NOT NULL,
                        [Amount] decimal(18,2) NOT NULL,
                        [CreatedBy] nvarchar(max) NOT NULL,
                        [CreatedDate] datetime2 NOT NULL,
                        CONSTRAINT [PK_Expenses] PRIMARY KEY ([Id])
                    );
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF OBJECT_ID(N'[dbo].[Expenses]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [dbo].[Expenses];
                END
                """);
        }
    }
}
