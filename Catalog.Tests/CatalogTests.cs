using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.Threading.Tasks;
using Catalog.Controllers;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Catalog.Entities;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using Google.Cloud.BigQuery.V2;
using System.Linq;

namespace Catalog.Tests
{


    public class CatalogTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetItemsUrl()
        {
            var factory = new WebApplicationFactory<Startup>();
            await Task.WhenAll(Enumerable.Range(0, 10).Select(async _ =>
             {
                 var client = factory.CreateClient();
                 var response = await client.GetAsync("/api/Items");
                 response.EnsureSuccessStatusCode();
                 var json = await response.Content.ReadAsStringAsync();
             }));
        }

        [Test]
        public void GetItemsDirect()
        {
            var factory = new WebApplicationFactory<Startup>();
            //var ctrl = factory.Services.GetService<IControllerFactory>().CreateController(new ControllerContext(new ;
            //var result = ctrl.GetItems();
        }

        [TestCase("mongodb+srv://sa:sa@cluster0.owceu.mongodb.net/test")]
        [TestCase("mongodb+srv://user1:Heslo3799@cluster0.owceu.mongodb.net/test")]
        [TestCase("mongodb://localhost:27017")]
        public void MongoTest(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var items = client.GetDatabase("catalog").GetCollection<Item>("items");
            Builders<Item>.IndexKeys.Ascending(i => i.Price);
            items.Indexes.CreateOne(new CreateIndexModel<Item>(Builders<Item>.IndexKeys.Ascending(i => i.Price)));
            var indexes = items.Indexes.List().ToList();
            Console.WriteLine(indexes.ToJson());
            
            for (int i=0; i< 100;i++)
            {
                items.InsertOne(new Item());
            }
        }


        [Test]
        public void BigQueryTest()
        {
            //https://console.cloud.google.com/bigquery?project=emerald-energy-306017&p=emerald-energy-306017&page=dataset&d=ds1
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\!Code\ASP.NET\google_bigquery_keys.json");
            var client = BigQueryClient.Create("emerald-energy-306017");
            var table = client.GetTable("bigquery-public-data", "samples", "shakespeare");
            var sql = $"SELECT corpus AS title, COUNT(word) AS unique_words FROM {table} GROUP BY title ORDER BY unique_words DESC LIMIT 10";

            var results = client.ExecuteQuery(sql, parameters: null);

            foreach (var row in results)
            {
                Console.WriteLine($"{row["title"]}: {row["unique_words"]}");
            }

            table = client.GetTable("emerald-energy-306017", "ds1", "Table1");
            //table = client.GetTable("bigquery-public-data", "bls", "wm");
            sql = $"SELECT * FROM {table} LIMIT 10";
            results = client.ExecuteQuery(sql, parameters: null);

            foreach (var row in results)
            {
                Console.WriteLine($"{row}");
            }
        }
    }

    public static class JsonExtensions
    {
        public static string ToJson(this object x) => JsonConvert.SerializeObject(x);
    }
}