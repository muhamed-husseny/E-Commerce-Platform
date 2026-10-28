
namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            this.StatusCode = statusCode;

            this.Message = message ?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad Request",
                401 => "Yor are Not Authrized",
                404 => "Resource Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
