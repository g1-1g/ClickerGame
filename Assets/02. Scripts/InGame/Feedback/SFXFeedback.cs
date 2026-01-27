using UnityEngine;

public class SFXFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private AudioClip _clip;
    public void Play(Vector3 hisPosition)
    {
        SoundManager.Instance.AudioSource.pitch = Random.Range(0.0f, 1.0f);
        SoundManager.Instance.Play(_clip);
    }
}
