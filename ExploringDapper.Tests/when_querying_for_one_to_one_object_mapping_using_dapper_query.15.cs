using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FluentAssertions;
using NUnit.Framework;

namespace ExploringDapper.Tests
{
    [TestFixture]
    public class when_querying_for_one_to_one_object_mapping_using_dapper_query
    {
        private const string Sql = @"SELECT p.ProductID as Id, p.Name, p.ProductNumber, m.ProductModelID as Id, m.Name FROM Production.Product p
                                    INNER JOIN Production.ProductModel m on m.ProductModelID = p.ProductModelID
                                    WHERE ProductID = @ProductID";

        [Test]
        public void it_should_get_one_to_one_object_mapping_data()
        {
            Product[] products;
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                products = sqlConnection
                    .Query<Product, ProductModel, Product>(
                        Sql,
                        (product, productModel) =>
                        {
                            product.ProductModel = productModel;
                            return product;
                        },
                        new { ProductID = 999 })
                    .ToArray();
            }

            products.Length.Should().Be(1);
            products[0].Id.Should().Be(999);
            products[0].ProductNumber.Should().Be("BK-R19B-52");
            products[0].ProductModel.Id.Should().Be(31);
            products[0].ProductModel.Name.Should().Be("Road-750");
        }
    }
}