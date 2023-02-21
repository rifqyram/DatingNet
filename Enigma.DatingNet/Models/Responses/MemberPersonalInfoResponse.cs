namespace Enigma.DatingNet.Models.Responses;

public class MemberPersonalInfoResponse
{
    public string MemberPersonalInfoId { get; set; } = string.Empty;
    public string SelfDescription { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateOnly Bod { get; set; }

    public string Gender { get; set; } = string.Empty;
    public string RecentPhotoPath { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}