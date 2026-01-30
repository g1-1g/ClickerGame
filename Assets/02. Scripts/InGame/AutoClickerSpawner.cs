using System.Threading;
using Lean.Pool;
using UnityEngine;

public class AutoClickerSpawner : MonoBehaviour
{
    private static AutoClickerSpawner _instance;
    public static AutoClickerSpawner Instance { get { return _instance; } }

    private LeanGameObjectPool _pool;

    [SerializeField] 
    private float _interval;

    private float _timer;

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

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _interval)
        {
            _timer = 0;
            Spawn();
        }
    }

    public void Spawn()
    {
        var autoClickerObject = _pool.Spawn(transform.position, Quaternion.identity, transform);


        MouseAutoClicker clicker = autoClickerObject.GetComponent<MouseAutoClicker>();
        clicker.MouseGo(CatManager.Instance.CurrentCat.transform.position);
    }

    public void Despawn(GameObject gameObject)
    {
        _pool.Despawn(gameObject);
    }
}
