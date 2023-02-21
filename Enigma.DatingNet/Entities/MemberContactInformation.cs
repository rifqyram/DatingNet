using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enigma.DatingNet.Entities;

[Table(name: "m_member_contact_info")]
public class MemberContactInformation
{
    [Key, Column(name: "m_member_contact_id")]
    public Guid MemberContactId { get; set; }

    [Column(name: "mobile_phone_number")] public string MobilePhoneNumber { get; set; } = null!;
    [Column(name: "instagram_id")] public string? InstagramId { get; set; }
    [Column(name: "twitter_id")] public string? TwitterId { get; set; }
    [Column(name: "email")] public string Email { get; set; } = null!;

    [Column(name: "member_id")] public Guid MemberId { get; set; }
    public MemberUserAccess? Member { get; set; }
}