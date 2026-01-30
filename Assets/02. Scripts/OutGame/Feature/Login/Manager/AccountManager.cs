using System;
using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;


public class AccountManager : MonoBehaviour
{
    // 로그인씬 (로그인/회원가입) -> 게임씬   

    private static AccountManager _instance;
    public static AccountManager Instance { get { return _instance; } }

    private Account _currentAccount = null;

    public bool IsLogin => _currentAccount != null;

    public string Email => _currentAccount?.Email ?? string.Empty;



    private void Awake()
    {
        if (_instance != null || _instance == gameObject)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public AuthResult TryLogin(string email, string password)
    {

        Account account; 

        try
        {
            account = new Account(email, password);
        }
        catch (Exception e)
        {
            return new AuthResult()
            {
                Success = false,
                Message = e.Message,
            };
        }

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

        _currentAccount = account;

        PlayerPrefs.SetString("LastEmail", email);
        return new AuthResult()
        {
            Success = true,
            Message = "",
        };
    }

    public AuthResult TryRegister(string email, string password)
    {
        try
        {
            Account account = new Account(email, password);
        }
        catch (Exception e)
        {
            return new AuthResult()
            {
                Success = false,
                Message = e.Message,
            };
        }

        if (PlayerPrefs.HasKey($"{email}Hash"))
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

}
