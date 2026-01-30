using System;
using System.Text.RegularExpressions;

// 객체 : 어떤 대상/개념에 대한 속성(데이터) + 기능(메서드)
// 도메인: 어떤 개념에 집중해서 객체로 표현한 것
public class Account 
{
    public readonly string Email;
    public readonly string Password;

    private static string _idPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    private static string _passwordPattern =
        @"^(?=.*[a-z])" +        // 소문자 1개 이상
        @"(?=.*[A-Z])" +         // 대문자 1개 이상
        @"(?=.*[\W_])" +         // 특수문자 1개 이상
        @"[A-Za-z\d\W_]{7,20}$"; // 허용 문자 + 길이

    private static readonly Regex EmailRegex = new Regex(
        _idPattern,
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    private static readonly Regex PasswordRegex = new Regex(
        _passwordPattern,
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public Account(string email, string password)
    {
        if (string.IsNullOrEmpty(email)) throw new ArgumentException($"이메일은 비어있을 수 없습니다.");
        if (!EmailRegex.IsMatch(email)) throw new ArgumentException($"올바르지 않은 이메일 형식입니다.");
        if (string.IsNullOrEmpty(password)) throw new ArgumentException($"비밀번호는 비어있을 수 없습니다.");
        if (!PasswordRegex.IsMatch(password)) throw new ArgumentException($"올바르지 않은 비밀번호 형식입니다.");

        Email = email;
        Password = password;
    }
}
