using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class AccountEmailSpecification
{
    private string _message;

    public string Message => _message;

    private static string _idPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    public static readonly Regex EmailRegex = new Regex(
        _idPattern,
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );
    public bool IsSatisfiedBy(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            _message = $"비밀번호는 비어있을 수 없습니다.";
            return false;
        }
        if (!EmailRegex.IsMatch(email))
        {
            _message = $"올바르지 않은 비밀번호 형식입니다.";
            return false;
        }
        return true;
    }
}