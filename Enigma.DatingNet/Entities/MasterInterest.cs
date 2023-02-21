using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enigma.DatingNet.Entities;

[Table(name: "m_interest")]
public class MasterInterest
{
    [Key, Column(name: "interest_id")] public Guid InterestId { get; set; }
    [Column(name: "interest")] public string Interest { get; set; } = null!;

    public ICollection<MemberInterest> MemberInterests { get; set; }
}