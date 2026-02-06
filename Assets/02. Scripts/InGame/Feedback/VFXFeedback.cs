using System;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;
using static UnityEngine.Rendering.DebugUI;

public class VFXFeedback : MonoBehaviour, IFeedback
{
    [SerializeField]
    private VFXPlayer _levelUp;

    public void Start()
    {
        CatManager.OnAffectionUp += LevelUpVFXPlay;
    }

    private void LevelUpVFXPlay(bool value)
    {
        if (value == false) return;

        _levelUp.Play(transform.position);
    }

    public void Play(ClickInfo clickInfo)
    {
        HeartAmountVFXSpawner.Instance.ShowAmountVFX(clickInfo);
    }

    public void OnDestroy()
    {
        CatManager.OnAffectionUp -= LevelUpVFXPlay;
    }
}
