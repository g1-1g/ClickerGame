using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEditor.U2D.Tooling.Analyzer;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Cat 
{
    public readonly CatSpecDataSO CatSpecData;

    public CatSaveData SaveData;

    public ECatType CatType => CatSpecData.CatType;

    public String Name => SaveData.Name;

    public float AffectionRatio
    {
        get
        {
            CatSpecData.TryGetLevelData(SaveData.Level, out var levelData);

            if (levelData.RequiredAffection == 0)
            {
                return 1;
            }

            return (float)(SaveData.Affection / levelData.RequiredAffection);
        }
    }
    public Cat(CatSpecDataSO specData, CatSaveData saveData)
    {
        CatSpecData = specData;

        SaveData = saveData;

        if (specData.Image == null) throw new System.ArgumentException($"이미지는 null일 수 없습니다.");
        if (specData.GetMaxLevel() == 0) throw new System.ArgumentException($"레벨 데이터가 없습니다.");
        if (specData.LevelUpClip) throw new System.ArgumentException($"레벨업 애니메이션 Clip이 비어있습니다.");
    }

    public CatLevelSpecData GetLevelData()
    {
        CatSpecData.TryGetLevelData(SaveData.Level, out CatLevelSpecData data);

        return data;
    }

    public void SetCatName(String name)
    {
        SaveData.Name = name;
    }

    public bool TryLevelUp()
    {
        if (CatSpecData.GetMaxLevel() == SaveData.Level)
        {
            Debug.Log("이미 최고레벨 입니다.");
            return false;
        }
        SaveData.Level++;

        SaveData.Affection = 0;

        return true;
    }


    public bool AffectionUp(double baseValue, double rate)
    {
        
        SaveData.Affection += baseValue * (1.0 + rate);

        CatSpecData.TryGetLevelData(SaveData.Level, out var levelData);

        if (SaveData.Affection >= levelData.RequiredAffection)
        {
            return TryLevelUp();
        }

        return false;
    }
}