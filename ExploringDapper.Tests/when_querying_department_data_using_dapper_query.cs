using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FluentAssertions;
using NUnit.Framework;

namespace ExploringDapper.Tests
{
    [TestFixture]
    public class when_querying_department_data_using_dapper_query
    {
        private const string Sql = "SELECT * FROM [HumanResources].[Department] WHERE [DepartmentId] = 1";

        [Test]
        public void it_should_get_department_data()
        {
            IEnumerable<dynamic> departments;
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                departments = sqlConnection
                    .Query(Sql)
                    .ToArray();
            }

            departments.Count().Should().Be(1);
            var department = departments.ToArray()[0];
            Assert.AreEqual(department.Name, "Engineering");
            Assert.AreEqual(department.GroupName, "Research and Development");
        }
    }
}
