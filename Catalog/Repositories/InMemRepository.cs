using Catalog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repositories
{
    public class InMemRepository : IRepository
    {
        List<Item> items = new List<Item>()
        {
            new Item { Id = Guid.NewGuid().ToString(), Name="i1", Price = 10 },
            new Item { Id = Guid.NewGuid().ToString(), Name="i2", Price = 20 },
        };

        public IEnumerable<Item> GetItems() => items;

        public Item GetItemByIndex(int index) => items[index];
        public void CreateItem(Item item) => items.Add(item);

        public void UpdateItem(Item item)
        {
            var index = items.FindIndex(i => i.Id.Contains(item.Id));
            items[index].Name = item.Name;
            items[index].Price = item.Price;
        }

        public void DeleteItem(string id)
        {
            items.RemoveAll(i => i.Id == id);
            
        }
    }
}
