using System;
using System.Collections.Generic;

namespace ExploringDapper.Tests
{
    public class Product
    {
        public Product()
        {
            ProductPrices = new List<ProductPrice>();
        }

        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public ProductModel ProductModel { get; set; }
        public List<ProductPrice> ProductPrices { get; set; }
    }

    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductPrice
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double ListPrice { get; set; }
    }
}