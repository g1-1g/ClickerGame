using UnityEngine;

public class AmountFloatFeedback : MonoBehaviour, IFeedback
{
    public void Play(ClickInfo clickInfo)
    {
        DamagerFloaterSpawner.Instance.ShowAmount(clickInfo);
    }
}
