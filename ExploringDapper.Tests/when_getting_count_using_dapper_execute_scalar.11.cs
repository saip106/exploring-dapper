using System.Data.SqlClient;
using Dapper;
using FluentAssertions;
using NUnit.Framework;

namespace ExploringDapper.Tests
{
    [TestFixture]
    public class when_getting_count_using_dapper_execute_scalar
    {
        private const string Sql = "SELECT COUNT(*) FROM [HumanResources].[Department]";

        [Test]
        public void it_should_get_department_data()
        {
            int departmentCount;
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                departmentCount = sqlConnection.ExecuteScalar<int>(Sql);
            }

            departmentCount.Should().Be(16);
        }
    }
}
