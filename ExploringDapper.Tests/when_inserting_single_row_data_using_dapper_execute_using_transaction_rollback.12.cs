using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FluentAssertions;
using NUnit.Framework;

namespace ExploringDapper.Tests
{
    [TestFixture]
    public class when_inserting_single_row_data_using_dapper_execute_using_transaction_rollback
    {
        private const string CreateTableSql = "CREATE TABLE Unicorn (Id INTEGER, Name NVARCHAR(50))";
        private const string InsertDataSql = "INSERT INTO Unicorn (Id, Name) VALUES (1, 'foo')";
        private const string SelectDataSql = "SELECT * FROM Unicorn";
        private const string DropTableDataSql = "DROP TABLE Unicorn";

        [Test]
        public void it_should_not_insert_any_rows()
        {
            IList<dynamic> unicorns;
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                sqlConnection.Open();
                sqlConnection.Execute(CreateTableSql);

                using (var transaction = sqlConnection.BeginTransaction())
                {
                    sqlConnection.Execute(InsertDataSql, null, transaction);
                    transaction.Rollback();
                }

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