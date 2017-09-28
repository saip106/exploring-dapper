using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FluentAssertions;
using NUnit.Framework;

namespace ExploringDapper.Tests
{
    [TestFixture]
    public class when_querying_some_columns_of_department_with_alias_using_dapper_generic_query
    {
        private const string Sql = "SELECT Name AS DepartmentName, GroupName AS DepartmentGroupName FROM [HumanResources].[Department] WHERE [DepartmentId] = 1";

        [Test]
        public void it_should_get_department_data()
        {
            IList<SomeOtherDepartment> departments;
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                departments = sqlConnection
                    .Query<SomeOtherDepartment>(Sql)
                    .ToArray();
            }

            departments.Count.Should().Be(1);
            departments[0].DepartmentName.Should().Be("Engineering");
            departments[0].DepartmentGroupName.Should().Be("Research and Development");
        }
    }

    public class SomeOtherDepartment
    {
        public string DepartmentName { get; set; }
        public string DepartmentGroupName { get; set; }
    }
}