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
        [Test]
        public void it_should_get_the_data()
        {
            IEnumerable<dynamic> departments;
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                const string sql = "SELECT * FROM [AdventureWorks].[HumanResources].[Department] WHERE [DepartmentId] = 1";
                departments = sqlConnection.Query(sql).ToArray();
            }

            departments.Count().Should().Be(1);
            var department = departments.ToArray()[0];
            Assert.AreEqual(department.Name, "Engineering");
            Assert.AreEqual(department.GroupName, "Research and Development");
        }
    }
}
