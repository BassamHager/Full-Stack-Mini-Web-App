using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using WepAPI.models;

namespace WepAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DepartmentController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public DepartmentController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		public JsonResult Get()
		{
			string query = @"select DepartmentId, DepartmentName from dbo.Department";

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
		public JsonResult Post(Department dep)
		{
			string query = @"
					insert into dbo.Department values('" + dep.DepartmentName + @"')
					";

			return SetupRequest(query, "Added Successfully");

		}

		[HttpPut]
		public JsonResult Put(Department dep)
		{
			string query = @"
					update dbo.Department set
					DepartmentName = '" + dep.DepartmentName + @"'
					where DepartmentId = " + dep.DepartmentId + @"
					";

			return SetupRequest(query, "Updated Successfully");
		}

		[HttpDelete("{id}")]
		public JsonResult Delete(int id)
		{
			string query = @"
					delete from dbo.Department
					where DepartmentId = " + id + @"
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
