using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Catalog.Tests
{

    public class LogTests
    {
        protected WebApplicationFactory<Startup> AppFactory = new WebApplicationFactory<Startup>();
        ILogger<LogTests> logger;

        [SetUp]
        public async Task SetUp()
        {
            //logger = AppFactory.Services.GetService<ILoggerFactory>().CreateLogger<LogTests>();
            logger = AppFactory.Services.GetService<ILogger<LogTests>>();//.CreateLogger<LogTests>();
        }


        [Test]
        public async Task Log()
        {
            logger.LogInformation("Simple message");
            logger.LogInformation("Parametrized message {Param1}", 123);
            using (logger.BeginScope("ScopeMessage1"))
            {
                logger.LogInformation("Scoped message");
            }
            using (logger.BeginScope(new Dictionary<string, object> { ["TestScope"] = "XXX100" }))
            {
                logger.LogInformation("Hello from XXX");
            }
        }
    }
}