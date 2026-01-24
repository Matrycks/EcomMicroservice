
using CatalogService.Domain;

namespace CatalogService.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public decimal Price { get; private set; }
        public ProductStatus Status { get; private set; }
        public bool InStock { get; set; }

        public Product() { }
        public Product(string name, string desc, decimal price)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(desc) || price <= 0)
                throw new ArgumentException("Cannot create product without required params");

            Name = name;
            Description = desc;
            Price = price;
            Status = ProductStatus.Active;
            InStock = true;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("A product name is required");

            Name = name;
        }

        public void SetDescription(string desc)
        {
            if (string.IsNullOrEmpty(desc))
                throw new ArgumentException("A product desc is required");

            Description = desc;
        }

        public void SetPrice(decimal price)
        {
            if (price <= 0)
                throw new ArgumentException("A product price is required");

            Price = price;
        }

        public void SetStatus(ProductStatus status)
        {
            Status = status;
        }

        public void IsStocked(bool stocked)
        {
            InStock = stocked;
        }
    }
}