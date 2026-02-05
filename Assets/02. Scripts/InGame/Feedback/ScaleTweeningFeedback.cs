using DG.Tweening;
using UnityEngine;

public class ScaleTweeningFeedback : MonoBehaviour, IFeedback
{
    private Vector3 _originScale;
    private Vector3 _targetScale;

    [SerializeField]
    private float _scaleFactor = 0.98f;
    [SerializeField]
    private float _duration = 0.1f;
    public void Start()
    {
        _originScale = transform.localScale;
        _targetScale = new Vector3(_originScale.x * _scaleFactor, _originScale.y* _scaleFactor, _originScale.z);
    }
    public void Play(ClickInfo clickInfo)
    {
        transform.DOKill();
        transform.DOScale(_targetScale, _duration).OnComplete(() =>
        {
            transform.localScale = _originScale;
        });
    }
}
