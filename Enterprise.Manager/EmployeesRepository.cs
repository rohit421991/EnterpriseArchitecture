using Enterprise.Contract;
using Enterprise.Data.Dtos;
using Enterprise.Data.Entities;
using Enterprise.Manager.EnterpriseDB;
using Microsoft.EntityFrameworkCore;
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
        private readonly EnterpriseFirebirdContext _contextFireBird;
        public EmployeesRepository(EnterpriseFirebirdContext enterpriseFirebirdContext)
        {
            _contextFireBird = enterpriseFirebirdContext;
        }
        public async Task<List<Employees>> GetAllEmployeesAsync()
        {
            return await _contextFireBird.Employees
                    .Select(x => new Employees
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        Mobile = x.Mobile,
                        Address = x.Address
                    }).ToListAsync();
        }

        public async Task<Employees> GetEmployeeAsync(int id)
        {
            try
            {
                var data = await _contextFireBird.Employees.Where(u => u.Id == id)
                    .Select(x => new Employees
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        Mobile = x.Mobile,
                        Address = x.Address
                    }).FirstOrDefaultAsync();

                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SaveEmployeeAsync(Employees modal)
        {
            Enterprise.Manager.EnterpriseDB.Employees employees = new Enterprise.Manager.EnterpriseDB.Employees();

            int newId = (_contextFireBird.Employees.Max(e => (int?)e.Id) ?? 0) + 1;

            employees.Id = newId;
            employees.FirstName = modal.FirstName;
            employees.LastName = modal.LastName;
            employees.Email = modal.Email;
            employees.Mobile = modal.Mobile;
            employees.Address = modal.Address;

            _contextFireBird.Employees.Add(employees);
            await _contextFireBird.SaveChangesAsync();

            return true;
        }
    }
}
