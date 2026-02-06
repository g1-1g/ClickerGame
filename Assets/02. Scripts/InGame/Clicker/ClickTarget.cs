using UnityEngine;

public class ClickTarget : MonoBehaviour, IClickable
{
    public bool OnClick(ClickInfo clickInfo)
    {
        Debug.Log($"{gameObject.name}: 쓰담");

        var feedbacks = GetComponentsInChildren<IFeedback>();
        foreach ( var feedback in feedbacks )
        {
            feedback.Play(clickInfo);
        }

        CurrencyManager.Instance.Add(ECurrencyType.Heart, clickInfo.HeartsAmount);

        return true;
    }
}
