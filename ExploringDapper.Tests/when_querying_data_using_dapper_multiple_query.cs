using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FluentAssertions;
using NUnit.Framework;

namespace ExploringDapper.Tests
{
    [TestFixture]
    public class when_querying_for_grid_data_using_dapper_multiple_query
    {
        private const string Sql = "SELECT * FROM [HumanResources].[Department] WHERE [DepartmentId] = @DepartmentId;" +
                                   "SELECT * FROM [Person].[Person] WHERE BusinessEntityID = @PersonId";

        [Test]
        public void it_should_get_grid_data()
        {
            IList<Department> departments;
            IList<Person> people;
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                var grid = sqlConnection
                    .QueryMultiple(Sql, new { DepartmentId = 1, PersonId = 5 });

                departments = grid.Read<Department>().ToList();
                people = grid.Read<Person>().ToList();
            }

            departments.Count.Should().Be(1);
            departments[0].Name.Should().Be("Engineering");
            departments[0].GroupName.Should().Be("Research and Development");

            people.Count.Should().Be(1);
            people[0].FirstName.Should().Be("Gail");
            people[0].LastName.Should().Be("Erickson");
        }
    }
}