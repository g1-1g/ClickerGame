using Lean.Pool;
using UnityEngine;

public class HeartAmountVFXSpawner : MonoBehaviour
{
    private static HeartAmountVFXSpawner _instance;
    public static HeartAmountVFXSpawner Instance { get { return _instance; } }

    private LeanGameObjectPool _pool;

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

    public void ShowAmountVFX(ClickInfo clickInfo)
    {
        var floaterObject = _pool.Spawn(clickInfo.Position, Quaternion.identity, transform);

        VFXFloater floater = floaterObject.GetComponent<VFXFloater>();
    }

    public void Despawn(GameObject gameObject)
    {
        _pool.Despawn(gameObject);
    }
}
