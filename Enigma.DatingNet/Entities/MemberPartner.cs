using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enigma.DatingNet.Entities;

[Table(name: "m_member_partner")]
public class MemberPartner
{
    [Key, Column(name: "member_partner_id")]
    public Guid MemberPartnerId { get; set; }
    [Column(name:"member_id")]
    public Guid MemberId { get; set; }
    [Column(name: "partner_id")]
    public Guid PartnerId { get; set; }

    public MemberUserAccess? Member { get; set; }
    public MemberUserAccess? Partner { get; set; }
}