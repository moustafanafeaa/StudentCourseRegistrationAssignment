using Microsoft.EntityFrameworkCore;
using StudentCourseRegistrationAssignment.DAL.Data.DBHelper;
using StudentCourseRegistrationAssignment.DAL.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCourseRegistrationAssignment.DAL.Repositories.StudentCourseRepository
{
    public class StudentCourseRepo : IStudentCourseRepo
    {
        private readonly ApplicationDbContext _context;

        public StudentCourseRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HashSet<int>> GetCourseIdsByStudentId(string studentId)
        {
            return _context.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .Select(sc => sc.CourseId)
                .ToHashSet();
        }

        public async Task<List<Course>> GetMyCoursesAsync(string studentId)
        {
            return await _context.StudentCourses
                .Where(s => s.StudentId == studentId)
               .Select(s => s.Course)
               .ToListAsync();
        }

        public async Task<bool> IsRegisteredAsync(string studentId, int courseId)
        {
            return await _context.StudentCourses.AnyAsync(s =>
                            s.StudentId == studentId &&
                            s.CourseId == courseId);
        }

        public async Task RegisterAsync(string studentId, int courseId)
        {
            var stuCourse = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId,
                RegisteredAt = DateTime.Now,
            };
            _context.StudentCourses.Add(stuCourse);
            await _context.SaveChangesAsync();
        }

        public async Task UnRegisterAsync(string studentId, int courseId)
        {
            var entity = await _context.StudentCourses.FirstOrDefaultAsync(s =>
                                s.StudentId == studentId &&
                                s.CourseId == courseId);

            if (entity == null)
                return;

            _context.StudentCourses.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
