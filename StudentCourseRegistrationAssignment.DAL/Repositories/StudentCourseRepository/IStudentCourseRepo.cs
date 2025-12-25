using StudentCourseRegistrationAssignment.DAL.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCourseRegistrationAssignment.DAL.Repositories.StudentCourseRepository
{
    public interface IStudentCourseRepo
    {
        Task RegisterAsync(string studentId, int courseId);
        Task UnRegisterAsync(string studentId, int courseId);
        Task<bool> IsRegisteredAsync(string studentId, int courseId);
        Task<List<Course>> GetMyCoursesAsync(string studentId);
        Task<HashSet<int>> GetCourseIdsByStudentId(string studentId);
    }
}
