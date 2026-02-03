using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;
    public static ItemManager Instance { get { return _instance; } }

    public static event Action<EItemType> OnDataChanged;

    [SerializeField] private ItemSpecTableSO _specTable;

    private Dictionary<EItemType, Item> _items = new();

    private int[] _levels = new int[(int)EItemType.Count];

    private IItemLevelRepository _repository;

    private void Awake()
    {
        if (_instance != null || _instance == gameObject)
        {
            Destroy(this);
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

        _repository = new ItemLevelRepository(AccountManager.Instance.Email);
    }

    public Item Get(EItemType type) => _items[type] ?? null;
    public List<Item> GetAll() => _items.Values.ToList();

    private void Start()
    {
        LoadData();
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

    private void LoadData()
    {
        var loadedData = _repository.Load();

        if (loadedData == null)
        {
            // 데이터 없으면 기본값으로 초기화
            Debug.Log("저장된 데이터 없음 - 기본값 생성");
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
        Debug.Log("데이터 로드 완료"); 
    }

    private void SaveData()
    {
        ItemLevelSaveData data = new ItemLevelSaveData();
        data.Levels = _levels;

        _repository.Save(data);
    }
}