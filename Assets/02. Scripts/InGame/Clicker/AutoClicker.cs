using UnityEngine;
using UnityEngine.UIElements;

public class AutoClicker : MonoBehaviour
{      
    [SerializeField] GameObject[] clickables;

    private void Start()
    {
        clickables = GameObject.FindGameObjectsWithTag("ClickTarget");
    }

    protected void Click(IClickable clickableScript, Vector2 position)
    {
        ClickInfo clickInfo = new ClickInfo
        {
            Type = EClickType.Auto,
            HeartsAmount = StatManager.Instance.GetStat(EItemType.HeartPerClick) ,
            Position = position,
        };

        clickableScript.OnClick(clickInfo);
    }
}
