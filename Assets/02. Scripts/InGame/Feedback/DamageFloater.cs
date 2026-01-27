using UnityEngine;
using DG.Tweening;
using Lean.Pool;
using System.Collections;
using TMPro;

public class DamageFloater : MonoBehaviour
{
    
    [SerializeField]
    private float _duration;
    [SerializeField]
    private float _moveAmount;

    private TextMeshProUGUI _damageText;
    private CanvasGroup _canvasGroup;
    private Sequence _currentSequence;

    public void Awake()
    {
        _damageText = GetComponent<TextMeshProUGUI>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show(float damage)  
    {
        _damageText.text = $"{damage}";
        
        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOMove(transform.position + Vector3.up * _moveAmount, _duration)).SetEase(Ease.InBack);
        _currentSequence.Append(_canvasGroup.DOFade(0, _duration)).SetEase(Ease.OutBack);
        _currentSequence.OnComplete(() => DamagerFloaterSpawner.Instance?.Despawn(gameObject));
    }

    private void OnDisable()
    {
        _currentSequence?.Kill();
        _currentSequence = null;

        _canvasGroup.alpha = 1f;
    }
}
