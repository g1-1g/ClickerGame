using System;
using UnityEngine;

public class Cat 
{
    public readonly CatSpecDataSO CatSpecData;

    public ECatType CatType => CatSpecData.CatType;

    public Sprite Image => CatSpecData.Image;

    public string Name { get; set; }

    public int Level { get; set; }

    public double Affection { get; set; }

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
        CatSpecData = specData;

        Name = saveData.Name;
        Level = saveData.Level;
        Affection = saveData.Affection;

        if (specData.Image == null) throw new System.ArgumentException($"이미지는 null일 수 없습니다.");
        if (specData.GetMaxLevel() == 0) throw new System.ArgumentException($"레벨 데이터가 없습니다.");
        if (!specData.LevelUpClip) throw new System.ArgumentException($"레벨업 애니메이션 Clip이 비어있습니다.");

        // SaveData 검사

    }

    public CatLevelSpecData GetLevelData()
    {
        CatSpecData.TryGetLevelData(Level, out CatLevelSpecData data);

        return data;
    }

    public void SetCatName(String name)
    {
        Name = name;
    }

    private bool TryLevelUp()
    {
        if (CatSpecData.GetMaxLevel() == Level)
        {
            Debug.Log("이미 최고레벨 입니다.");
            return false;
        }
        Level++;

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