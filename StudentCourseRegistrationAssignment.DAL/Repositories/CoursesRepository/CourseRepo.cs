using Microsoft.EntityFrameworkCore;
using StudentCourseRegistrationAssignment.DAL.Data.DBHelper;
using StudentCourseRegistrationAssignment.DAL.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCourseRegistrationAssignment.DAL.Repositories.CoursesRepository
{
    public class CourseRepo : ICourseRepo
    {
        private readonly ApplicationDbContext _context;

        public CourseRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllAsync()
            => await _context.Course.AsNoTracking().ToListAsync();
        

        public async Task<Course?> GetByIdAsync(int id)
            => await _context.Course.FirstOrDefaultAsync(z=>z.Id == id);

        public async Task AddAsync(Course course)
        {
            await _context.Course.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Course.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
                return false;

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> HasRegistrationsAsync(int courseId)
             => await _context.StudentCourses.AnyAsync(sc => sc.CourseId == courseId);


    }
}
