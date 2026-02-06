using DG.Tweening;
using UnityEngine;

public class MouseAutoClicker : AutoClicker
{
    private DG.Tweening.Sequence _sequence;
    private float _duration = 2f;

    private Vector3 _originalPosition;

    void Start()
    {
        _originalPosition = transform.position;
    }

    public void MouseGo(Vector3 position)
    {
        _sequence = DOTween.Sequence();

        _sequence.Append(transform.DOMove(position, _duration));
        _sequence.OnComplete(() => AutoClickerSpawner.Instance?.Despawn(this.gameObject));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IClickable clickable = collision.GetComponent<IClickable>();

        if (clickable != null)
        {
            Click(clickable, collision.ClosestPoint(transform.position));
        }  
    }

    public void OnDisable()
    {
        _sequence?.Kill();
    }
}
