using LmsModels.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsServices.Course.Interfaces
{
	public interface ICourseModuleService
	{
		public void CreateCourseModule(CourseModuleModel courseModule);
		public List<CourseModuleModel> GetAllCourseModules(int CourseModuleId = 0, short CourseId = 0);

	}
}
