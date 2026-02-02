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

    private LocalAccountRepository _repository;


    private void Awake()
    {
        if (_instance != null || _instance == gameObject)
        {
            Destroy(this);
            return;
        }
        _instance = this;

        _repository = new LocalAccountRepository();
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
        
        AuthResult result = _repository.Login(email, password);

        if (result.Success)
        {
            _currentAccount = account;
            return new AuthResult
            {
                Success = true,
                Message = result.Message,
            };
        }
        else
        {
            return new AuthResult
            {
                Success = false,
                Message = result.Message,
            };
        }
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

        AuthResult result = _repository.Register(email, password);


        if (result.Success)
        {
            string salt = PasswordHasher.GenerateSalt();
            PlayerPrefs.SetString($"{email}Salt", salt);
            PlayerPrefs.SetString($"{email}Hash", PasswordHasher.HashPassword(password, salt));

            return new AuthResult
            {
                Success = true,
                Message = result.Message
            };
        }
        else
        {
            return new AuthResult
            {
                Success = false,
                Message = result.Message,
            };
        }
    }
}
