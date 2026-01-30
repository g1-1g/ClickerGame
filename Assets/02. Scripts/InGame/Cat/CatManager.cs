using System;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    private static CatManager _instance;
    public static CatManager Instance { get { return _instance; } }

    [SerializeField]
    private bool _defaultName = true;

    private float _heartsPerClick = 10;

    [SerializeField]
    private Cat _currentCat;
    [SerializeField]
    private VFXPlayer _player;

    private CatLevelDatabaseSO _currentCatDatabase;

    public Cat CurrentCat => _currentCat;
    public bool DefaultName => _defaultName;

    public float HeartsPerClick;

    public static event Action<CatLevelDatabaseSO> OnCatChanged;

    private void Awake()
    {
        if (_instance != null || _instance == gameObject)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    private void Start()
    {

        _currentCat.OnLevelChanged += OnLevelChanged;
    }

    private void OnLevelChanged(CatLevelDataSO sO)
    {
        _player.Play(_currentCat.transform.position);
    }

    public void SetCat(CatLevelDatabaseSO database)
    {
        _currentCat.OnLevelChanged -= OnLevelChanged;

        _currentCatDatabase = database;
        OnCatChanged?.Invoke(database);

        _currentCat.OnLevelChanged += OnLevelChanged;
    }

    public void SetCatName(String name)
    {
        _currentCat.NameChange(name);
    }

    public void OnDestroy()
    {
        _currentCat.OnLevelChanged -= OnLevelChanged;
    }

    public void IncreaseHeartPerClick(float value)
    {
        _heartsPerClick += value;
    }
}
