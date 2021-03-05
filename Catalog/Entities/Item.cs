using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Entities
{
    public class Item 
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Range(0, 10000)]
        public int Price { get; set; } = DateTime.Now.Second;
    }

}
