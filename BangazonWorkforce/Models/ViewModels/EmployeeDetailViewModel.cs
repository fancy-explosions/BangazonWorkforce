using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforce.Models.ViewModels
{
    public class EmployeeDetailViewModel
    {
        private string _connectionString;

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public Employee Employee { get; set; }
        public Computer Computer { get; set; }
        public Department Department { get; set; }

        public List<TrainingProgram> Programs { get; set; }

        public EmployeeDetailViewModel() { }
        public EmployeeDetailViewModel(int id, string config)
        {
            _connectionString = config;
            Programs = GetEmployeeTrainingPrograms(id);
            Department = GetDepartment(id);
            Computer = GetComputer(id);

        }
        private List<TrainingProgram> GetEmployeeTrainingPrograms(int id)
        {
            List<TrainingProgram> TPrograms = new List<TrainingProgram>();

            using (SqlConnection conn = Connection)
            {
                //Open connection
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT 
                                            tp.Id,
                                            tp.Name,
                                            tp.StartDate,
                                            tp.EndDate,
                                            tp.MaxAttendees,
                                        FROM TrainingProgram tp
                                        JOIN EmployeeTraining et ON et.TrainingProgramId = tp.Id
                                        JOIN Employee e ON e.Id = et.EmployeeId
                                        WHERE e.Id = @id
                                        ";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        TrainingProgram program = new TrainingProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                            MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees"))
                        };
                        TPrograms.Add(program);

                    }
                    reader.Close();

                    return TPrograms;
                }
            }
        }
        private Computer GetComputer(int id)
        {
            
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id,
                                               c.PurchaseDate,
                                               c.DecomissionDate,
                                               c.Make,
                                               c.Manufacturer
                                        FROM Computer c
                                        JOIN EmployeeComputer ec ON ec.ComputerId = c.Id
                                        JOIN Employee e ON e.Id = ec.EmployeeId
                                        WHERE e.Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    Computer comp = null;
                    while (reader.Read())
                    {
                        comp = new Computer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                            DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate")),
                            Make = reader.GetString(reader.GetOrdinal("Make")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                        };
                    }
                    reader.Close();
                    return comp;
                }
               

            }

        }
        private Department GetDepartment(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT 
                                            d.Id,
                                            d.Name,
                                            d.Budget
                                        FROM Department d
                                        JOIN Employee e ON d.Id = e.DepartmentId
                                        WHERE e.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    Department dept = null;

                    while (reader.Read())
                    {
                        dept = new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                        };

                    }
                    reader.Close();
                    return dept;
                }
            }
        }
    }
}
