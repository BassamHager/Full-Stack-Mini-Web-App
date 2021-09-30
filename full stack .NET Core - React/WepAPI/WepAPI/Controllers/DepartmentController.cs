using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

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
			string query = @"
					select DepartmentId, DepartmentName from dbo.Department";

			DataTable table = new DataTable();
			// db connection string
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppcon");
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
	}
}
