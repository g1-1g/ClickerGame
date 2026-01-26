using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance {  get { return _instance; } }

    private AudioSource _audioSource;

    public AudioSource AudioSource { get { return _audioSource; } }

    private void Awake()
    {
        if (_instance != null || _instance == gameObject)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public void Play(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

}
