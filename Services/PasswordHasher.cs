using BCryptNet = BCrypt.Net.BCrypt;

namespace LagalerieFurniture.Services;

/// <summary>
/// خدمة تشفير كلمات المرور باستخدام BCrypt (hash + salt مدمج).
/// BCrypt بيخزّن الـ salt جوه الـ hash نفسه، فمش محتاجين عمود منفصل للـ salt.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>تشفير كلمة المرور الصريحة وإرجاع الـ hash.</summary>
    string Hash(string password);

    /// <summary>التحقق من تطابق كلمة المرور مع الـ hash المخزّن.</summary>
    bool Verify(string password, string hashedPassword);

    /// <summary>التحقق هل النص المخزّن هو BCrypt hash صالح (يبدأ بـ $2).</summary>
    bool IsBCryptHash(string value);
}

public class PasswordHasher : IPasswordHasher
{
    // work factor = 12: توازن جيد بين الأمان والأداء (سنة 2026).
    private const int WorkFactor = 12;

    public string Hash(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("كلمة المرور لا يمكن أن تكون فارغة", nameof(password));

        return BCryptNet.HashPassword(password, WorkFactor);
    }

    public bool Verify(string password, string hashedPassword)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
            return false;

        try
        {
            return BCryptNet.Verify(password, hashedPassword);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            // الـ hash المخزّن مش بصيغة BCrypt (ممكن يكون نص صريح قديم)
            return false;
        }
    }

    public bool IsBCryptHash(string value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        // BCrypt hashes دايماً بتبدأ بـ $2a$, $2b$, أو $2y$ وطولها 60 حرف
        return value.Length == 60 && value.StartsWith("$2");
    }
}
