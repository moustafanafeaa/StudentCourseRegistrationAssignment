using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCourseRegistrationAssignment.BLL.StudentServices
{
    public interface IStudentService
    {
        Task<GeneralResponse> GetAllCourses(string studentId);
        Task<GeneralResponse> GetMyCourses(string studentId);
        Task<GeneralResponse> RegisterCourse(string studentId, int courseId);
        Task<GeneralResponse> UnRegisterCourse(string studentId, int courseId);
    }
}
