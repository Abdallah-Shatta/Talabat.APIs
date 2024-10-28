namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse : ApiResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(string ? details = null) : base(500)
        {
            Details = details;
        }
    }
}
