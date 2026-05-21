using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    private static AccountManager _instance;
    public static AccountManager Instance { get { return _instance; } }

    private Account _currentAccount = null;
    private IAccountRepository _repository;
    private bool _isReady = false;

    public bool IsLogin => _currentAccount != null;
    public string Email => _currentAccount?.Email ?? string.Empty;
    public bool IsReady => _isReady;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize(IAccountRepository repository)
    {
        if (_isReady)
        {
            return;
        }

        _repository = repository;
        _isReady = true;
    }

    public async Task<AccountResult> TryLogin(string email, string password)
    {
        if (!_isReady) return new AccountResult()
        {
            Success = false,
            Message = "Service not ready"
        };

        Account account;

        try
        {
            account = new Account(email, password);
        }
        catch (Exception e)
        {
            return new AccountResult()
            {
                Success = false,
                Message = e.Message,
            };
        }

        AccountResult result = await _repository.Login(email, password);

        if (result.Success)
        {
            _currentAccount = account;
            PlayerPrefs.SetString("LastEmail", _currentAccount.Email);

            return new AccountResult
            {
                Success = true,
                Message = result.Message,
            };
        }

        return new AccountResult
        {
            Success = false,
            Message = result.Message,
        };
    }

    public async UniTask<AccountResult> TryRegister(string email, string password)
    {
        if (!_isReady) return new AccountResult()
        {
            Success = false,
            Message = "Service not ready"
        };

        try
        {
            Account account = new Account(email, password);
        }
        catch (Exception e)
        {
            return new AccountResult()
            {
                Success = false,
                Message = e.Message,
            };
        }

        AccountResult result = await _repository.Register(email, password);

        return result;
    }

    public void Logout()
    {
        _repository.Logout();
    }
}
