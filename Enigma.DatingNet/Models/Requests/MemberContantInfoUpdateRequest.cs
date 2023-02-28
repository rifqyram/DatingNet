using System.ComponentModel.DataAnnotations;

namespace Enigma.DatingNet.Models.Requests;

public class MemberContactInfoUpdateRequest
{
    [Required] public string MemberContactId { get; set; } = null!;
    [Required] public string MobilePhoneNumber { get; set; } = null!;
    [Required] public string Email { get; set; } = null!;
    [Required] public string InstagramId { get; set; } = string.Empty;
    [Required] public string TwitterId { get; set; } = string.Empty;
    public string? MemberId { get; set; }

}