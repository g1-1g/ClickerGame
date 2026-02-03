using UnityEngine;
using Firebase.Extensions;
using Firebase.Auth;

public class FirebaseTutorial : MonoBehaviour
{
    private Firebase.FirebaseApp _app = null;
    private FirebaseAuth _auth = null;

    void Start()
    {
        
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                // 1. Firebase 초기화 성공했다면 
                _app = Firebase.FirebaseApp.DefaultInstance; // FirebaseApp 모듈 가져오기
                _auth = Firebase.Auth.FirebaseAuth.DefaultInstance; // FirebaseAuth 모듈 가져오기

                Debug.Log("Firebase 초기화 성공");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
        });


    }

    public void Register(string email, string password)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("회원가입이 실패했습니다: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("회원가입에 성공했습니다.: {0} ({1})", result.User.DisplayName, result.User.UserId);
        });
    }

    public void Login(string email, string password)
    {
        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("로그인 실패" + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;

            FirebaseUser resultUser = task.Result.User;
            FirebaseUser user = _auth.CurrentUser;

            Debug.LogFormat("로그인 성공: {0} ({1})",
                result.User.Email, result.User.UserId);
        });
    }

    public void Logout()
    {
        _auth.SignOut();
        Debug.Log("로그아웃 성공");
    }

    public void checkLoginStatus()
    {
        FirebaseUser user = _auth.CurrentUser;
        if (_auth == null)
        {
            Debug.LogFormat("로그인 정보 없음");
        }
        else
        {
            Debug.LogFormat("로그인 중 :{0} ({1})", user.Email, user.UserId);
        }      
    }

    private void Update()
    {
        if (_app == null)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Register("djdjdjd@dndja.com", "dksjlfa");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Login("djdjdjd@dndja.com", "dksjlfa");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Logout();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            checkLoginStatus();
        }
    }

}
