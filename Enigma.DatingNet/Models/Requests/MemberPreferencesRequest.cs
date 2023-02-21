using System.ComponentModel.DataAnnotations;
using Enigma.DatingNet.Entities;

namespace Enigma.DatingNet.Models.Requests;

public class MemberPreferencesRequest
{
    [Required] public string LookingForGender { get; set; } = null!;
    [Required] public string LookingForDomicile { get; set; } = null!;
    [Required] public int LookingForStartAge { get; set; }
    [Required] public int LookingForEndAge { get; set; }
    [Required] public string MemberId { get; set; } = null!;
}