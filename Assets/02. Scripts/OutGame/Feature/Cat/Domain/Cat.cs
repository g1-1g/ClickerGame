using System;
using UnityEngine;

public class Cat
{
    public readonly CatSpecDataSO CatSpecData;

    private Level _level;

    public ECatType CatType => CatSpecData.CatType;
    public Sprite Image => CatSpecData.Image;
    public string Name { get; private set; }
    public int Level => _level.Value;
    public double Affection { get; private set; }

    public float AffectionRatio
    {
        get
        {
            CatSpecData.TryGetLevelData(Level, out var levelData);

            if (levelData.RequiredAffection == 0)
            {
                return 1;
            }

            return (float)(Affection / levelData.RequiredAffection);
        }
    }

    public Cat(CatSpecDataSO specData, CatSaveData saveData)
    {
        if (specData == null) throw new ArgumentNullException(nameof(specData));
        if (saveData == null) throw new ArgumentNullException(nameof(saveData));

        CatSpecData = specData;

        if (specData.Image == null) throw new ArgumentException("이미지는 null일 수 없습니다.");
        if (specData.GetMaxLevel() == 0) throw new ArgumentException("레벨 데이터가 없습니다.");
        if (!specData.LevelUpClip) throw new ArgumentException("레벨업 애니메이션 Clip이 비어있습니다.");

        Name = saveData.Name;
        _level = new Level(saveData.Level, specData.GetMaxLevel());
        Affection = saveData.Affection;
    }

    public CatLevelSpecData GetLevelData()
    {
        CatSpecData.TryGetLevelData(Level, out CatLevelSpecData data);

        return data;
    }

    public void SetCatName(string name)
    {
        Name = name;
    }

    private bool TryLevelUp()
    {
        if (_level.IsMax)
        {
            Debug.Log("Cat is already max level.");
            return false;
        }

        _level = _level.Increase();
        Affection = 0;

        return true;
    }

    public bool AffectionUp(double baseValue, double rate)
    {
        Affection += baseValue * (1.0 + rate);

        CatSpecData.TryGetLevelData(Level, out var levelData);

        if (Affection >= levelData.RequiredAffection)
        {
            return TryLevelUp();
        }

        return false;
    }
}
