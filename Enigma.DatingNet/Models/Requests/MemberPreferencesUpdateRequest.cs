using System.ComponentModel.DataAnnotations;

namespace Enigma.DatingNet.Models.Requests;

public class MemberPreferencesUpdateRequest
{
    [Required] public string PreferenceId { get; set; } = null!;
    [Required] public string LookingForGender { get; set; } = null!;
    [Required] public string LookingForDomicile { get; set; } = null!;
    [Required] public int LookingForStartAge { get; set; }
    [Required] public int LookingForEndAge { get; set; }
    public string? MemberId { get; set; }

}