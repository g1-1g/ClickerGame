using System;
using UnityEngine;

public class LocalAccountRepository : ILoginRepository
{
    public AuthResult Login(string email, string password)
    {
        if (!PlayerPrefs.HasKey($"{email}Hash"))
        {
            return new AuthResult()
            {
                Success = false,
                Message = "존재하지 않는 이메일입니다..",
            };
        }

        if (!PasswordHasher.VerifyPassword(password, PlayerPrefs.GetString($"{email}Hash"), PlayerPrefs.GetString($"{email}Salt")))
        {
            return new AuthResult()
            {
                Success = false,
                Message = "이메일과 비밀번호가 일치하지 않습니다.",
            };
        }

        PlayerPrefs.SetString("LastEmail", email);
        return new AuthResult()
        {
            Success = true,
            Message = "",
        };
    }

    public AuthResult Register(string email, string password)
    {
        if (IsEmailAvailable(email))
        {
            return new AuthResult()
            {
                Success = false,
                Message = "이미 존재하는 계정입니다.",
            };
        }

        string salt = PasswordHasher.GenerateSalt();
        PlayerPrefs.SetString($"{email}Salt", salt);
        PlayerPrefs.SetString($"{email}Hash", PasswordHasher.HashPassword(password, salt));

        return new AuthResult()
        {
            Success = true,
            Message = "",
        };
    }


    public void Logout()
    {
        Debug.Log("로그아웃 되었습니다.");
    }

    public bool IsEmailAvailable(string email)
    {

        if (PlayerPrefs.HasKey($"{email}Hash"))
        {
            return true;
        }
        return false;
    }
}
