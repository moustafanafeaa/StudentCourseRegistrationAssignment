using StudentCourseRegistrationAssignment.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCourseRegistrationAssignment.BLL.CourseServices
{
    public interface ICourseService
    {
        Task<GeneralResponse> GetAll();
        Task<GeneralResponse> GetById(int id);
        Task<GeneralResponse> Create(CourseVM model);
        Task<GeneralResponse> Update(CourseVM model);
        Task<GeneralResponse> Delete(int id);
    }
}
