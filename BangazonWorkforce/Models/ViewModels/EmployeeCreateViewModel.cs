using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace BangazonWorkforce.Models.ViewModels
{
    public class EmployeeCreateViewModel
    {
        public List<SelectListItem> Departments { get; set;  }

        public Employee Employee { get; set; }

        private readonly string _connectionString;

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public EmployeeCreateViewModel() { }

        public EmployeeCreateViewModel(string connectionString)
        {
            _connectionString = connectionString;

            Departments = GetAllDepartments()
                .Select(li => new SelectListItem
                {
                    Text = li.Name,
                    Value = li.Id.ToString()
                })
                .ToList();
                Departments
                .Insert(0, new SelectListItem
                {
                    Text = "Choose cohort...",
                    Value = "0"
                });
        }
        private List<Department> GetAllDepartments()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Department";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Department> departments = new List<Department>();
                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        });
                    }
                    reader.Close();

                    return departments;
                }
            }
        }
    }
}
