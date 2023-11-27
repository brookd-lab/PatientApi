using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

//This is DPAPI Encryption/Decryption
public class Encryption
{
    static string encryptedPwd = "AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAZ6XVUMzAykyRvh+d9syQYQAAAAACAAAAAAAQZgAAAAEAACAAAADd/3sDu7eRsLYV6SuxyAEIGMzLv7yT/3hbUCqiuA8BugAAAAAOgAAAAAIAACAAAADaHtwBvBbjy3h/ZUFVtE8yfRXJCRpqML5uB0XT54LUxyAAAAAijzRd/zBFg9z6fJEo0I/Dz5sZCJCFOcAF6Bz4wfrWK0AAAADuqV+ToMk2DX3cgAuSRahQw3W3eq5bHBd/p6OF2ch9OCirb9YTgJeJG8RiIg+JGtWXQiHCRt4jRxB8purJf0GU"; // pwd = "Test123!";
    static string key = "Atlas"; //Hide this when ready to use
    static DataProtectionScope scope = DataProtectionScope.CurrentUser;//specified in IIS

    static void Main(string[] args)
    {
        Console.WriteLine(Encrypt("user1@test.com"));

        Console.WriteLine(Decrypt(encryptedPwd));
    }

    public static string Encrypt(string textToEncrypt)
    {
        byte[] originalText = Encoding.Unicode.GetBytes(textToEncrypt);
        byte[] entropy = Encoding.Unicode.GetBytes(key);
        string encryptedText = Convert.ToBase64String(ProtectedData.Protect(originalText, entropy, scope));
        return encryptedText;
    }

    public static string Decrypt(string encryptedText)
    {
        byte[] encrypted = Convert.FromBase64String(encryptedText);
        byte[] entropy = Encoding.Unicode.GetBytes(key);
        return Encoding.Unicode.GetString(ProtectedData.Unprotect(encrypted, entropy, scope));
    }

    public static string Protect(string stringToEncrypt, string optionalEntropy, DataProtectionScope scope)
    {
        return Convert.ToBase64String(
            ProtectedData.Protect(
                Encoding.UTF8.GetBytes(stringToEncrypt)
                , optionalEntropy != null ? Encoding.UTF8.GetBytes(optionalEntropy) : null
                , scope));
    }

    public static string Unprotect(string encryptedString, string optionalEntropy, DataProtectionScope scope)
    {
        return Encoding.UTF8.GetString(
            ProtectedData.Unprotect(
                Convert.FromBase64String(encryptedString)
                , optionalEntropy != null ? Encoding.UTF8.GetBytes(optionalEntropy) : null
                , scope));
    }
}
