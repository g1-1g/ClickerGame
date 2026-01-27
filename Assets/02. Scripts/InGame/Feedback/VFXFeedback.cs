using UnityEngine;
using UnityEngine.UIElements;

public class VFXFeedback : MonoBehaviour, IFeedback
{
    public void Play(ClickInfo clickInfo)
    {
        HeartAmountVFXSpawner.Instance.ShowAmountVFX(clickInfo);
    }
}
