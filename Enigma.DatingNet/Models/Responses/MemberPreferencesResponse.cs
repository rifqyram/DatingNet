using Enigma.DatingNet.Entities;

namespace Enigma.DatingNet.Models.Responses;

public class MemberPreferencesResponse
{
    public string PreferenceId { get; set; } = string.Empty;
    public string LookingForGender { get; set; } = string.Empty;
    public string LookingForDomicile { get; set; } = string.Empty;
    public int LookingForStartAge { get; set; }
    public int LookingForEndAge { get; set; }
    public string MemberId { get; set; } = string.Empty;
}