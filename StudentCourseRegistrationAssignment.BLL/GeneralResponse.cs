using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentCourseRegistrationAssignment.BLL
{
    public class GeneralResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; }
        public List<string>? Errors { get; set; }
        public object? Model { get; set; }
        public int StatusCode { get; set; }

        public static GeneralResponse SuccessResponse(string message, object? model = null)
        => new() { Success = true, Message = message, Model = model };

        public static GeneralResponse Failure(string message)
            => new() { Success = false, Message = message };
    }
}
