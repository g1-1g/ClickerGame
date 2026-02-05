using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;
    public static ItemManager Instance { get { return _instance; } }

    public static event Action<EItemType> OnDataChanged;

    [SerializeField] private ItemSpecTableSO _specTable;

    private Dictionary<EItemType, Item> _items = new();

    private int[] _levels = new int[(int)EItemType.Count];

    private IItemLevelRepository _repository;

    private async void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        // 스펙 데이터에 따라 도메인 생성
        foreach (var specData in _specTable.Datas)
        {
            if (_items.ContainsKey(specData.StatType))
            {
                throw new Exception($"There is already an upgrade with type {specData.StatType}");
            }

            _items.Add(specData.StatType, new Item(specData));

            OnDataChanged?.Invoke(specData.StatType);
        }

        // Firebase 초기화 대기
        await WaitForFirebaseAsync();

        // Repository 생성
        _repository = new FirebaseItemLevelRepository(AccountManager.Instance.Email);

        // 데이터 로드
        await LoadData();
    }

    public Item Get(EItemType type) => _items[type] ?? null;
    public List<Item> GetAll() => _items.Values.ToList();

    
    private async UniTask WaitForFirebaseAsync()
    {
        // FirebaseManager가 준비될 때까지 대기
        while (FirebaseInitializer.Instance == null ||
               !FirebaseInitializer.Instance.IsInitialized || AccountManager.Instance.Email == string.Empty)
        {
            await UniTask.Yield();
        }
    }

    public bool CanLevelUp(EItemType type)
    {
        if (!_items.TryGetValue(type, out Item upgrade))
        {
            return false;
        }

        if (!upgrade.CanLevelUp())
        {
            return false;
        }

        return CurrencyManager.Instance.CanAfford(ECurrencyType.Heart, upgrade.Cost);
    }

    public bool TryLevelUp(EItemType type)
    {
        if (!_items.TryGetValue(type, out Item item))
        {
            return false;
        }

        if (!CurrencyManager.Instance.TrySpend(ECurrencyType.Heart, item.Cost))
        {
            return false;
        }

        if (!item.TryLevelUp())
        {
            return false;
        }

        _levels[(int)type] = item.Level;

        OnDataChanged?.Invoke(type);
        SaveData();

        return true;
    }

    private async UniTask LoadData()
    {
        var loadedData = await _repository.Load();

        if (loadedData == null)
        {
            // 데이터 없으면 기본값으로 초기화
            Debug.Log("[Item] 기본값 생성");
            _levels = ItemLevelSaveData.Default.Levels;

            // 즉시 저장 (다음부터는 로드됨)
            SaveData();
        }
        else
        {
            _levels = loadedData.Levels;
        }

        for (int i = 0; i < _levels.Length; i++)
        {
            _items[(EItemType)i].SetLevel(_levels[i]);
            OnDataChanged?.Invoke((EItemType)i);
        }
        Debug.Log("[Item] 데이터 로드 완료"); 
    }

    private void SaveData()
    {
        ItemLevelSaveData data = new ItemLevelSaveData();
        data.Levels = _levels;

        _repository.Save(data);
    }
}