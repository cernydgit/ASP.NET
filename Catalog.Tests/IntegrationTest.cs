using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;


namespace Catalog.Tests
{
    public class IntegrationTest { }

    public abstract class IntegrationTest<T> where T : DbContext
    {
        protected T DbContext { get; private set; }
        protected WebApplicationFactory<Startup> AppFactory { get; } = new WebApplicationFactory<Startup>();
        protected ILogger<IntegrationTest> Logger { get; private set; }

        [SetUp]
        public async Task SetUp()
        {
            DbContext = CreateDbContext();
            Logger = AppFactory.Services.GetService<ILogger<IntegrationTest>>();
            RecreateDatabase();
            await SeedDatabase();
        }

        private void RecreateDatabase()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Database.Migrate();
            Logger.LogInformation("Applied migrations:\n " + string.Join(",\n", DbContext.Database.GetAppliedMigrations()));
        }

        protected T CreateDbContext() => AppFactory.Services.CreateScope().ServiceProvider.GetService<T>();
        protected abstract Task SeedDatabase();

    }
}