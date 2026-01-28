using System;
using System.Xml.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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

    public event Action<CatLevelDatabaseSO> OnCatChange;

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
        OnCatChange?.Invoke(database);
        
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
