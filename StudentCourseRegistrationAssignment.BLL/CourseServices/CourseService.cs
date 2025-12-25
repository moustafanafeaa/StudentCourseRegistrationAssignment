using StudentCourseRegistrationAssignment.BLL.ViewModels;
using StudentCourseRegistrationAssignment.DAL.Data.Entites;
using StudentCourseRegistrationAssignment.DAL.Repositories.CoursesRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCourseRegistrationAssignment.BLL.CourseServices
{
    public class CourseService : ICourseService
    {

        private readonly ICourseRepo _courseRepo;

        public CourseService(ICourseRepo courseRepo)
        {
            _courseRepo = courseRepo;
        }

        public async Task<GeneralResponse> GetAll()
        {
            var courses = await _courseRepo.GetAllAsync();
     
            var data = courses.Select(c => new CourseVM
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Credits = c.Credits,
                Semester = c.Semester
            }).ToList();

            return GeneralResponse.SuccessResponse("Courses loaded successfully", data);
        }

        public async Task<GeneralResponse> GetById(int id)
        {
            var course = await _courseRepo.GetByIdAsync(id);
            if (course == null)
                return GeneralResponse.Failure("Course not found");

            var vm = new CourseVM
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Credits = course.Credits,
                Semester = course.Semester
            };

            return GeneralResponse.SuccessResponse("Course loaded", vm);
        }

        public async Task<GeneralResponse> Create(CourseVM model)
        {
            var course = new Course
            {
                Name = model.Name,
                Description = model.Description,
                Credits = model.Credits,
                Semester = model.Semester
            };

            await _courseRepo.AddAsync(course);

            return GeneralResponse.SuccessResponse("Course created successfully");
        }

        public async Task<GeneralResponse> Update(CourseVM model)
        {
            var course = await _courseRepo.GetByIdAsync(model.Id);
            if (course == null)
                return GeneralResponse.Failure("Course not found");

            course.Name = model.Name;
            course.Description = model.Description;
            course.Credits = model.Credits;
            course.Semester = model.Semester;

            await _courseRepo.UpdateAsync(course);

            return GeneralResponse.SuccessResponse("Course updated successfully");
        }

        public async Task<GeneralResponse> Delete(int id)
        {
            if (await _courseRepo.HasRegistrationsAsync(id))
                return GeneralResponse.Failure("Cannot delete course with registrations");

            var deleted = await _courseRepo.DeleteAsync(id);
            if (!deleted)
                return GeneralResponse.Failure("Course not found");

            return GeneralResponse.SuccessResponse("Course deleted successfully");
        }
    }
}