// Models/RepositoryResult.cs
using System.Net;

namespace EmployeeAssignments.API.Models
{
    public class RepositoryResult<T>
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static RepositoryResult<T> Ok(T data, string? message = null) =>
            new() { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = message, Data = data };

        public static RepositoryResult<T> Error(HttpStatusCode statusCode, string message) =>
            new() { IsSuccess = false, StatusCode = statusCode, Message = message };
    }
}
