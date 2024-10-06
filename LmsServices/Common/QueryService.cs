using LmsEnv;
using Microsoft.Data.SqlClient;
using System.Data;
	
namespace LmsServices.Common
{
	public static class QueryService
	{

        public static bool NonQuery(string procedure, List<KeyValuePair<string, object>> parameters)
		{
			string connString = DbConnect.DefaultConnection;

			using (SqlConnection connection = new SqlConnection(connString))
			{
				using (SqlCommand command = new SqlCommand($"{procedure}", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					foreach (var param in parameters)
					{
						command.Parameters.Add(new SqlParameter(param.Key, param.Value));
					}

					connection.Open();
					int rowsAffected = command.ExecuteNonQuery();
					if (rowsAffected > 0)
					{
						return true;
					}
					return false;
				}

			}
		}

		public static List<T> Query<T>(string spName,  Func<SqlDataReader, T> mapFunction, params SqlParameter[] parameters)
		{
			string connString = DbConnect.DefaultConnection;

			List<T> results = new List<T>();
			using (SqlConnection con = new SqlConnection(connString))
			{
				con.Open();
				using (SqlCommand cmd = new SqlCommand(spName, con))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					// Add parameters to command
					if (parameters != null)
					{
						cmd.Parameters.AddRange(parameters);
					}

					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							// Map each row to the desired model using the provided mapping function
							T item = mapFunction(dr);
							results.Add(item);
						}
					}
				}
			}
			return results;
		}


	}
}
