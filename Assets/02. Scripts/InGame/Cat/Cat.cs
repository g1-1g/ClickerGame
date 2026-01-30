using System;
using UnityEngine;

public class Cat : MonoBehaviour
{
    private int _level;
    private double _affection = 0;

    private CatAnimationPlayer _player;

    [Header("고양이 이름")]
    [SerializeField]
    private string _name;


    [Header("고양이 이미지")]
    [SerializeField]
    private Sprite _image;

    [SerializeField]
    private CatLevelDatabaseSO _catLevelsDatabase;

    private CatLevelDataSO _currentLevelData;

    public string Name => _name;
    public Sprite Image => _image;

    public CatLevelDataSO CurrentLevelData => _currentLevelData;

    public float AffectionRatio
    {
        get
        {
            if (_currentLevelData == null || _currentLevelData.RequiredAffection == 0)
            {
                return 1;
            }
            return (float)(_affection / _currentLevelData.RequiredAffection);
        }
    }

    public int Level => _level;
    public double Affection => _affection;
    public CatLevelDatabaseSO LevelDatabase => _catLevelsDatabase;

    public event Action<CatLevelDataSO> OnLevelChanged;
    public event Action<float> OnAffectionChanged;
    public event Action<String> OnNameChanged;
    public bool TryLevelUp()
    {
        if (_catLevelsDatabase.GetMaxLevel() == _level)
        {
            Debug.Log("이미 최고레벨 입니다.");
            return false;
        }
        _level++;

        _currentLevelData = _catLevelsDatabase.GetLevelData(_level);

        _affection = 0;

        OnLevelChanged?.Invoke(_currentLevelData);

        if (_level == 1) return true;
        _player.LevelUpTrigger();
        return true;
    }

    public void NameChange(string name)
    {
        _name = name;
        OnNameChanged?.Invoke(name);
    }
    public void AffectionUp(double value)
    {
        _affection += value;

        if (_affection >= _currentLevelData.RequiredAffection)
        {
            TryLevelUp();
        }

        OnAffectionChanged?.Invoke(AffectionRatio);
    }

    private void Awake()
    {
        _player = GetComponent<CatAnimationPlayer>();
    }

    private void Start()
    {
        TryLevelUp();
    }
}
