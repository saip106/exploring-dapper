using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FluentAssertions;
using NUnit.Framework;

namespace ExploringDapper.Tests
{
    [TestFixture]
    public class when_updatinging_single_row_data_using_dapper_execute
    {
        private const string CreateTableSql = "CREATE TABLE Unicorn (Id INTEGER, Name NVARCHAR(50))";
        private const string InsertDataSql = "INSERT INTO Unicorn (Id, Name) VALUES (1, 'foo')";
        private const string SelectDataSql = "SELECT * FROM Unicorn";
        private const string UpdateDataSql = "UPDATE Unicorn SET Name = 'bar' WHERE Id = 1";
        private const string DeleteDataSql = "DELETE FROM Unicorn";
        private const string DropTableDataSql = "DROP TABLE Unicorn";

        [Test]
        public void it_should_update_single_row()
        {
            IList<dynamic> unicorns;
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                sqlConnection.Execute(CreateTableSql);
                sqlConnection.Execute(InsertDataSql);

                unicorns = sqlConnection
                    .Query(SelectDataSql)
                    .ToArray();
            }

            unicorns.Count.Should().Be(1);
            var unicorn = unicorns.ToArray()[0];
            Assert.AreEqual(unicorn.Id, 1);
            Assert.AreEqual(unicorn.Name, "foo");

            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                sqlConnection.Execute(UpdateDataSql);

                unicorns = sqlConnection
                    .Query(SelectDataSql)
                    .ToArray();
            }

            unicorns.Count.Should().Be(1);
            unicorn = unicorns.ToArray()[0];
            Assert.AreEqual(unicorn.Id, 1);
            Assert.AreEqual(unicorn.Name, "bar");

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