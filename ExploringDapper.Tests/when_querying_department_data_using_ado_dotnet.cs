using System.Collections.Generic;
using System.Data.SqlClient;
using FluentAssertions;
using NUnit.Framework;

namespace ExploringDapper.Tests
{
    [TestFixture]
    public class when_querying_department_data_using_ado_dotnet
    {
        [Test]
        public void it_should_get_the_data()
        {
            var departments = new List<Department>();
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM [AdventureWorks].[HumanResources].[Department] WHERE [DepartmentId] = 1";
                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            var department = new Department
                            {
                                Id = (short) sqlDataReader["DepartmentId"],
                                Name = (string) sqlDataReader["Name"],
                                GroupName = (string) sqlDataReader["GroupName"],
                            };
                            departments.Add(department);
                        }
                    }
                }
            }

            departments.Count.Should().Be(1);
            departments[0].Name.Should().Be("Engineering");
            departments[0].GroupName.Should().Be("Research and Development");
        }
    }
}
