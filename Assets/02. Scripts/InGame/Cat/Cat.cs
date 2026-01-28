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

    public string Name => _name;
    public Sprite Image => _image;

    [SerializeField]
    private CatLevelDatabaseSO _catLevelsDatabase;

    private CatLevelDataSO _currentLevelData;


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

    public CatLevelDataSO CurrentLevelData => _currentLevelData;
    public CatLevelDatabaseSO LevelDatabase => _catLevelsDatabase;

    public event Action<CatLevelDataSO> OnLevelChanged;
    public event Action<float> OnAffectionChanged;
    public event Action<String> OnNameChanged;
    public void LevelUp()
    {
        if (_catLevelsDatabase.GetMaxLevel() == _level)
        {
            Debug.Log("이미 최고레벨 입니다.");
            return;
        }
        _level++;
        _currentLevelData = _catLevelsDatabase.GetLevelData(_level);

        _affection = 0;

        OnLevelChanged?.Invoke(_currentLevelData);

        if (_level == 1) return;
        _player.LevelUpTrigger();
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
            LevelUp();
        }

        OnAffectionChanged?.Invoke(AffectionRatio);
    }

    private void Awake()
    {
        _player = GetComponent<CatAnimationPlayer>();
    }

    private void Start()
    {
        LevelUp();
    }
    
}
