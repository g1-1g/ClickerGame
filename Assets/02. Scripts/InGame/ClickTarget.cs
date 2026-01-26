using UnityEngine;

public class ClickTarget : MonoBehaviour, IClickable
{
    public bool OnClick(ClickInfo clickInfo)
    {
        Debug.Log($"{gameObject.name}: ¾²´ã");

        var feedbacks = GetComponentsInChildren<IFeedback>();
        foreach ( var feedback in feedbacks )
        {
            feedback.Play();
        }

        return true;
    }
}
