using System.ComponentModel.DataAnnotations;

namespace Enigma.DatingNet.Models.Responses;

public class LoginResponse
{
    public string Username { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
}