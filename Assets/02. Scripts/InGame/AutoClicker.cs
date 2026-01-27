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
            HeartsAmount = GameManager.Instance.HeartsPerClick,
            Position = position,
        };

        clickableScript.OnClick(clickInfo);
    }
}
