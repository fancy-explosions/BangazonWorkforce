// ===============================
// AUTHOR: Sam Britt
// CREATE DATE: 07-11-2019
// PURPOSE: CRUD functionality for Employees
// ===============================

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BangazonWorkforce.Models;
using BangazonWorkforce.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonWorkforce.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _config;

        public EmployeeController(IConfiguration config)
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
        // GET: Employee
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                //open the connection
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //SQL Command
                    cmd.CommandText = @"SELECT e.Id,
                                               e.FirstName,
                                               e.LastName,
                                               e.IsSuperVisor,
                                               e.DepartmentId,
                                               de.Id,
                                               de.Name,
                                               de.Budget
                                        FROM Employee e
                                        LEFT JOIN Department de ON e.DepartmentId = de.Id;";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Employee> employees = new List<Employee>();

                    while (reader.Read())
                    {
                        Employee emp = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor")),
                            department = new Department()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                            }
                        };
                        employees.Add(emp);
                    }
                    reader.Close();

                    return View(employees);
                }
            }
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            Employee emp = GetEmployeeById(id);

            if (emp == null)
            {
                return NotFound();
            }
            else
            {
                EmployeeDetailViewModel EDM = new EmployeeDetailViewModel(id, _config.GetConnectionString("DefaultConnection"));
                EDM.Employee = emp;
                return View(EDM);
            }

        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            EmployeeCreateViewModel employeeCreateViewModel = new EmployeeCreateViewModel(_config.GetConnectionString("DefaultConnection"));
            return View(employeeCreateViewModel);
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeCreateViewModel model)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Employee
                                            (FirstName, LastName, IsSuperVisor, DepartmentId)
                                            VALUES
                                            ( @FirstName, @lastName, @IsSuperVisor, @DepartmentId)";
                        cmd.Parameters.Add(new SqlParameter("@firstName", model.Employee.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", model.Employee.LastName));
                        cmd.Parameters.Add(new SqlParameter("@IsSuperVisor", model.Employee.IsSuperVisor));
                        cmd.Parameters.Add(new SqlParameter("@DepartmentId", model.Employee.DepartmentId));

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

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {

            EmployeeEditViewModel EmpEditViewModel = new EmployeeEditViewModel(_config.GetConnectionString("DefaultConnection"));

            EmpEditViewModel.Employee = GetEmployeeById(id);

            return View(EmpEditViewModel);
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

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EmployeeEditViewModel viewModel)
        {
            Employee emp = viewModel.Employee;

            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Employee
                                        SET 
                                            FirstName = @FirstName,
                                            LastName = @LastName,
                                            DepartmentId = @DepartmentId,
                                            IsSuperVisor = @IsSuperVisor
                                        WHERE Id = @id";

                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        cmd.Parameters.Add(new SqlParameter("@FirstName", emp.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", emp.LastName));
                        cmd.Parameters.Add(new SqlParameter("@DepartmentId", emp.DepartmentId));
                        cmd.Parameters.Add(new SqlParameter("@IsSuperVisor", emp.IsSuperVisor));

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

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private Employee GetEmployeeById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT
                                            Id,
                                            FirstName,
                                            LastName,
                                            DepartmentId,
                                            IsSuperVisor
                                        FROM Employee
                                        WHERE Id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    Employee emp = null;

                    if (reader.Read())
                    {
                        emp = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSuperVisor")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"))

                        };
                    }
                    reader.Close();
                    return emp;
                }

            }
        }
    }
}