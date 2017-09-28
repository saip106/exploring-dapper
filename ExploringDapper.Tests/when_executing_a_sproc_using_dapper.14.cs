using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FluentAssertions;
using NUnit.Framework;

namespace ExploringDapper.Tests
{
    [TestFixture]
    public class when_executing_a_sproc_using_dapper
    {
        [Test]
        public void it_should_insert_single_row()
        {
            IList<Employee> employees;
            using (var sqlConnection = new SqlConnection(Constants.ConnectionString))
            {
                employees = sqlConnection
                    .Query<Employee>("uspGetEmployeeManagers", new { BusinessEntityId = 3 }, null, true, null, CommandType.StoredProcedure)
                    .ToList();
            }

            employees.Count.Should().Be(1);
            var employee = employees.ToArray()[0];
            employee.ManagerFirstName.Should().Be("Terri");
            employee.ManagerLastName.Should().Be("Duffy");
        }
    }

    public class Employee
    {
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
    }
}