using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    [OpenApiIgnore]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly ILogger<ItemsController> logger;

        public ItemsController(IRepository repository, IOptions<SecretSettings> secretSettings, ILogger<ItemsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
            this.logger.LogInformation($"Creating with secretSettings: {secretSettings.Value.ServiceApiKey}");
        }

        [HttpGet]
        [OpenApiIgnore]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IEnumerable<Item> GetItems()
        {
            logger.LogInformation($"{nameof(GetItems)} started");
            var items =  repository.GetItems();
            logger.LogInformation($"{nameof(GetItems)} finished");
            return items;
        }

        [HttpGet("{index}")]
        [OpenApiIgnore]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<Item> GetItemByIndex(int index)
        {
            logger.LogInformation($"{nameof(GetItemByIndex)} id {index}");
            var item = repository.GetItemByIndex(index);
            return item == null ? NotFound() : item;
        }

        [HttpPost]
        [OpenApiIgnore]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<Item> CreateItem(Item item)
        {
            logger.LogInformation($"{nameof(CreateItem)}");
            item.Id = Guid.NewGuid().ToString();
            repository.CreateItem(item);
            return CreatedAtAction(nameof(GetItems), new { id = item.Id }, item);
        }

        [HttpPut]
        [OpenApiIgnore]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult UpdateItem(Item item)
        {
            logger.LogInformation($"{nameof(UpdateItem)}");
            repository.UpdateItem(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [OpenApiIgnore]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult DeleteItem(string id)
        {
            repository.DeleteItem(id);
            return NoContent();
        }

        [HttpGet("throw")]
        [OpenApiIgnore]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult Throw()
        {
            throw new InvalidOperationException("Test exception");
        }

    }
}
