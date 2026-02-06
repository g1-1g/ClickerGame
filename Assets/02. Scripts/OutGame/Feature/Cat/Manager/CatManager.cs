using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static UnityEditor.Progress;


public class CatManager : MonoBehaviour
{
    // ===== Singleton =====
    private static CatManager _instance;
    public static CatManager Instance { get { return _instance; } }

    // ===== Serialized Fields =====
    [Header("Cat Database")]
    [SerializeField] private CatSpecTableSO _catSpecTable;

    [SerializeField] private bool _defaultName = true;

    // ===== Private Fields =====
    private ECatType _defaultCatType = ECatType.YellowCat;
    private Dictionary<ECatType, Cat> _ownedCats = new Dictionary<ECatType, Cat>();
    private CatSaveData[] _ownedCatsData = new CatSaveData[(int)ECatType.Count]; 
    private Cat _currentCat;
  
    private ICatsRepository _repository;

    private bool _isReady = false;

    // ===== Properties =====
    public Cat CurrentCat => _currentCat;

    // ===== Events =====
    public static event Action OnCatChanged;
    public static event Action<bool> OnAffectionUp;
    public static event Action<ECatType> OnCatAdded;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    private async void Start()
    {
        // Firebase 초기화 대기
        await WaitForFirebaseAsync();

        // Repository 생성
        _repository = new FirebaseCatsRepository(AccountManager.Instance.Email);

        // 데이터 로드
        await LoadData();
    }

    private async UniTask WaitForFirebaseAsync()
    {
        // FirebaseManager가 준비될 때까지 대기
        while (FirebaseInitializer.Instance == null ||
               !FirebaseInitializer.Instance.IsInitialized || AccountManager.Instance.Email == string.Empty)
        {
            await UniTask.Yield();
        }

        _isReady = true;
    }

    public async UniTask LoadData()
    {
        var loadedData = await _repository.Load();

        if (loadedData == null)
        {
            // 데이터 없으면 기본값으로 초기화
            Debug.Log("[CatManager] 기본값 생성");
            AddCat(_defaultCatType);

            SetCat(_defaultCatType);
        }
        else
        {
            for (int i = 0; i < loadedData.OwnedCats.Length; i++)
            {
                if (loadedData.OwnedCats[i] == null) continue;
                _ownedCats.Add((ECatType)i, new Cat((_catSpecTable.GetCatData((ECatType)i)), loadedData.OwnedCats[i]));
            }

            _currentCat = _ownedCats[loadedData.CurrentCatType];
            _ownedCatsData = loadedData.OwnedCats;
            SetCat(loadedData.CurrentCatType);
        }

        Debug.Log("[CatManager] 데이터 로드 완료");
    }

    public void SaveData()
    {
        if (_currentCat == null) return;

        OwnedCatsSaveData saveData = new OwnedCatsSaveData()
        {
            CurrentCatType = _currentCat.CatType,
            OwnedCats = _ownedCatsData,
        };

        _repository.Save(saveData);
    }

    public void SetCat(ECatType catType)
    {
        if (!_ownedCats.ContainsKey(catType))
        {
            Debug.LogError($"해당 고양이를 소유하고 있지 않습니다.: {catType}");
            return;
        }

        _currentCat = _ownedCats[catType];
     
        SaveData();

        OnCatChanged?.Invoke();
    }

    public void AddCat(ECatType catType)
    {
        if (_ownedCats.ContainsKey(catType))
        {
            Debug.LogWarning($"이미 보유 중인 고양이: {catType}");
            return;
        }

        _ownedCats.Add(catType, new Cat(_catSpecTable.GetCatData(catType), new CatSaveData(catType)));
        _ownedCatsData[(int)catType] = _ownedCats[catType].SaveData;

        OnCatAdded?.Invoke(catType);
        Debug.Log($"새 고양이 획득: {catType}");
    }

    public bool AffectionUp(double amount)
    {
        bool isLevelUp = _currentCat.AffectionUp(amount, StatManager.Instance.GetStat(EItemType.AffectionGrowthRate));
        
        _ownedCatsData[(int)_currentCat.CatType] = _currentCat.SaveData;

        SaveData();

        OnAffectionUp?.Invoke(isLevelUp);

        if (isLevelUp)
        {
            return true;
        }
        
        return false;   
    }

    public void SetName(ECatType catType, string name)
    {
        _ownedCats[catType].SetCatName(name);
        if (_currentCat.CatType == catType)
        {
            OnCatChanged?.Invoke();
        }    
    }
}
