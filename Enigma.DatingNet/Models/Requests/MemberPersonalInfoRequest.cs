using System.ComponentModel.DataAnnotations;
using Enigma.DatingNet.Entities;

namespace Enigma.DatingNet.Models.Requests;

public class MemberPersonalInfoRequest
{
    [Required] public string Name { get; set; } = null!;
    [Required] public string Gender { get; set; } = null!;
    [Required] public string Bod { get; set; } = null!;
    [Required] public IFormFile ProfilePicture { get; set; } = null!;
    public string SelfDescription { get; set; } = string.Empty;
    [Required] public string City { get; set; } = null!;
    public string? MemberId { get; set; }
}