using StudentCourseRegistrationAssignment.BLL.ViewModels;
using StudentCourseRegistrationAssignment.DAL.Data.Entites;
using StudentCourseRegistrationAssignment.DAL.Repositories.CoursesRepository;
using StudentCourseRegistrationAssignment.DAL.Repositories.StudentCourseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCourseRegistrationAssignment.BLL.StudentServices
{
    public class StudentService : IStudentService
    {
        private readonly ICourseRepo _courseRepo;
        private readonly IStudentCourseRepo _studentCourseRepo;

        public StudentService(
            ICourseRepo courseRepo,
            IStudentCourseRepo studentCourseRepo)
        {
            _courseRepo = courseRepo;
            _studentCourseRepo = studentCourseRepo;
        }

        public async Task<GeneralResponse> GetAllCourses(string studentId)
        {
            var courses = await _courseRepo.GetAllAsync();

            var registeredCourseIds = await _studentCourseRepo.GetCourseIdsByStudentId(studentId);

            var data = courses.Select(c => new CourseVM
            {
                Id = c.Id,
                Name = c.Name,
                Credits = c.Credits,
                Semester = c.Semester,
                Description = c.Description,
                IsRegistered = registeredCourseIds.Contains(c.Id)
            }).ToList();

            return GeneralResponse.SuccessResponse("Courses loaded successfully", data);
         }

        public async Task<GeneralResponse> GetMyCourses(string studentId)
        {
            var courses = await _studentCourseRepo.GetMyCoursesAsync(studentId);

            var data = courses.Select(c => new CourseVM
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Credits = c.Credits,
                Semester = c.Semester
            }).ToList();

            return GeneralResponse.SuccessResponse("My courses loaded", data);
        }

        public async Task<GeneralResponse> RegisterCourse(string studentId, int courseId)
        {
            var registered = await _studentCourseRepo.IsRegisteredAsync(studentId, courseId);

            if (registered)
                return GeneralResponse.Failure("Already registered");

           
            await _studentCourseRepo.RegisterAsync(studentId,courseId);

            return GeneralResponse.SuccessResponse("Registered successfully");
        }

        public async Task<GeneralResponse> UnRegisterCourse(string studentId, int courseId)
        {
            var registered = await _studentCourseRepo.IsRegisteredAsync(studentId, courseId);

            if (!registered)
                return GeneralResponse.Failure("You are not registered in this course");


            await _studentCourseRepo.UnRegisterAsync(studentId, courseId);

            return GeneralResponse.SuccessResponse("Course unregistered successfully");
        }
    }
}
