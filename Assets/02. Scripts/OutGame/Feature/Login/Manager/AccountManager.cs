using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class AccountManager : MonoBehaviour
{
    // 로그인씬 (로그인/회원가입) -> 게임씬   

    private static AccountManager _instance;
    public static AccountManager Instance { get { return _instance; } }

    private Account _currentAccount = null;

    public bool IsLogin => _currentAccount != null;

    public string Email => _currentAccount?.Email ?? string.Empty;

    private IAccountRepository _repository;

    private bool _isReady = false;

    public bool IsReady => _isReady;

    private async void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        await WaitForFirebaseAsync();
        _repository = new FirebaseAccountRepository();

        _isReady = true;
    }

    private async UniTask WaitForFirebaseAsync()
    {
        // FirebaseManager가 준비될 때까지 대기
        while (FirebaseInitializer.Instance == null ||
               !FirebaseInitializer.Instance.IsInitialized)
        {
            await UniTask.Yield();
        }
    }

    public async Task<AccountResult> TryLogin(string email, string password)
    {
        if (!_isReady) return new AccountResult()
        {
            Success = false,
            Message = "Service not ready"
        }; ;

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
        else
        {
            return new AccountResult
            {
                Success = false,
                Message = result.Message,
            };
        }
    }

    public async UniTask<AccountResult> TryRegister(string email, string password)
    {
        if (!_isReady) return new AccountResult()
        {
            Success = false,
            Message = "Service not ready"
        }; ;
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
