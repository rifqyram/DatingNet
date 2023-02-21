using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enigma.DatingNet.Entities;

[Table(name: "m_member_user_access")]
public class MemberUserAccess
{
    [Key, Column(name: "member_id")] public Guid MemberId { get; set; }
    [Column(name: "username")] public string Username { get; set; } = string.Empty;
    [Column(name: "password")] public string Password { get; set; } = string.Empty;
    [Column(name: "join_date")] public DateTime JoinDate { get; set; }
    [Column(name: "verification_status")] public string VerificationStatus { get; set; } = string.Empty;

    public ICollection<MemberInterest>? MemberInterests { get; set; }
    public ICollection<MemberPartner>? Partners { get; set; }
    public ICollection<MemberPartner>? Members { get; set; }
}