namespace Enigma.DatingNet.Models.Responses;

public class MemberInfoResponse
{
    public string MemberId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }
    public string VerificationStatus { get; set; } = string.Empty;
}