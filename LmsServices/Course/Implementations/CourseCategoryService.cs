using LmsEnv;
using LmsModels.Course;
using LmsServices.Common;
using LmsServices.Course.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsServices.Course.Implementations
{
	public class CourseCategoryService: ICourseCategoryService
	{
		private readonly string connString;
		public CourseCategoryService()
		{
			connString = DbConnect.DefaultConnection;
		}

		public void Create(CourseCategoryModel category)
		{
			var parameters = new List<KeyValuePair<string, object>>
			{
				new ("@type", "INSERT"),
				new ("@CourseCategoryId", 0),
				new ("@ParentId", category.ParentId?? (object)DBNull.Value),
				new ("@CourseCategoryName", category.CourseCategoryName),
				new ("@Status", category.Status?? (object)DBNull.Value),
			};

			QueryService.NonQuery("[sp_CreateUpdateDeleteRestore_CourseCategories]", parameters);
		}

		public void Delete(int id)
		{
			throw new NotImplementedException();
		}

		public List<CourseCategoryModel> GetAll()
		{

			return QueryService.Query(
				"sp_GetAll_CourseCategories",
				reader =>
				{
					return new CourseCategoryModel
					{
						CourseCategoryId = reader["CourseCategoryId"] != DBNull.Value ? Convert.ToInt16(reader["CourseCategoryId"]) : (short)0,
						ParentId = reader["ParentId"] != DBNull.Value ? Convert.ToInt16(reader["ParentId"]) : (short?)null, // Nullable short
						ParentCategoryName = reader["ParentCategoryName"] != DBNull.Value ? reader["ParentCategoryName"].ToString() : null,
						CourseCategoryName = reader["CourseCategoryName"].ToString(), // Assuming CourseCategoryName is non-nullable in DB
						Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : null
					};
				});
		}

		public Task<List<CourseCategoryModel>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public CourseCategoryModel GetById(int id)
		{
			throw new NotImplementedException();
		}

		public void Restore(int id)
		{
			throw new NotImplementedException();
		}

		public void ToggleStatus(int id)
		{
			throw new NotImplementedException();
		}

		public void Update(CourseCategoryModel category)
		{
			throw new NotImplementedException();
		}
	}
}
