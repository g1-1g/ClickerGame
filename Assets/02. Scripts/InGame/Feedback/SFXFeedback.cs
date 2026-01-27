using UnityEngine;

public class SFXFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private AudioClip _clip;
    public void Play(ClickInfo clickInfo)
    {
        SoundManager.Instance.AudioSource.pitch = Random.Range(0.0f, 1.0f);
        SoundManager.Instance.Play(_clip);
    }
}
