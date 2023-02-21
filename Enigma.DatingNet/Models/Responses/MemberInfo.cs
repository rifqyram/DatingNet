using Enigma.DatingNet.Entities;

namespace Enigma.DatingNet.Models.Responses;

public class MemberInfo
{
    public Guid MemberId { get; set; }
    public MemberPersonalInformation MemberPersonalInformation { get; set; }
    public MemberContactInformation MemberContactInformation { get; set; }
    public MemberPreferences MemberPreferences { get; set; }
    public List<MemberInterest> MemberInterests { get; set; }
}