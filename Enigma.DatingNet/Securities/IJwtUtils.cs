using Enigma.DatingNet.Entities;

namespace Enigma.DatingNet.Securities;

public interface IJwtUtils
{
    string GenerateToken(MemberUserAccess userAccess);
}