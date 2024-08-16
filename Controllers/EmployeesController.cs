using ApiDemo.Data;
using ApiDemo.Models;
using ApiDemo.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiDemo.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeesController : ControllerBase
	{
		private readonly ApplicationDbContext dbcontext;

		public EmployeesController(ApplicationDbContext _dbcontext)
        {
			dbcontext = _dbcontext;
		}

        [HttpGet]
		public async Task<IActionResult> GetAllEmployees()
		{
			var allEmployees= await dbcontext.Employees.ToListAsync();

			return Ok(allEmployees);
		}

		[HttpGet]
		[Route("{id:guid}")]
		public async Task<IActionResult> GetEmployeeById(Guid id)
		{
			var employee = await dbcontext.Employees.FindAsync(id);

			if (employee == null)
			{
				return NotFound();
			}
			return Ok(employee);
		}

		[HttpPost]
		public async Task<IActionResult> AddEmployee(AddEmployeeDto addEmployeeDto)
		{
			var employeeEntity = new Employee()
			{
				Name = addEmployeeDto.Name,
				Email = addEmployeeDto.Email,
				Phone = addEmployeeDto.Phone,
				Salary = addEmployeeDto.Salary,
			};

			await dbcontext.Employees.AddAsync(employeeEntity);

			await dbcontext.SaveChangesAsync();

			return Ok(employeeEntity);
		}

		[HttpPut]
		[Route("{id:guid}")]
		public async Task<IActionResult> UpdateEmployee(Guid id,UpdateEmployeeDto updateEmployeeDto)
		{
			var employee = await dbcontext.Employees.FindAsync(id);
			if (employee == null)
			{
				return NotFound();
			}

			employee.Name = updateEmployeeDto.Name;
			employee.Email = updateEmployeeDto.Email;
			employee.Phone = updateEmployeeDto.Phone;
			employee.Salary = updateEmployeeDto.Salary;

			await dbcontext.SaveChangesAsync();

			return Ok(employee);

		}

		[HttpDelete]
		[Route("{id:guid}")]
		public async Task<IActionResult> DeleteEmployee(Guid id)
		{
			var employee = await dbcontext.Employees.FindAsync(id);
			if(employee == null)
			{
				return NotFound();
			}
			
			dbcontext.Employees.Remove(employee);
			await dbcontext.SaveChangesAsync();

			return Ok(employee);
		}
	}
}
