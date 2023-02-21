using System.ComponentModel.DataAnnotations;

namespace Enigma.DatingNet.Models.Requests;

public class MemberContactInfoRequest
{
    [Required] public string MobilePhoneNumber { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string InstagramId { get; set; } = string.Empty;
    [Required] public string TwitterId { get; set; } = string.Empty;
    [Required] public string MemberId { get; set; } = null!;
}