using UnityEngine;
using UnityEngine.UIElements;

public class VFXFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private GameObject _particle;

    ParticleSystem[] _particles;

    public void Awake()
    {
        _particles = _particle.GetComponentsInChildren<ParticleSystem>();
    }
    public void Play(Vector3 hitPosition)
    {
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.position = hitPosition;

        foreach (ParticleSystem particle in _particles)
        {
            particle.Emit(emitParams, 1);
        }
    }
}
