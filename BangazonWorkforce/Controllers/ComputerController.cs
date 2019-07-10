using System;
using System.Collections.Generic;
using System.Data.SqlClient;
//using System.Linq;
//using System.Threading.Tasks;
using BangazonWorkforce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonWorkforce.Controllers
{
    public class ComputerController : Controller
    {
        private readonly IConfiguration _config;

        public ComputerController(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: Computer
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, 
                                               Make, 
                                               Manufacturer, 
                                               PurchaseDate, 
                                               DecomissionDate 
                                          FROM Computer
                                     ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Computer> computers = new List<Computer>();
                    while (reader.Read())
                    {
                        Computer computer = new Computer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Make = reader.GetString(reader.GetOrdinal("Make")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                            PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate"))
                        };

                        //Since DateTime is a non-nullable data type IsDBNull is required to query the DecomissionDate
                        if (!reader.IsDBNull(reader.GetOrdinal("DecomissionDate")))
                        {
                            computer.DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate"));
                        }
                        computers.Add(computer);
                    }
                    reader.Close();
                    return View(computers);
                }
            }
        }

        // GET: Computer/Details/5
        public ActionResult Details(int id)
        {
            //See GetComputerByID code at the bottom
            Computer computer = GetComputerByID(id);
            return View(computer);
        }

        // GET: Computer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Computer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Computer computer)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Computer (Manufacturer, Make, PurchaseDate)
                                            VALUES (@Manufacturer, @Make, @PurchaseDate)
                                          ";
                        cmd.Parameters.Add(new SqlParameter("@Manufacturer", computer.Manufacturer));
                        cmd.Parameters.Add(new SqlParameter("@Make", computer.Make));
                        cmd.Parameters.Add(new SqlParameter("@PurchaseDate", computer.PurchaseDate));
                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));

                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Computer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Computer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Computer/Delete/5
        public ActionResult Delete(int id)
        {
            Computer computer = GetComputerByID(id);
            return View(computer);
        }

        // POST: Computer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        
                        cmd.CommandText = @"DELETE FROM Computer WHERE Id = @Id
                                          ";

                        cmd.Parameters.Add(new SqlParameter("@Id", id));
                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            catch
            {
                return View();
            }
        }

        //Define GetComputerByID here so that it can be reused to keep the code DRY.
        private Computer GetComputerByID(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id,
                                               Make,
                                               Manufacturer,
                                               PurchaseDate,
                                               DecomissionDate
                                          FROM Computer
                                         WHERE Id = @id
                                      ";

                    //Use the id as a parameter to get details about a specific computer.
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Computer computer = null;
                    if (reader.Read())
                    {
                        computer = new Computer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Make = reader.GetString(reader.GetOrdinal("Make")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                            PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate"))
                        };

                        //Since DateTime is a non-nullable data type IsDBNull is required to query the DecomissionDate
                        if (!reader.IsDBNull(reader.GetOrdinal("DecomissionDate")))
                        {
                            computer.DecomissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate"));
                        }
                    }
                    reader.Close();
                    return computer;
                }
            }
        }
        //private ComputerEmployee IsAssigned(int id)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"SELECT c.Id,
        //                                      ce.ComputerId,
        //                                  FROM Computer c
        //                                  JOIN ComputerEmployee ce ON c.Id = ce.ComputerId
        //                                 WHERE c.Id = @id
        //                              ";

        //            //Use the id as a parameter to get details about a specific computer.
        //            cmd.Parameters.Add(new SqlParameter("@id", id));
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            ComputerEmployee computerEmployee = null;
        //            if (reader.Read())
        //            {
        //                computerEmployee = new ComputerEmployee
        //                {
        //                    Id = reader.GetInt32(reader.GetOrdinal("id")),
        //                    ComputerId = reader.GetInt32(reader.GetOrdinal("ComputerId"))
        //                };
        //            }
        //            reader.Close();
        //            return computerEmployee;
        //        }
        //    }
        //}
    }
}
