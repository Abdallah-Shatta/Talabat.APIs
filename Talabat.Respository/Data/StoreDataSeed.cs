using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Respository.Data
{
    public static class StoreDataSeed
    {
        public static async Task SeedAsync(StoreDbContext dbContext)
        {
            if(dbContext.Brands.Count() == 0)
            {
                var brandsData = File.ReadAllText("../Talabat.Respository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count() > 0)
                {
                    foreach (var brand in brands)
                    {
                        dbContext.Set<ProductBrand>().Add(brand);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (dbContext.Categories.Count() == 0)
            {
                var categoriesData = File.ReadAllText("../Talabat.Respository/Data/DataSeed/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

                if (categories?.Count() > 0)
                {
                    foreach (var category in categories)
                    {
                        dbContext.Set<ProductCategory>().Add(category);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (dbContext.Products.Count() == 0)
            {
                var productsData = File.ReadAllText("../Talabat.Respository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count() > 0)
                {
                    foreach (var product in products)
                    {
                        dbContext.Set<Product>().Add(product);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            
            if (dbContext.DeliveryMethods.Count() == 0)
            {
                var deliveryMethodsData = File.ReadAllText("../Talabat.Respository/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                if (deliveryMethods?.Count() > 0)
                {
                    foreach (var method in deliveryMethods)
                    {
                        dbContext.Set<DeliveryMethod>().Add(method);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
