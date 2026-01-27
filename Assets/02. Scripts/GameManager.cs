using System;
using System.Xml.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField]
    private CatLevel _currentCat;
    [SerializeField]
    private VFXPlayer _player;


    private float _heartsPerClick;
    private float _totalHeart;

    private CatLevelDatabaseSO _currentCatDatabase;

    public CatLevel CurrentCat => _currentCat;

    public float HeartsPerClick;

    public event Action<float> OnTotalChange;
    public event Action<float> OnHeartGet;
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
        SetTotalHeart(0);
        _currentCat.OnLevelChanged += OnLevelChanged;
    }

    private void OnLevelChanged(CatLevelDataSO sO)
    {
        _player.Play(_currentCat.transform.position);
    }

    public void GetHeart(float heartAmount)
    {
        float value = _totalHeart + heartAmount;

        SetTotalHeart(value);
        CurrentCat.AffectionUp(heartAmount);
        OnHeartGet?.Invoke(heartAmount);
    }

    public void SetTotalHeart(float value)
    {
        _totalHeart = value;
        OnTotalChange?.Invoke(_totalHeart);
    }

    public void SetCat(CatLevelDatabaseSO database)
    {
        _currentCat.OnLevelChanged -= OnLevelChanged;

        _currentCatDatabase = database;
        OnCatChange?.Invoke(database);
        
        _currentCat.OnLevelChanged += OnLevelChanged;
    }

    public void IncreaseHeartPerClick(float value)
    {
        _heartsPerClick += value;
    }

    public void OnDestroy()
    {
        _currentCat.OnLevelChanged -= OnLevelChanged;
    }

}
