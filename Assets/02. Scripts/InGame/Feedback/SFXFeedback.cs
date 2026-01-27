using UnityEngine;

public class SFXFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private AudioClip _clip;
    public void Play(ClickInfo clickInfo)
    {
        SoundManager.Instance.Play(_clip);
    }
}
