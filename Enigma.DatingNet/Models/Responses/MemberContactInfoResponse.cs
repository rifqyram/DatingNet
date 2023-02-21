namespace Enigma.DatingNet.Models.Responses;

public class MemberContactInfoResponse
{
    public string ContactInfoId { get; set; } = string.Empty;
    public string MobilePhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? InstagramId { get; set; } = string.Empty;
    public string? TwitterId { get; set; } = string.Empty;
}