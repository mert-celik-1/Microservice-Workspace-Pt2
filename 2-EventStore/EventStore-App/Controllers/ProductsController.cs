using EventStore_App.Models;
using EventStore_Shared.Events;
using EventStore_Shared.Models;
using EventStore_Shared.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;


namespace EventStore_App.Controllers;

public class ProductsController(IEventStoreService eventStoreService, IMongoDBService mongoDBService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var productCollection = mongoDBService.GetCollection<Product>("Products");
            var products = await (await productCollection.FindAsync<Product>(_ => true)).ToListAsync();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM model)
        {
            NewProductAddedEvent newProductAddedEvent = new()
            {
                ProductId = Guid.NewGuid().ToString(),
                InitialCount = model.Count,
                InitialPrice = model.Price,
                IsAvailable = model.IsAvailable,
                ProductName = model.ProductName
            };

            await eventStoreService.AppendToStreamAsync("products-stream", new[] { eventStoreService.GenerateEventData(newProductAddedEvent) });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string productId)
        {
            var productCollection = mongoDBService.GetCollection<Product>("Products");
            var product = await (await productCollection.FindAsync(p => p.Id == productId)).FirstOrDefaultAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> CountUpdate(Product model, int durum)
        {
            var productCollection = mongoDBService.GetCollection<Product>("Products");
            var product = await (await productCollection.FindAsync(p => p.Id == model.Id)).FirstOrDefaultAsync();

            if (durum == 1)
            {
                CountDecreasedEvent countDecreasedEvent = new()
                {
                    ProductId = model.Id,
                    DecrementAmount = model.Count,
                };
                await eventStoreService.AppendToStreamAsync("products-stream", new[] { eventStoreService.GenerateEventData(countDecreasedEvent) });
            }
            else if (durum == 0)
            {
                CountIncreasedEvent countIncreasedEvent = new()
                {
                    ProductId = model.Id,
                    IncrementAmount = model.Count,
                };
                await eventStoreService.AppendToStreamAsync("products-stream", new[] { eventStoreService.GenerateEventData(countIncreasedEvent) });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PriceUpdate(Product model, int durum)
        {
            var productCollection = mongoDBService.GetCollection<Product>("Products");
            var product = await (await productCollection.FindAsync(p => p.Id == model.Id)).FirstOrDefaultAsync();

            if (durum == 1)
            {
                PriceDecreasedEvent priceDecreasedEvent = new()
                {
                    ProductId = model.Id,
                    DecrementAmount = model.Price
                };
                await eventStoreService.AppendToStreamAsync("products-stream", new[] { eventStoreService.GenerateEventData(priceDecreasedEvent) });
            }
            else if (durum == 0)
            {
                PriceIncreasedEvent priceIncreasedEvent = new()
                {
                    ProductId = model.Id,
                    IncrementAmount = model.Price
                };
                await eventStoreService.AppendToStreamAsync("products-stream", new[] { eventStoreService.GenerateEventData(priceIncreasedEvent) });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AvailableUpdate(Product model)
        {
            var productCollection = mongoDBService.GetCollection<Product>("Products");
            var product = await (await productCollection.FindAsync(p => p.Id == model.Id)).FirstOrDefaultAsync();

            if (product.IsAvailable != model.IsAvailable)
            {
                AvailabilityChangedEvent availabilityChangedEvent = new()
                {
                    ProductId = model.Id,
                    IsAvailable = model.IsAvailable,
                };
                await eventStoreService.AppendToStreamAsync("products-stream", new[] { eventStoreService.GenerateEventData(availabilityChangedEvent) });
            }
            return RedirectToAction("Index");
        }
    }