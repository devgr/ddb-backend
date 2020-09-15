using System;

namespace Ddb.Api.Models
{
    public class ErrorResponse
    {
        public string Error { get; set; }
        public string ErrorDetails { get; set; }

        public ErrorResponse(string error, string errorDetails)
        {
            Error = error;
            ErrorDetails = errorDetails;
        }

        public ErrorResponse(Exception ex)
        {
            Error = ex.GetType().FullName;
            ErrorDetails = ex.Message;
        }
    }
}
