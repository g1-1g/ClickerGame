using System;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;
using static UnityEngine.Rendering.DebugUI;

public class VFXFeedback : MonoBehaviour, IFeedback, ILevelUpFeedback
{
    [SerializeField]
    private VFXPlayer _levelUp;

    public void Play(ClickInfo clickInfo)
    {
        HeartAmountVFXSpawner.Instance.ShowAmountVFX(clickInfo);
    }

    public void PlayLevelUp()
    {
        _levelUp.Play(transform.position);    
    }
}
