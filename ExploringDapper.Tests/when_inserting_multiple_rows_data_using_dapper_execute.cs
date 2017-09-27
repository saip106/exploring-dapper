using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FluentAssertions;
using NUnit.Framework;

namespace ExploringDapper.Tests
{
    [TestFixture]
    public class when_inserting_multiple_rows_data_using_dapper_execute
    {
        private const string CreateTableSql = "CREATE TABLE Unicorn (Id INTEGER, Name NVARCHAR(50))";
        private const string InsertDataSql = "INSERT INTO Unicorn (Id, Name) VALUES (@Id, @Name)";
        private const string SelectDataSql = "SELECT * FROM Unicorn";
        private const string DeleteDataSql = "DELETE FROM Unicorn";
        private const string DropTableDataSql = "DROP TABLE Unicorn";

        [Test]
        public void it_should_insert_multiple_rows()
        {
            IList<dynamic> unicorns;
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                sqlConnection.Execute(CreateTableSql);
                sqlConnection.Execute(InsertDataSql, new []
                {
                    new { Id = 1, Name = "foo" },
                    new { Id = 2, Name = "bar" },
                    new { Id = 3, Name = "baz" },
                });

                unicorns = sqlConnection
                    .Query(SelectDataSql)
                    .ToArray();
            }

            unicorns.Count.Should().Be(3);
            var firstUnicorn = unicorns.ToArray()[0];
            Assert.AreEqual(firstUnicorn.Id, 1);
            Assert.AreEqual(firstUnicorn.Name, "foo");

            var secondUnicorn = unicorns.ToArray()[1];
            Assert.AreEqual(secondUnicorn.Id, 2);
            Assert.AreEqual(secondUnicorn.Name, "bar");

            var thirdUnicorn = unicorns.ToArray()[2];
            Assert.AreEqual(thirdUnicorn.Id, 3);
            Assert.AreEqual(thirdUnicorn.Name, "baz");

            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                sqlConnection.Execute(DeleteDataSql);

                unicorns = sqlConnection
                    .Query(SelectDataSql)
                    .ToArray();
            }

            unicorns.Count.Should().Be(0);
        }

        [TearDown]
        public void Teardown()
        {
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                sqlConnection.Execute(DropTableDataSql);
            }
        }
    }
}