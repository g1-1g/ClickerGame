using UnityEngine;

public class VFXFloater : MonoBehaviour
{
    private ParticleSystem[] _particles;
    void Start()
    {
        _particles = GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        foreach (ParticleSystem particle in _particles)
        {
            if (particle.isPlaying)
            {
                return;
            }
        }
        HeartAmountVFXSpawner.Instance.Despawn(this.gameObject);
    }
}
