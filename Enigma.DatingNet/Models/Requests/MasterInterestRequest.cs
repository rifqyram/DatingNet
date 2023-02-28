namespace Enigma.DatingNet.Models.Requests;

public class MasterInterestRequest
{
    public string? MemberId { get; set; }
    public List<string> Interests { get; set; } = null!;
}