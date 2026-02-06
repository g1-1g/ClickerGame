using Cysharp.Threading.Tasks;
using UnityEngine;

public class LoginTest : MonoBehaviour
{
    private async UniTask StartAsync()
    {
        AccountManager.Instance.Logout();
        await AccountManager.Instance.TryRegister("djdjddfdjd@dndja.com", "dksjlfa1234!");
        await AccountManager.Instance.TryLogin("djdjddfdjd@dndja.com", "dksjlfa1234!");
    }

    private async void Awake()
    {
        // Firebase 초기화 대기
        await WaitForFirebaseAsync();

        StartAsync().Forget();
    }

    private async UniTask WaitForFirebaseAsync()
    {
        // FirebaseManager가 준비될 때까지 대기
        while (FirebaseInitializer.Instance == null ||
               !FirebaseInitializer.Instance.IsInitialized || AccountManager.Instance.IsReady == false)
        {
            await UniTask.Yield();
        }
    }
}
