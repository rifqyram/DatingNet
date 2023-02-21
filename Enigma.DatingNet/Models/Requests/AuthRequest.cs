using System.ComponentModel.DataAnnotations;

namespace Enigma.DatingNet.Models.Requests;

public class AuthRequest
{
    [Required] public string Username { get; set; } = null!;

    [Required, StringLength(int.MaxValue, MinimumLength = 8)]
    public string Password { get; set; } = null!;
}