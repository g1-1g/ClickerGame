using System.Collections.Generic;
using UnityEngine;

public class UI_UpgradePanel : MonoBehaviour
{
    public List<UI_UpgradeItem> Items;


    private void Start()
    {
        RefreshAll();

        CurrencyManager.OnDataChanged += RefreshAll;
        ItemManager.OnDataChanged += Refresh;
    }


    private void RefreshAll()
    {
        var upgrades = ItemManager.Instance.GetAll();

        for (int i = 0; i < Items.Count; ++i)
        {
            Items[i].Refresh(upgrades[i]);
        }
    }

    private void Refresh(EItemType type)
    {
        var upgrade = ItemManager.Instance.Get(type);

        Items[(int)type].Refresh(upgrade);
    }

    private void OnDestroy()
    {
        CurrencyManager.OnDataChanged -= RefreshAll;
        ItemManager.OnDataChanged -= Refresh;
    }
}