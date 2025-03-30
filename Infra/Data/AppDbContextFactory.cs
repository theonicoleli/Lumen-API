using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Infra.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=localhost;Port=3306;Database=lumen_api;User=root;Password=PUC@1234;";
            var builder = new DbContextOptionsBuilder<AppDbContext>();

            builder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 21))
            );

            return new AppDbContext(builder.Options);
        }
    }
}
