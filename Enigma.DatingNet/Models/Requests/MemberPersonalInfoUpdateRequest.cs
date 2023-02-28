using System.ComponentModel.DataAnnotations;

namespace Enigma.DatingNet.Models.Requests;

public class MemberPersonalInfoUpdateRequest
{
    [Required] public string PersonalInformationId { get; set; }
    [Required] public string Name { get; set; } = null!;
    [Required] public string Gender { get; set; } = null!;
    [Required] public string Bod { get; set; } = null!;
    public IFormFile? ProfilePicture { get; set; }
    public string SelfDescription { get; set; } = string.Empty;
    [Required] public string City { get; set; } = null!;
    public string? MemberId { get; set; }
}