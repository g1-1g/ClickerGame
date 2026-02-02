using System;

public class Item
{
    // 1. 기획 테이블의 데이터를 가져온다.
    public readonly ItemSpecData SpecData;


    // 게임 중간에 바뀌는 데이터 (플레이어가 만들어 가는 값)
    public int Level { get; private set; }
    public double Cost => SpecData.BaseCost + Math.Pow(SpecData.CostMultiplier, Level);   // 지수 공식 : 기본 비용 + 증가량 ^ 레벨 
    public bool IsMaxLevel => Level >= SpecData.MaxLevel;

    // 2. 핵심 규칙(유효성)을 작성한다.
    public Item(ItemSpecData specData)
    {
        SpecData = specData;

        if (specData.MaxLevel < 0) throw new System.ArgumentException($"최대 레벨은 0보다 커야 합니다: {specData.MaxLevel}");
        if (specData.BaseCost <= 0) throw new System.ArgumentException($"기본 비용은 0보다 커야 합니다: {specData.BaseCost}");
        if (specData.Value <= 0) throw new System.ArgumentException($"증가량은 0보다 커야 합니다: {specData.Value}");
        if (specData.CostMultiplier <= 0) throw new System.ArgumentException($"비용 증가량은 0보다 커야 합니다: {specData.CostMultiplier}");
        if (string.IsNullOrEmpty(specData.Name)) throw new System.ArgumentException("이름은 비어있을 수 없습니다");
        if (string.IsNullOrEmpty(specData.Description)) throw new System.ArgumentException("설명은 비어있을 수 없습니다");

        Level = 1;
    }


    public bool CanLevelUp()
    {
        return !IsMaxLevel;
    }

    public bool TryLevelUp()
    {
        if (!CanLevelUp()) return false;

        Level++;

        return true;
    }

    public void SetLevel(int value)
    {
        Level = value;
    }
}