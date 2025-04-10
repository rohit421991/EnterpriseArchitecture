using Enterprise.Contract;
using Enterprise.Data.Dtos;
using Enterprise.Data.Entities;
using Enterprise.Manager.EnterpriseDB;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employees = Enterprise.Data.Entities.Employees;

namespace Enterprise.Manager
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly string _sqlConnectionString;
        public EmployeesRepository(IConfiguration configuration)
        {
            _sqlConnectionString = configuration.GetConnectionString("FirebirdSqlConnection")
                ?? throw new InvalidOperationException("Firebird Sql connection string is missing.");
        }
        public async Task<List<Employees>> GetAllEmployeesAsync()
        {
            var employees = new List<Employees>();

            using (var connection = new FbConnection(_sqlConnectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT \"Id\", \"FIRSTNAME\", \"LASTNAME\", \"EMAIL\", \"MOBILE\", \"ADDRESS\" FROM EMPLOYEES";
                using (var command = new FbCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employees.Add(new Employees
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                Mobile = reader.GetString(4),
                                Address = reader.GetString(5)
                            });
                        }
                    }
                }
            }

            return employees;
        }
        public async Task<Employees> GetEmployeeAsync(int id)
        {
            Employees? employee = null;

            using (var connection = new FbConnection(_sqlConnectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT \"Id\", \"FIRSTNAME\", \"LASTNAME\", \"EMAIL\", \"MOBILE\", \"ADDRESS\" FROM EMPLOYEES WHERE \"Id\" = @Id";
                using (var command = new FbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            employee = new Employees
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Email = reader.GetString(3),
                                Mobile = reader.GetString(4),
                                Address = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return employee ?? throw new KeyNotFoundException("Employee not found.");
        }
        public async Task<bool> SaveEmployeeAsync(Employees modal)
        {
            using (var connection = new FbConnection(_sqlConnectionString))
            {
                await connection.OpenAsync();

                // Get the current maximum Id
                var getMaxIdQuery = "SELECT COALESCE(MAX(\"Id\"), 0) + 1 FROM EMPLOYEES";
                int newId;

                using (var getMaxIdCommand = new FbCommand(getMaxIdQuery, connection))
                {
                    newId = Convert.ToInt32(await getMaxIdCommand.ExecuteScalarAsync());
                }

                var query = @"
                    INSERT INTO EMPLOYEES (""Id"", ""FIRSTNAME"", ""LASTNAME"", ""EMAIL"", ""MOBILE"", ""ADDRESS"")
                    VALUES (@Id, @FirstName, @LastName, @Email, @Mobile, @Address)";

                using (var command = new FbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", newId);
                    command.Parameters.AddWithValue("@FirstName", modal.FirstName);
                    command.Parameters.AddWithValue("@LastName", modal.LastName);
                    command.Parameters.AddWithValue("@Email", modal.Email);
                    command.Parameters.AddWithValue("@Mobile", modal.Mobile);
                    command.Parameters.AddWithValue("@Address", modal.Address);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
