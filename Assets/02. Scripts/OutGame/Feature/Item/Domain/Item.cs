using System;

public class Item
{
    public readonly ItemSpecData SpecData;

    private readonly Currency _baseCost;
    private Level _level;

    public int Level => _level.Value;
    public Currency Cost => new Currency(_baseCost.Value + Math.Pow(SpecData.CostMultiplier, Level));
    public bool IsMaxLevel => _level.IsMax;

    public Item(ItemSpecData specData)
    {
        if (specData == null) throw new ArgumentNullException(nameof(specData));

        SpecData = specData;

        if (specData.MaxLevel < 1) throw new ArgumentException($"최대 레벨은 0보다 커야 합니다: {specData.MaxLevel}");
        if (specData.BaseCost <= 0) throw new ArgumentException($"기본 비용은 0보다 커야 합니다: {specData.BaseCost}");
        if (specData.Value <= 0) throw new ArgumentException($"값은 0보다 커야 합니다: {specData.Value}");
        if (specData.CostMultiplier <= 0) throw new ArgumentException($"비용 배율은 0보다 커야 합니다: {specData.CostMultiplier}");
        if (string.IsNullOrEmpty(specData.Name)) throw new ArgumentException("이름은 비어 있을 수 없습니다.");
        if (string.IsNullOrEmpty(specData.Description)) throw new ArgumentException("설명은 비어 있을 수 없습니다.");

        _baseCost = new Currency(specData.BaseCost);
        _level = new Level(1, SpecData.MaxLevel);
    }

    public bool CanLevelUp()
    {
        return !IsMaxLevel;
    }

    public bool TryLevelUp()
    {
        if (!CanLevelUp()) return false;

        _level = _level.Increase();

        return true;
    }

    public void SetLevel(int value)
    {
        _level = new Level(value, SpecData.MaxLevel);
    }
}