using System.Collections;
using UnityEngine;

public class ColorFlashFeedback : MonoBehaviour, IFeedback
{
    private Coroutine _coroutine;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Color _flashColor;

    public void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Play(Vector3 hisPosition)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(Play_Coroutine());
    }

    private IEnumerator Play_Coroutine()
    {
        _spriteRenderer.color = _flashColor;

        yield return new WaitForSeconds(0.1f);

        _spriteRenderer.color = Color.white;

    }
}
