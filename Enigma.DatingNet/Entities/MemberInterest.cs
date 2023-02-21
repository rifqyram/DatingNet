using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enigma.DatingNet.Entities;

[Table(name: "m_member_interest")]
public class MemberInterest
{
    [Key, Column(name: "member_interest_id")]
    public Guid MemberInterestId { get; set; }

    [Column(name: "interest_id")] public Guid InterestId { get; set; }
    [Column(name: "member_id")] public Guid MemberId { get; set; }

    public MasterInterest? Interest { get; set; }
    public MemberUserAccess? Member { get; set; }
}