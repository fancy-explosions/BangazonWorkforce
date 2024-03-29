﻿// Author: Billy Mitchell
// The purpose for the Order controller is to define the methods to be used for accessing all tables associated with an order in the BangazonAPI database.
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonWorkforce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonWorkforce.Controllers
{   // Setting the default route, api/order 
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;
        public OrderController(IConfiguration config)
        {
            _config = config;
        }
        // Getting and setting the connection property through the DefaultConnection in appsettings.json which connects this file to the database
        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: api/Order
        // Method to get all Order entries in the database if no query parameter is inputed
        [HttpGet]
        public async Task<IActionResult> Get(string _include, string completed)
        {
            string sql = @"SELECT 
                            o.Id AS OrderId, o.CustomerId, o.PaymentTypeId,
                            
                            p.Id AS ProductId, p.Price, p.Title, p.Description, p.Quantity, p.ProductTypeId, p.CustomerId,
                            pt.Id, pt.Name,
                            c.Id, c.FirstName, c.LastName,
                            payt.Id, payt.AcctNumber, payt.Name, payt.CustomerId
                            FROM [Order] o
                            LEFT JOIN OrderProduct op ON o.Id = op.OrderId
                            LEFT JOIN Product p ON p.Id = op.ProductId
                            LEFT JOIN ProductType pt ON pt.Id = p.ProductTypeId
                            LEFT JOIN Customer c ON c.Id = o.CustomerId
                            LEFT JOIN PaymentType payt ON payt.Id = o.PaymentTypeId
                            ";
            // op.Id, op.OrderId, op.ProductId,

            if (completed == "false")
            {
                sql += " WHERE o.PaymentTypeId IS NULL";
            }
            else if (completed == "true")
            {
                sql += " WHERE o.PaymentTypeId IS NOT NULL";
            }
            else if (completed != null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

         
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    List<Order> orders = new List<Order>();
                    List<Product> products = new List<Product>();

                    while (reader.Read())
                    {
                        Order order = null;

                        order = new Order()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
                               //PaymentTypeId = null
                            };
                        if (!reader.IsDBNull(reader.GetOrdinal("PaymentTypeId")))
                        {
                            order.PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"));
                        }


                        Product product = null;

                        if (_include == "products" && !reader.IsDBNull(reader.GetOrdinal("ProductId")))
                        {
                            product = new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId")),
                                CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                            };
                            products.Add(product);
                        }
                        if (!orders.Any(o => o.Id == order.Id))
                        {
                            orders.Add(order); 
                        }

                        if (_include == "products")
                        {
                            orders.Find(o => o.Id == order.Id).ListOfProducts = products.Where(p => p.CustomerId == order.Id).ToArray();
                        }                       

                    }
                    reader.Close();
                    return Ok(orders);
                }
            }
        }

        // GET: api/Order/5
        // Method to get one Order entry in the database if no query parameter is inputed
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!OrderExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            string sql = "SELECT Id AS OrderId, CustomerId, PaymentTypeId FROM [Order] WHERE Id = @id";
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    Order order = null;

                    if (reader.Read())
                    {
                        order = new Order()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("OrderId")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            PaymentTypeId = reader.GetInt32(reader.GetOrdinal("PaymentTypeId"))
                        };
                    }
                    reader.Close();

                    return Ok(order);
                }
            }
        }

        // POST One Order Entry
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order) // , Product product
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {



                    cmd.CommandText = @"
                        INSERT INTO [Order] (CustomerId, PaymentTypeId)
                        OUTPUT INSERTED.Id
                        VALUES (@CustomerId, @PaymentTypeId)  
                        INSERT INTO OrderProduct(OrderId, ProductId)
                        OUTPUT INSERTED.Id
                        VALUES(@OrderId, @ProductId)
                        ";                       


                    cmd.Parameters.Add(new SqlParameter("@CustomerId", order.CustomerId));
                    cmd.Parameters.Add(new SqlParameter("@PaymentTypeId", order.PaymentTypeId));
                    cmd.Parameters.Add(new SqlParameter("@OrderId", order.Id));
                    cmd.Parameters.Add(new SqlParameter("@ProductId", 2));

                    int NewId = (int)await cmd.ExecuteScalarAsync();
                    order.Id = NewId;
                    return CreatedAtRoute("GetOrder", new { id = NewId }, order);

                }
            }
        }

        // PUT (update) One Order Entry
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Order order)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE [Order]
                                            SET PaymentTypeId = @paymentTypeId                      
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@paymentTypeId", order.PaymentTypeId));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE One Order Entry and all Products associated with 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM OrderProduct WHERE OrderId = @id
                                            DELETE FROM [Order] WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        //This method is used within the get by id, put, and delete methods to make sure the object exists.
        private bool OrderExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id FROM [Order] WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}
