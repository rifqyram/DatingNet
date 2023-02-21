using System.ComponentModel.DataAnnotations;

namespace Enigma.DatingNet.Models.Responses;

public class RegisterResponse
{
    public string Username { get; set; } = string.Empty;
}