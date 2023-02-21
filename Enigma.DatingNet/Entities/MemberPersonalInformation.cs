using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enigma.DatingNet.Entities;

[Table(name: "m_member_personal_info")]
public class MemberPersonalInformation
{
    [Key, Column(name: "personal_info_id")]
    public Guid PersonalInformationId { get; set; }

    [Column(name: "name")] public string Name { get; set; } = string.Empty;
    [Column(name: "bod")] public DateOnly Bod { get; set; }
    [Column(name: "gender")] public string Gender { get; set; } = string.Empty;
    [Column(name: "self_description")] public string SelfDescription { get; set; } = string.Empty;
    [Column(name: "recent_photo_path")] public string RecentPhotoPath { get; set; } = string.Empty;
    [Column(name: "city")] public string City { get; set; }

    [Column(name: "member_id")] public Guid MemberId { get; set; }
    public MemberUserAccess? Member { get; set; }
}