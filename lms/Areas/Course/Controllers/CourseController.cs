using LmsModels.Course;
using LmsServices.Course.Implementations;
using LmsServices.Course.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace lms.Areas.Course.Controllers
{
	[Area("course")]
	public class CourseController : Controller
	{
		ICourseCategoryService _courseCategoryService;
		ICourseService _courseService;
		ICourseModuleService _courseModuleService;

		public CourseController()
		{
			_courseCategoryService = new CourseCategoryService();
			_courseService = new CourseService();
			_courseModuleService = new CourseModuleService();
		}

		public IActionResult Index()
		{
			var courses =  _courseService.GetAll(); // CourseId, CategoryId
			return View(courses); // Pass the resolved list to the view
		}

		public IActionResult Create()
		{
			ViewBag.CourseCategories =  _courseCategoryService.GetAll();
			return View();
		}

		[HttpPost]
		public IActionResult Create(CourseModel course, string feesJsonString)
		{
			_courseService.CreateWithFees(course, feesJsonString);

			TempData["success"] = "Course with fees created successfully!";
			return RedirectToAction("Index");
		}

		public IActionResult Edit(int id)
		{
			ViewBag.CourseCategories = _courseCategoryService.GetAll();
			ViewBag.id = id;
			CourseModel course = _courseService.GetById(id);
			return View(course);
		}

		[HttpPost]
		public IActionResult Edit(CourseModel course)
		{
			_courseService.Update(course);

			TempData["success"] = "Course updated successfully!";
			return RedirectToAction("Index");
		}



		public IActionResult GetIdNameList(int id)
		{
			var courses =  _courseService.GetAll(0, id); // CourseId, CategoryId
			if (courses == null || !courses.Any())
			{
				return Json(new object[] { }); // Return an empty JSON array
			}

			// Select only the CourseId and CourseName
			var idNameList = courses.Select(c => new
			{
				c.CourseId,
				c.CourseName
			}).ToList();
			return Json(idNameList);

		}

		public IActionResult CourseModule()
		{
			var courses =  _courseModuleService.GetAllCourseModules(0, 0);
			return View(courses);
		}


		public IActionResult CreateModule()
		{
			ViewBag.courses =  _courseService.GetAll(); // CourseId, CategoryId

			ViewBag.CourseCategories =  _courseCategoryService.GetAll();

			return View();
		}

		[HttpPost]
		public IActionResult CreateModule(CourseModuleModel courseModule)
		{
			_courseModuleService.CreateCourseModule(courseModule);

			TempData["success"] = "Course Module created successfully!";
			return RedirectToAction("CourseModule");
		}


	}
}
