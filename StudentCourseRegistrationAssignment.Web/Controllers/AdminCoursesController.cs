using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCourseRegistrationAssignment.BLL.CourseServices;
using StudentCourseRegistrationAssignment.BLL.ViewModels;

namespace StudentCourseRegistrationAssignment.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCoursesController : Controller
    {
        private readonly ICourseService _courseService;

        public AdminCoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _courseService.GetAll();

            return View(result.Model ?? new List<CourseVM>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _courseService.Create(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _courseService.GetById(id);
            if (!result.Success)
                return NotFound();

            return View(result.Model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CourseVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _courseService.Update(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _courseService.Delete(id);

            if (!result.Success)
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}
