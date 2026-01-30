using System.Text.RegularExpressions;

public class AccountSpecification
{
    private string _message;

    public string Message => _message;

    private static string _idPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    public static readonly Regex EmailRegex = new Regex(
        _idPattern,
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    private static string _passwordPattern =
        @"^(?=.*[a-z])" +        // 소문자 1개 이상
        @"(?=.*[A-Z])" +         // 대문자 1개 이상
        @"(?=.*[\W_])" +         // 특수문자 1개 이상
        @"[A-Za-z\d\W_]{7,20}$"; // 허용 문자 + 길이

    public static readonly Regex PasswordRegex = new Regex(
        _passwordPattern,
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public bool IsSatisfiedEmailBy(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            _message = $"이메일은 비어있을 수 없습니다.";
            return false;
        }
        if (!EmailRegex.IsMatch(email))
        {
            _message = $"올바르지 않은 이메일 형식입니다.";
            return false;
        }
        return true;
    }

    public bool IsSatisfiedPasswordBy(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            _message = $"비밀번호는 비어있을 수 없습니다.";
            return false;
        }
        if (!EmailRegex.IsMatch(password))
        {
            _message = $"올바르지 않은 비밀번호 형식입니다.";
            return false;
        }
        return true;
    }
}
