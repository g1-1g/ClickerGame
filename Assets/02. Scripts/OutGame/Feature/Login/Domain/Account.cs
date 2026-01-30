using System;
using System.Text.RegularExpressions;

// 객체 : 어떤 대상/개념에 대한 속성(데이터) + 기능(메서드)
// 도메인: 어떤 개념에 집중해서 객체로 표현한 것
public class Account 
{
    public readonly string Email;
    public readonly string Password;


    public Account(string email, string password)
    {
        var AccountSpec = new AccountSpecification();
        if (!AccountSpec.IsSatisfiedEmailBy(email))
        {
            throw new ArgumentException(AccountSpec.Message);
        }

        if (!AccountSpec.IsSatisfiedPasswordBy(password))
        {
            throw new ArgumentException(AccountSpec.Message);
        }

        Email = email;
        Password = password;
    }
}
