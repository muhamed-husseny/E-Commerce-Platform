namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse
    {
        public string? Details { get; set; }

        public ApiExceptionResponse(int statuscode, string? message = null,string details = null)
        {
            Details = details;
        }
    }
}
