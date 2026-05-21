using Cysharp.Threading.Tasks;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class AppBootstrapper : MonoBehaviour
{
    private static AppBootstrapper _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Create()
    {
        if (_instance != null)
        {
            return;
        }

        var bootstrapper = new GameObject(nameof(AppBootstrapper));
        bootstrapper.AddComponent<AppBootstrapper>();
    }

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

    private async void Start()
    {
        await WaitForFirebaseAsync();
        await WaitForAccountManagerAsync();

        AccountManager.Instance.Initialize(new FirebaseAccountRepository());
    }

    private async UniTask WaitForFirebaseAsync()
    {
        if (FirebaseInitializer.Instance != null &&
            FirebaseInitializer.Instance.IsInitialized)
        {
            return;
        }

        var completionSource = new UniTaskCompletionSource();

        void OnInitialized()
        {
            completionSource.TrySetResult();
        }

        FirebaseInitializer.OnFirebaseInitialized += OnInitialized;

        try
        {
            if (FirebaseInitializer.Instance != null &&
                FirebaseInitializer.Instance.IsInitialized)
            {
                return;
            }

            await completionSource.Task;
        }
        finally
        {
            FirebaseInitializer.OnFirebaseInitialized -= OnInitialized;
        }
    }

    private async UniTask WaitForAccountManagerAsync()
    {
        while (AccountManager.Instance == null)
        {
            await UniTask.Yield();
        }
    }
}
