using System;

using Cysharp.Threading.Tasks;

using UnityEngine;


public class CatManager : MonoBehaviour
{
    // ===== Singleton =====
    private static CatManager _instance;
    public static CatManager Instance { get { return _instance; } }

    // ===== Serialized Fields =====
    [Header("Cat Database")]
    [SerializeField] private CatsDatabaseSO _catsDatabase;
    [SerializeField] private VFXPlayer _vfxPlayer;
    [SerializeField] private bool _defaultName = true;

    // ===== Private Fields =====
    private ECatType _defaultCatType = ECatType.YellowCat;
    private float _heartsPerClick = 10;
    private CatData _currentCat;
    private CatAnimationPlayer _animationPlayer;
    private CatDatabaseSO _currentCatDatabase;
    private CatData[] _ownedCats = new CatData[(int)ECatType.Count];
    private ICatsRepository _repository;
    private bool _isReady = false;

    // ===== Properties =====
    public Sprite Image => _currentCatDatabase.Image;
    public CatLevelDataSO CurrentLevelData => _currentLevelData;
    public CatData CurrentCat => _currentCat;
    public bool DefaultName => _defaultName;
    public float HeartsPerClick;

    public float AffectionRatio
    {
        get
        {
            if (_currentLevelData == null || _currentLevelData.RequiredAffection == 0)
            {
                return 1;
            }
            return (float)(_currentCat.Affection / _currentLevelData.RequiredAffection);
        }
    }

    private CatLevelDataSO _currentLevelData => _catsDatabase.GetCatLevelData(_currentCat);

    // ===== Events =====
    public event Action OnCatChanged;
    public event Action<CatLevelDataSO> OnLevelChanged;
    public event Action<float> OnAffectionChanged;
    public event Action<String> OnNameChanged;




    private void Awake()
    {
        if (_instance != null || _instance == gameObject)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }
    private async void Start()
    {
        _animationPlayer = GetComponent<CatAnimationPlayer>();
        // Firebase 초기화 대기
        await WaitForFirebaseAsync();

        // Repository 생성
        _repository = new FirebaseCatsRepository(AccountManager.Instance.Email);

        // 데이터 로드
        await LoadData();

        CurrencyManager.OnCurrencyAdded += AffectionUp;
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
            // 즉시 저장 (다음부터는 로드됨)
        }
        else
        {
            _ownedCats = loadedData.OwnedCats;
            SetCat(loadedData.CurrentCatType);
        }

        Debug.Log("[CatManager] 데이터 로드 완료");
    }

    public void SaveData()
    {
        OwnedCatsSaveData saveData = new OwnedCatsSaveData()
        {
            CurrentCatType = _currentCat.CatType,
            OwnedCats = _ownedCats,
        };

        _repository.Save(saveData);
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

    public void SetCat(ECatType catType)
    {
        if (_ownedCats[(int)catType] == null)
        {
            Debug.LogError($"해당 고양이를 소유하고 있지 않습니다.: {catType}");
            return;
        }

        _currentCat = _ownedCats[(int)catType];
        _currentCatDatabase = _catsDatabase.GetCatData(_currentCat);
     
        SaveData();

        OnCatChanged?.Invoke();
        OnLevelChanged?.Invoke(_currentLevelData);
    }

    public void AddCat(ECatType catType)
    {
        if (_ownedCats[(int)catType] != null)
        {
            Debug.LogWarning($"이미 보유 중인 고양이: {catType}");
            return;
        }

        var newCat = new CatData(catType);
        _ownedCats[(int)catType] = newCat;
        TryLevelUp(); // 레벨 1로 설정

        Debug.Log($"새 고양이 획득: {catType}");
    }

    public void SetCatName(String name)
    {
        _currentCat.Name = name;

    }
    public bool TryLevelUp()
    {
        if (_catsDatabase.GetMaxLevel(_currentCat) == _currentCat.Level)
        {
            Debug.Log("이미 최고레벨 입니다.");
            return false;
        }
        _currentCat.Level++;

        _currentCat.Affection = 0;

        OnLevelChanged?.Invoke(_currentLevelData);
        SaveData();

        if (_currentCat.Level == 1) return true;
        PlayLevelUpAnimation();
        PlayLevelUpVFX(transform.position);
        return true;
    }


    public void AffectionUp(double value)
    {
        _currentCat.Affection += value + value * StatManager.Instance.GetStat(EItemType.AffectionGrowthRate);

        if (_currentCat.Affection >= _currentLevelData.RequiredAffection)
        {
            TryLevelUp();
        }

        OnAffectionChanged?.Invoke(AffectionRatio);
        SaveData();
    }
    public void PlayLevelUpVFX(Vector3 position)
    {
        _vfxPlayer.Play(position);
    }

    public void PlayLevelUpAnimation()
    {
        _animationPlayer.LevelUpTrigger();
    }

    public void IncreaseHeartPerClick(float value)
    {
        _heartsPerClick += value;
    }

    public void OnDestroy()
    {
        CurrencyManager.OnCurrencyAdded -= AffectionUp;
    }
}
