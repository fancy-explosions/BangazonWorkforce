// Author: Billy Mitchell
// This ViewModel is to capture details data of the selected training program, pass the data to the view for the user/requester.

using System.Collections.Generic;
using System.Data.SqlClient;

namespace BangazonWorkforce.Models.ViewModels
{
    public class TrainingProgramDetailsViewModel
    {
        public List<Employee> Employees { get; set; }

        public TrainingProgram trainingProgram { get; set; }

        private string _connectionString;

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public TrainingProgramDetailsViewModel(int id, string connectionString)
        {
            _connectionString = connectionString;

            Employees = GetTrainingProgramEmployees(id);
        }

        private List<Employee> GetTrainingProgramEmployees(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT
                                            e.Id,
                                            e.FirstName,
                                            e.LastName,
                                            e.DepartmentId,
                                            e.IsSuperVisor
                                            FROM Employee e
                                            JOIN EmployeeTraining et ON et.EmployeeId = e.Id
                                            WHERE et.TrainingProgramId = @Id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    List<Employee> employees = new List<Employee>();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                            IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor"))
                        };
                        employees.Add(employee);
                    }
                    reader.Close();

                    return employees;
                }
            }
        }
    }
}
