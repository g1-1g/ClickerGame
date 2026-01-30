using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeItem : MonoBehaviour
{
    public TextMeshProUGUI NameTextUI;
    public TextMeshProUGUI DescriptionTextUI;
    public TextMeshProUGUI LevelTextUI;
    public TextMeshProUGUI CostTextUI;

    public Image UpgradeButtonImage;
    public Button UpgradeButton;

    private Upgrade _upgrade;

    public void Refresh(Upgrade upgrade)
    {
        _upgrade = upgrade;

        NameTextUI.text = upgrade.SpecData.Name;
        DescriptionTextUI.text = string.Format(upgrade.SpecData.Description, upgrade.HeartGet);
        LevelTextUI.text = $"LV. {upgrade.Level}";
        CostTextUI.text = upgrade.Cost.ToString();

        bool canLevelUp = UpgradeManager.Instance.CanLevelUp(upgrade.SpecData.Type);

        CostTextUI.color = canLevelUp ? Color.white : Color.gray4;
        UpgradeButtonImage.color = canLevelUp ? Color.white : Color.gray4;
        UpgradeButton.TryGetComponent<ButtonReactionController>(out ButtonReactionController UpgradeButtonAnimation);
        UpgradeButtonAnimation.Active = canLevelUp? true : false;
        UpgradeButton.interactable = canLevelUp;
    }

    public void LevelUp()
    {
        if (_upgrade == null) return;

        if (UpgradeManager.Instance.TryLevelUp(_upgrade.SpecData.Type))
        {
            // todo: 이펙트, 애니메이션, 트위닝
        }
    }
}