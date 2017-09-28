using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FluentAssertions;
using NUnit.Framework;

namespace ExploringDapper.Tests
{
    [TestFixture]
    public class when_querying_for_one_to_many_object_mapping_using_dapper_query
    {
        private const string Sql = @"SELECT p.ProductID as Id, p.Name, p.ProductNumber, h.ListPrice, h.StartDate, h.EndDate FROM Production.Product p
                                    INNER JOIN Production.ProductListPriceHistory h on p.ProductID = h.ProductID
                                    WHERE p.ProductID = @ProductID";

        [Test]
        public void it_should_get_one_to_many_object_mapping_data()
        {
            var products = new List<Product>();
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                sqlConnection.Query<Product, ProductPrice, Product>(
                        Sql,
                        (product, productPrice) =>
                        {
                            var selectedProduct = products.FirstOrDefault(x => x.Id == product.Id);
                            if (selectedProduct == null)
                            {
                                products.Add(product);
                                selectedProduct = product;
                            }

                            selectedProduct.ProductPrices.Add(productPrice);
                            return null;
                        },
                        new { ProductID = 717 },
                        null,
                        true,
                        "ListPrice");
            }

            products.Count.Should().Be(1);
            var product1 = products[0];
            product1.Id.Should().Be(717);
            product1.ProductNumber.Should().Be("FR-R92R-62");

            product1.ProductPrices.Count.Should().Be(3);
            product1.ProductPrices[0].ListPrice.Should().Be(1263.4598);
            product1.ProductPrices[1].ListPrice.Should().Be(1301.3636);
            product1.ProductPrices[2].ListPrice.Should().Be(1431.50);
        }
    }
}