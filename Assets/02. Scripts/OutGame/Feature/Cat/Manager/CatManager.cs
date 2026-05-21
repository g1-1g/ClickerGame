using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    private static CatManager _instance;
    public static CatManager Instance { get { return _instance; } }

    [Header("Cat Database")]
    [SerializeField] private CatSpecTableSO _catSpecTable;

    [SerializeField] private bool _defaultName = true;

    private ECatType _defaultCatType = ECatType.YellowCat;
    private Dictionary<ECatType, Cat> _ownedCats = new Dictionary<ECatType, Cat>();
    private CatSaveData[] _ownedCatsData = new CatSaveData[(int)ECatType.Count];
    private Cat _currentCat;
    private ICatsRepository _repository;
    private bool _isReady = false;

    public Cat CurrentCat => _currentCat;

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

    public async UniTask Initialize(ICatsRepository repository)
    {
        if (_isReady)
        {
            return;
        }

        _repository = repository;
        await LoadData();

        _isReady = true;
    }

    public async UniTask LoadData()
    {
        var loadedData = await _repository.Load();

        if (loadedData == null)
        {
            Debug.Log("[CatManager] Create default data");
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

        Debug.Log("[CatManager] Load complete");
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
            Debug.LogError($"Cat is not owned: {catType}");
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
            Debug.LogWarning($"Cat is already owned: {catType}");
            return;
        }

        Cat cat = new Cat(_catSpecTable.GetCatData(catType), new CatSaveData(catType));
        _ownedCats.Add(catType, cat);
        CommitCatToSaveData(cat);

        OnCatAdded?.Invoke(catType);
        Debug.Log($"Cat acquired: {catType}");
    }

    public bool AffectionUp(double amount)
    {
        bool isLevelUp = _currentCat.AffectionUp(amount, StatManager.Instance.GetStat(EItemType.AffectionGrowthRate));
        CommitCatToSaveData(_currentCat);

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

    private void CommitCatToSaveData(Cat cat)
    {
        CatSaveData saveData = new CatSaveData()
        {
            CatType = cat.CatType,
            Name = cat.Name,
            Level = cat.Level,
            Affection = cat.Affection
        };

        _ownedCatsData[(int)cat.CatType] = saveData;
    }
}
