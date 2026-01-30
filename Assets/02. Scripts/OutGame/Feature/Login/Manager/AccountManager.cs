using System;
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

    public bool TryLogin(string email, string password)
    {

        Account account; 

        try
        {
            account = new Account(email, password);
        }
        catch (Exception e)
        {
            return false;
        }

        if (!PlayerPrefs.HasKey($"{email}Hash"))
        {
            return false;
        }

        if (!PasswordHasher.VerifyPassword(password, PlayerPrefs.GetString($"{email}Hash"), PlayerPrefs.GetString($"{email}Salt")))
        {
            return false;
        }

        _currentAccount = account;

        PlayerPrefs.SetString("LastEmail", email);
        return true;
    }

    public bool TryRegister(string email, string password)
    {
        try
        {
            Account account = new Account(email, password);
        }
        catch (Exception e)
        {
            return false;
        }

        if (PlayerPrefs.HasKey(email))
        {
            return false;
        }

        string salt = PasswordHasher.GenerateSalt();
        PlayerPrefs.SetString($"{email}Salt", salt);
        PlayerPrefs.SetString($"{email}Hash", PasswordHasher.HashPassword(password, salt));

        return true;
    }

}
