using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WepAPI.models;

namespace WepAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public EmployeeController(IConfiguration configuration) => _configuration = configuration;

		[HttpGet]
		public JsonResult Get()
		{
			string query = @"
						select EmployeeId, EmployeeName, Department, 
						convert(varchar(10),DateOfJoining,120) as DateOfJoining
						,PhotoFileName
						from dbo.Employee
						";

			DataTable table = new DataTable();
			// db connection string
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);

					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult(table);
		}

		[HttpPost]
		public JsonResult Post(Employee emp)
		{
			string query = @"
					insert into dbo.Employee (EmployeeName,Department,DateOfJoining,PhotoFileName)
					values('" + emp.EmployeeName + @"','" + emp.Department + @"','" + emp.DateOfJoining + @"','" + emp.PhotoFileName + @"')
					";

			return SetupRequest(query, "Added Successfully");

		}

		[HttpPut]
		public JsonResult Put(Employee emp)
		{
			string query = @"
					update dbo.Employee set
					EmployeeName = '" + emp.EmployeeName + @"'
					,Department = '" + emp.Department + @"'
					,DateOfJoining = '" + emp.DateOfJoining + @"'
					where EmployeeId = " + emp.EmployeeId + @"
					";

			return SetupRequest(query, "Updated Successfully");
		}

		[HttpDelete("{id}")]
		public JsonResult Delete(int id)
		{
			string query = @"
					delete from dbo.Employee
					where EmployeeId = " + id + @"
					";

			return SetupRequest(query, "Deleted Successfully");
		}

		JsonResult SetupRequest(string query, string msg)
		{
			DataTable table = new DataTable();
			// db connection string
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);

					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult(msg);
		}
	}
}
