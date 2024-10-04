using System.Data;
using LmsEnv;
using LmsModels.Course;
using LmsServices.Common;
using LmsServices.Course.Interfaces;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace LmsServices.Course.Implementations
{
	public  class CourseService: ICourseService
	{
		private readonly string connString;
		public CourseService()
		{
			connString = DbConnect.DefaultConnection;
		}

		public void Create(CourseModel course)
		{
			var parameters = new List<KeyValuePair<string, object>>
			{
				new("@type", "INSERT"),
				new("@CourseId", 0),
				new("@CourseCategoryId", course.CourseCategoryId),
				new("@CourseName", course.CourseName),
				new("@CourseDescription", course.CourseDescription?? ""),
				new("@CourseLevel", course.CourseLevel?? (object)DBNull.Value),
				new("@Status", course.Status),

			};

			QueryService.NonQuery("[sp_CreateUpdateDeleteRestore_Courses]", parameters);
		}


		public void CreateWithFees(CourseModel course, string FeesJsonString)
		{
			var courseFeesList = MapJsonToCourseFeeList(FeesJsonString);

			// Convert the list of CourseFee to DataTable
			DataTable courseFeesTable = new DataTable();
			courseFeesTable.Columns.Add("TotalInstallments", typeof(int));
			courseFeesTable.Columns.Add("FeeAmount", typeof(float));
			courseFeesTable.Columns.Add("GstPercentage", typeof(float));

			foreach (var fee in courseFeesList)
			{
				courseFeesTable.Rows.Add(fee.TotalInstallments, fee.FeeAmount, fee.GstPercentage);
			}


			var parameters = new List<KeyValuePair<string, object>>
			{
				//new KeyValuePair<string, object>("@type", "INSERT"),
				new("@CourseId", 0),
				new("@CourseCategoryId", course.CourseCategoryId),
				new("@CourseName", course.CourseName),
				new("@CourseDescription", course.CourseDescription?? ""),
				new("@CourseLevel", course.CourseLevel?? (object)DBNull.Value),
				new("@Status", course.Status),
				new("@CourseFeesList", courseFeesTable),

			};
			QueryService.NonQuery("[sp_Create_CourseWithFees]", parameters);
		}

		public List<CourseFeeModel> MapJsonToCourseFeeList(string jsonData)
		{
			var courseFeeJsonList = JsonConvert.DeserializeObject<List<CourseFeeJson>>(jsonData);

			var courseFeeList = courseFeeJsonList.Select(json => new CourseFeeModel
			{
				TotalInstallments = json.txtTotalInstallments,
				GstPercentage = json.textGstPercentage,
				FeeAmount = json.txtTotalAmount
			}).ToList();

			return courseFeeList;
		}


		public void Delete(int id)
		{
			var parameters = new List<KeyValuePair<string, object>>
			{
				new("@type", "DELETE"),
				new("@CourseId", id),
				new("@CourseCategoryId", 0),
				new("@CourseName", ""),
				new("@CourseDescription", ""),
				new("@CourseLevel", (object)DBNull.Value),
				new("@Status", (object)DBNull.Value),

			};
			QueryService.NonQuery("[sp_CreateUpdateDeleteRestore_Courses]", parameters);

		}

		public List<CourseModel> GetAll(int courseId = 0, int categoryId = 0)
		{
			return QueryService.Query(
				"sp_GetAll_Course",
				reader =>
				{
					return new CourseModel
					{
						CourseId = Convert.ToInt16(reader["CourseId"]),
						CourseCategoryId = Convert.ToInt16(reader["CourseCategoryId"]),
						CourseCategoryName = reader["CourseCategoryName"].ToString(),
						CourseName = reader["CourseName"].ToString(),
						CourseDescription = reader["CourseDescription"].ToString(),
						CourseLevel = reader["CourseLevel"].ToString(),
						Status = reader["Status"].ToString(),
						CourseOrder = Convert.ToInt16(reader["CourseId"]),
					};
				},
				new SqlParameter("@CourseId", courseId),
				new SqlParameter("@CourseCategoryId", categoryId)
			);
		}


		public List<CourseModel> GetById(int id)
		{
			return QueryService.Query(
				"sp_GetAll_Course",
				reader =>
				{
					return new CourseModel
					{
						CourseId = Convert.ToInt16(reader["CourseId"]),
						CourseCategoryId = Convert.ToInt16(reader["CourseCategoryId"]),
						CourseCategoryName = reader["CourseCategoryName"].ToString(),
						CourseName = reader["CourseName"].ToString(),
						CourseDescription = reader["CourseDescription"].ToString(),
						CourseLevel = reader["CourseLevel"].ToString(),
						Status = reader["Status"].ToString(),
						CourseOrder = Convert.ToInt16(reader["CourseId"]),
					};
				},
				new SqlParameter("@CourseId", id),
				new SqlParameter("@CourseCategoryId", 0)
			);
		}

		public void Restore(int id)
		{
			throw new NotImplementedException();
		}

		public void ToggleStatus(int id)
		{
			throw new NotImplementedException();
		}

		public void Update(CourseModel course)
		{
			throw new NotImplementedException();
		}
	}
}