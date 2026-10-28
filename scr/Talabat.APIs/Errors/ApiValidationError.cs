namespace Talabat.APIs.Errors
{
    public class ApiValidationError : ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationError() : base(400)
        {
            Errors = new List<string>();   
        }
    }
}
