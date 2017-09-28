using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FluentAssertions;
using NUnit.Framework;

namespace ExploringDapper.Tests
{
    [TestFixture]
    public class when_querying_department_data_using_dapper_parameterized_generic_query
    {
        private const string Sql = "SELECT * FROM [HumanResources].[Department] WHERE [DepartmentId] = 1";

        [Test]
        public void it_should_get_department_data()
        {
            IList<Department> departments;
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                departments = sqlConnection
                    .Query<Department>(Sql)
                    .ToArray();
            }

            departments.Count.Should().Be(1);
            departments[0].Name.Should().Be("Engineering");
            departments[0].GroupName.Should().Be("Research and Development");
        }
    }
}