using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BangazonWorkforce.Models;

namespace BangazonWorkforce.Models.ViewModels
{

    public class DepartmentEployeeViewModel
    {

        public List<Department> Departments { get; set; }
        public List<Employee> Employees { get; set; }

        private string _connectionString;

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public DepartmentEployeeViewModel(string connectionString)
        {
            _connectionString = connectionString;
            GetAllDepartments();
            GetAllEmployees();
        }

        private void GetAllDepartments()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT
                            Id,
                            Name,
                            Budget,
                            Employee
                        FROM Department";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Departments = new List<Department>();
                    while (reader.Read())
                    {
                        Employees.Add(new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        });
                    }

                    reader.Close();
                }
            }
        }

        private void GetAllEmployees()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT
                            Id,
                            FirstName,
                            LastName,
                            DepartmentId
                        FROM Employee";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Departments = new List<Department>();
                    while (reader.Read())
                    {
                        Departments.Add(new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("FName")),
                            Budget = reader.GetInt32(reader.GetOrdinal("Budget")),
                        });
                    }

                    reader.Close();
                }
            }
        }
    }
}


