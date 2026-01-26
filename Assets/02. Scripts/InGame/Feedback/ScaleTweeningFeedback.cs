using DG.Tweening;
using UnityEngine;

public class ScaleTweeningFeedback : MonoBehaviour
{
    private ClickTarget _owner;

    private void Awake()
    {
        _owner = GetComponent<ClickTarget>();
    }

    public void PlayTween()
    {
        _owner.transform.DOKill();
        _owner.transform.DOScale(1.1f, 0.3f).OnComplete(() =>
        {
            _owner.transform.localScale = Vector3.one;
        });
    }
}
