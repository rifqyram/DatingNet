using Enigma.DatingNet.Exceptions;

namespace Enigma.DatingNet.Utils;

public class AuthUtil
{
    public string HashPassword(string plainText)
    {
        var hashPassword = BCrypt.Net.BCrypt.HashPassword(plainText, BCrypt.Net.BCrypt.GenerateSalt());
        return hashPassword;
    }

    public bool Verify(string plainText, string hashPassword)
    {
        var verify = BCrypt.Net.BCrypt.Verify(plainText, hashPassword);
        return verify;
    }
}