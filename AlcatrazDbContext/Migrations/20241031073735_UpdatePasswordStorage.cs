using Alcatraz.DTO.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;


#nullable disable

namespace Alcatraz.Context.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePasswordStorage : Migration
    {
        MainDbContext _dbContext;
		public UpdatePasswordStorage(MainDbContext context)
		{
            _dbContext = context;
		}

		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
        {
        }
    }

}
