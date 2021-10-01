using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
		private readonly IWebHostEnvironment _env;

		public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
		{
			_configuration = configuration;
			_env = env;

		}

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

		[Route("GetAllDepartmentNames")]
		public JsonResult GetAllDepartmentNames()
		{
			string query = @"
						select DepartmentName from dbo.Department	
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


		[Route("SaveFile")]
		[HttpPost]
		public IActionResult SaveFile()
		{
			try
			{
				var httpRequest = Request?.Form;
				var postedFile = httpRequest?.Files[0];
				var fileName = postedFile?.FileName;
				var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

				using (var stream = new FileStream(physicalPath, FileMode.Create))
				{
					postedFile?.CopyTo(stream);
				}

				return new JsonResult(fileName);

			}
			catch (Exception e)
			{
				return BadRequest();
			}
		}



		// helper function
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
