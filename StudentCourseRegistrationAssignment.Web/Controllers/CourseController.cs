using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentCourseRegistrationAssignment.BLL.CourseServices;
using StudentCourseRegistrationAssignment.BLL.StudentServices;
using StudentCourseRegistrationAssignment.BLL.ViewModels;
using StudentCourseRegistrationAssignment.DAL.Data.Entites;

namespace StudentCourseRegistrationAssignment.Web.Controllers
{
    [Authorize(Roles = "Student")]

    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ICourseService courseService, IStudentService studentService, UserManager<ApplicationUser> userManager)
        {
            _courseService = courseService;
            _studentService = studentService;
            _userManager = userManager;
        }
        private string StudentId =>
            _userManager.GetUserId(User);

      public async Task<IActionResult> Index()
        {
            var studentId = _userManager.GetUserId(User);
            var result = await _studentService.GetAllCourses(studentId);
            return View(result.Model ?? new List<CourseVM>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _courseService.GetById(id);
            return View(result.Model);
        }

        public async Task<IActionResult> MyCourses()
        {
            var result = await _studentService.GetMyCourses(StudentId);
            return View(result.Model ?? new List<CourseVM>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int courseId)
        {
            var studentId = _userManager.GetUserId(User);
            var result = await _studentService.RegisterCourse(studentId, courseId);

            if (!result.Success)
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnRegister(int courseId)
        {
            var studentId = _userManager.GetUserId(User);
            var result = await _studentService.UnRegisterCourse(studentId, courseId);

            if (!result.Success)
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}
