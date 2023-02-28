using System.Net;

namespace Enigma.DatingNet.Models.Responses;

public class ErrorResponse
{
    public int Code { get; set; } = (int)HttpStatusCode.InternalServerError;
    public string Status { get; set; } = HttpStatusCode.InternalServerError.ToString();
    public string Message { get; set; } = "Internal Server Error";
}