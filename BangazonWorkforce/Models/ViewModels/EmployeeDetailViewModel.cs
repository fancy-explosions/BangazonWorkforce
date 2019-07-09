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
        public List<TrainingProgram> Programs { get; set; }

        public EmployeeDetailViewModel() { }
        public EmployeeDetailViewModel(int id, string config)
        {
            _connectionString = config;
        }
        private List<TrainingProgram> AttendsTrainingPrograms(int id)
        {
            List<TrainingProgram> TProgram = new List<TrainingProgram>();

            using (SqlConnection conn = Connection)
            {
                //Open connection
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT ee.Id,
                                               ee.EmployeeId,
                                               ee.TrainingProgramId";
                }
            }
        }
    }
}
