using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforce.Models.ViewModels
{
    public class EmployeeEditViewModel
    {
        private string _connectionString;

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public EmployeeEditViewModel() { }
        public List<int> SelectedValues { get; set; } = new List<int>();
        public List<SelectListItem> AvailableDepartments { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableComputers { get; set; } = new List<SelectListItem>();
        public Employee Employee { get; set; }

        public EmployeeEditViewModel(string connectionString)
        {
            _connectionString = connectionString;

            AvailableDepartments = GetDepartments()
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                }
                    ).ToList();
           AvailableDepartments
                .Insert(0, new SelectListItem
                {
                    Text = "Choose a Department...",
                    Value = "0"
                });

            //AvailableComputers = GetComputers()
            //   .Select(d => new SelectListItem
            //   {
            //       Text = d.Make,
            //       Value = d.Id.ToString()
            //   }
            //       ).ToList();
            //AvailableComputers
            //    .Insert(0, new SelectListItem
            //    {
            //        Text = "Choose a Computer...",
            //        Value = "0"
            //    });
        }
        private List<Department> GetDepartments()
        {
            List<Department> depts = new List<Department>();

            using (SqlConnection conn = Connection)
            {
                //Open connection
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT 
                                            Id,
                                            Name,
                                            Budget
                                        FROM Department;";


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Department department = new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                        };
                        depts.Add(department);

                    }
                    reader.Close();

                    return depts;
                }
            }
        }
        private List<Computer> GetComputers()
        {
            List<Computer> comps = new List<Computer>();

            using (SqlConnection conn = Connection)
            {
                //Open connection
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT 
                                               Id,
                                               PurchaseDate,
                                               DecomissionDate,
                                               Make,
                                               Manufacturer
                                        FROM Computer;";


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Computer computer = new Computer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                            DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate")),
                            Make = reader.GetString(reader.GetOrdinal("Make")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                        };
                        comps.Add(computer);

                    }
                    reader.Close();

                    return comps;
                }
            }
        }
    }
}

