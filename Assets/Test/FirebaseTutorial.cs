using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine;

public class FirebaseTutorial : MonoBehaviour
{
    private Firebase.FirebaseApp _app = null;
    private FirebaseAuth _auth = null;
    private FirebaseFirestore _db = null;

    [SerializeField]
    private TextMeshProUGUI _progressText;

    private void Start()
    {
        StartAsync().Forget();
    }
    private async UniTask StartAsync()
    {
        await FirebaseInit();
        _progressText.text = "Firebase 초기화 완료";

        Logout();
        _progressText.text = "로그아웃 완료";

        await Login("djdjdjd@dndja.com", "dksjlfa");
        _progressText.text = "로그인 완료";

        await SaveDogs();
        _progressText.text = "추가 완료";

    }

    private async UniTask FirebaseInit()
    {

        var status = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();

        if (status == Firebase.DependencyStatus.Available)
        {
            // 1. Firebase 초기화 성공했다면 
            _app = Firebase.FirebaseApp.DefaultInstance; // FirebaseApp 모듈 가져오기
            _auth = Firebase.Auth.FirebaseAuth.DefaultInstance; // FirebaseAuth 모듈 가져오기
            _db = FirebaseFirestore.DefaultInstance; // Firestore 모듈 가져오기

            Debug.Log("Firebase 초기화 성공");
        }
        else
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {status}");
        }
        
    }

    public async UniTask Register(string email, string password)
    {
        try
        {
            var result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);

            Debug.LogFormat("회원가입에 성공했습니다.: {0} ({1})", result.User.DisplayName, result.User.UserId);
        }
        catch (System.Exception e)
        {
            Debug.LogError("회원가입이 실패했습니다: " + e);
        }  
    }

    public async UniTask Login(string email, string password)
    {
        try
        {
            var result = await _auth.SignInWithEmailAndPasswordAsync(email, password);

            Debug.LogFormat("로그인 성공: {0} ({1})",
                result.User.Email, result.User.UserId);
        }
        catch (System.Exception e)
        {
            Debug.LogError("로그인 실패" + e);
        }  
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

    public async UniTask SaveDogs()
    {
        Dog dog = new Dog("소똥", 3);
        try
        {
            await _db.Collection("Dogs").Document("도").SetAsync(dog);
            Debug.Log("저장 성공: ");
        }
        catch (System.Exception e)
        {
            Debug.LogError("저장 실패: " + e);
        }
    }

    public async UniTask LoadDog()
    {
        try
        {
            var result = _db.Collection("Dogs").Document("개").GetSnapshotAsync();

            DocumentSnapshot snapshot = result.Result;
            if (snapshot.Exists)
            {
                Dog dog = snapshot.ConvertTo<Dog>();
                Debug.LogFormat("불러오기 성공: {0}, {1}", dog.Name, dog.Age);
            }
            else
            {
                Debug.Log("불러오기 실패: 문서가 존재하지 않습니다.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("불러오기 실패: " + e);
            return;
        }
    }

    private void LoadDogs()
    {
        try
        {
            var result = _db.Collection("Dogs").GetSnapshotAsync();

            var snapshots = result.Result;
            foreach (var snapshot in snapshots.Documents)
            {
                Dog dog = snapshot.ConvertTo<Dog>();
                Debug.LogFormat("불러오기 성공: {0}, {1}", dog.Name, dog.Age);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("불러오기 실패: " + e);
            return;
        }
    }

    private void DeleteDog(string name)
    {
        try
        {
            var result = _db.Collection("Dogs").WhereEqualTo("Name", name).GetSnapshotAsync();

            var snapshots = result.Result;
            foreach (var snapshot in snapshots.Documents)
            {
                _db.Collection("Dogs").Document(snapshot.Id).DeleteAsync().ContinueWithOnMainThread(task =>
                {
                    Debug.Log("삭제 성공");
                });
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("불러오기 실패: " + e);
            return;
        }
    }

    private void Update()
    {
        if (_app == null)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Register("djdjdjd@dndja.com", "dksjlfa");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Login("djdjdjd@dndja.com", "dksjlfa");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Logout();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            checkLoginStatus();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SaveDogs();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            LoadDogs();
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            DeleteDog("소똥");
        }
    }

}

