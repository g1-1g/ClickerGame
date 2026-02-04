using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseAccountRepository : IAccountRepository
{

    FirebaseAuth _auth;

    public FirebaseAccountRepository()
    {
        _auth = FirebaseInitializer.Instance.Auth;
    }
    public async UniTask<bool> IsEmailAvailable(string email)
    {
        try
        {
            var result = await _auth.FetchProvidersForEmailAsync(email);

            bool isRegistered = result != null && result.Count() > 0;

            if (isRegistered)
            {
                return false; // 이미 등록된 이메일
            }else
            {
                return true; // 사용 가능한 이메일
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("이메일 조회 실패" + e);
            return false;
        }
    }

    public async UniTask<AccountResult> Login(string email, string password)
    {

        try
        {
            var result = await _auth.SignInWithEmailAndPasswordAsync(email, password);

            Debug.Log("로그인 성공");

            return new AccountResult()
            {
                Success = true,
                Message = "",
            };
        }
        catch (System.Exception e)
        {
            Debug.LogError("로그인 실패" + e);
            return new AccountResult()
            {
                Success = false,
                Message = "로그인에 실패하였습니다.",
            };
        }
    }

    public void Logout()
    {
        _auth.SignOut();
        Debug.Log("로그아웃 되었습니다");
    }

    public async UniTask<AccountResult> Register(string email, string password)
    {
        var isEmailAvailable = await IsEmailAvailable(email);
        if (!isEmailAvailable)
        {
            Debug.LogError("이미 존재하는 계정입니다");
            return new AccountResult()
            {
                Success = false,
                Message = "이미 존재하는 계정입니다.",
            };
        }

        try
        {
            var result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);

            Debug.LogFormat("회원가입에 성공했습니다.: {0} ({1})", result.User.DisplayName, result.User.UserId);
            return new AccountResult()
            {
                Success = true,
                Message = "",
            };
        }
        catch (System.Exception e)
        {
            Debug.LogError("회원가입이 실패했습니다: " + e);
            return new AccountResult()
            {
                Success = false,
                Message = "회원가입이 실패했습니다: " + e,
            };
        }
    }
}