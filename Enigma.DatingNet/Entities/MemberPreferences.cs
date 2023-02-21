using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enigma.DatingNet.Entities;

[Table(name: "m_member_preferences")]
public class MemberPreferences
{
    [Key, Column(name: "preference_id")] public Guid PreferenceId { get; set; }
    [Column(name: "looking_for_gender")] public string LookingForGender { get; set; } = string.Empty;
    [Column(name: "looking_for_domicile")] public string LookingForDomicile { get; set; } = string.Empty;

    [Column(name: "looking_for_start_age")]
    public int LookingForStartAge { get; set; }

    [Column(name: "looking_for_end_age")] public int LookingForEndAge { get; set; }

    [Column(name: "member_id")] public Guid MemberId { get; set; }
    public MemberUserAccess? Member { get; set; }
}