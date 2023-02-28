namespace Enigma.DatingNet.Models.Requests;

public class PartnerRequest
{
    public string? MemberId { get; set; }
    public string PartnerId { get; set; } = null!;
}