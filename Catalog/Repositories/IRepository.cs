using Catalog.Entities;
using System;
using System.Collections.Generic;

namespace Catalog.Repositories
{
    public interface IRepository
    {
        Item GetItemByIndex(int index);
        IEnumerable<Item> GetItems();
        void CreateItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(string id);
    }
}