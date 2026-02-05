using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour
{
    public static FirebaseInitializer Instance { get; private set; }

    private Firebase.FirebaseApp _app = null;
    public FirebaseAuth Auth { get; private set; }
    public FirebaseFirestore Database { get; private set; }
    public bool IsInitialized => _app != null && Auth != null && Database != null;

    public static event Action OnFirebaseInitialized;

    private void Awake()
    {
        if (Instance != null || Instance == this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        FirebaseInit().Forget();
    }

    private async UniTask FirebaseInit()
    {

        var status = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();

        if (status == Firebase.DependencyStatus.Available)
        {
            // 1. Firebase 초기화 성공했다면.
            _app = Firebase.FirebaseApp.DefaultInstance; // FirebaseApp 모듈 가져오기.
            Auth = FirebaseAuth.DefaultInstance; // FirebaseAuth 모듈 가져오기.
            Database = FirebaseFirestore.DefaultInstance; // Firestore 모듈 가져오기.

            Debug.Log("Firebase 초기화 성공");
            OnFirebaseInitialized?.Invoke();
        }
        else
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {status}");
        }
    }



}