using Lean.Pool;
using UnityEngine;
using UnityEngine.AI;

public class DamagerFloaterSpawner : MonoBehaviour
{

    private static DamagerFloaterSpawner _instance;
    public static DamagerFloaterSpawner Instance { get { return _instance; } }

    private LeanGameObjectPool _pool;

    [SerializeField]
    private Transform _poolParent;

    private void Awake()
    {
        if (_instance != null || _instance == gameObject)
        {
            Destroy(this);
            return;
        }
        _instance = this;

        _pool = GetComponent<LeanGameObjectPool>();
    }

    public void ShowAmount(ClickInfo clickInfo)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(clickInfo.Position);

        var floaterObject = _pool.Spawn(Vector3.zero, Quaternion.identity, _poolParent);

        if (floaterObject.TryGetComponent<RectTransform>(out var rectTransform))
        {
            rectTransform.position = screenPos;
        }

        DamageFloater floater = floaterObject.GetComponent<DamageFloater>();
        floater.Show(clickInfo.HeartsAmount);
    }

    public void Despawn(GameObject gameObject)
    {
        _pool.Despawn(gameObject);
    }
}
