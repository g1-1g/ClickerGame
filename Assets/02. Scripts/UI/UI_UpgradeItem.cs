using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_UpgradeItem : MonoBehaviour
{
    public TextMeshProUGUI NameTextUI;
    public TextMeshProUGUI DescriptionTextUI;
    public TextMeshProUGUI LevelTextUI;
    public TextMeshProUGUI CostTextUI;

    public Image UpgradeButtonImage;
    public Button UpgradeButton;

    private Item _item;

    public void Start()
    {
        UpgradeButton.onClick.AddListener(LevelUp);
    }

    public void OnDestroy()
    {
        UpgradeButton.onClick.RemoveListener(LevelUp);
    }

    public void Refresh(Item item)
    {
        _item = item;

        NameTextUI.text = item.SpecData.Name;
        DescriptionTextUI.text = string.Format(item.SpecData.Description);
        LevelTextUI.text = $"LV. {item.Level}";
        CostTextUI.text = item.Cost.ToString();

        bool canLevelUp = ItemManager.Instance.CanLevelUp(item.SpecData.StatType);

        CostTextUI.color = canLevelUp ? Color.white : Color.gray4;
        UpgradeButtonImage.color = canLevelUp ? Color.white : Color.gray4;
        UpgradeButton.TryGetComponent<ButtonReactionController>(out ButtonReactionController UpgradeButtonAnimation);
        UpgradeButtonAnimation.Active = canLevelUp? true : false;
        UpgradeButton.interactable = canLevelUp;
    }

    public void LevelUp()
    {
        if (_item == null) return;

        if (ItemManager.Instance.TryLevelUp(_item.SpecData.StatType))
        {
            // todo: 이펙트, 애니메이션, 트위닝
        }
    }
}