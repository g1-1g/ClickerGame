using UnityEngine;
using static UnityEngine.ParticleSystem;

public class VFXPlayer : MonoBehaviour
{
    private ParticleSystem[] _particles;
    void Start()
    {
        _particles = GetComponentsInChildren<ParticleSystem>();
    }
    
    public void Play(Vector3 position)
    {
        transform.position = position;
        foreach (var particle in _particles)
        {
            particle.Emit(1);
        }
    }
}
