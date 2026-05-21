using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        foreach (var specData in _specTable.Datas)
        {
            if (_items.ContainsKey(specData.StatType))
            {
                throw new Exception($"There is already an upgrade with type {specData.StatType}");
            }

            _items.Add(specData.StatType, new Item(specData));

            OnDataChanged?.Invoke(specData.StatType);
        }
    }

    public async UniTask Initialize(IItemLevelRepository repository)
    {
        if (_repository != null)
        {
            return;
        }

        _repository = repository;
        await LoadData();
    }

    public Item Get(EItemType type) => _items[type] ?? null;
    public List<Item> GetAll() => _items.Values.ToList();

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

        if (!item.CanLevelUp())
        {
            return false;
        }

        if (!CurrencyManager.Instance.TrySpend(ECurrencyType.Heart, item.Cost))
        {
            return false;
        }

        item.TryLevelUp();
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
            Debug.Log("[Item] Create default data");
            _levels = ItemLevelSaveData.Default.Levels;

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

        Debug.Log("[Item] Load complete");
    }

    private void SaveData()
    {
        ItemLevelSaveData data = new ItemLevelSaveData();
        data.Levels = _levels;

        _repository.Save(data);
    }
}
