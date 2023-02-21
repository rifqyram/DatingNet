namespace Enigma.DatingNet.Models.Responses;

public class CommonResponse<T> where T : class
{
    public int Code { get; set; }
    public string Status { get; set; } = null!;
    public string Message { get; set; } = null!;
    public T Data { get; set; } = null!;
}