using System;

public struct Level
{
    public int Value { get; }
    public int MaxValue { get; }

    public bool IsMax => Value >= MaxValue;

    public Level(int value, int maxValue)
    {
        if (maxValue < 1)
        {
            throw new ArgumentException($"최고 레벨은 1 이상이어야 합니다. : {maxValue}");
        }

        if (value < 1)
        {
            throw new ArgumentException($"레벨은 1 이상이어야 합니다. : {value}");
        }

        if (value > maxValue)
        {
            throw new ArgumentException($"레벨은 최대 레벨을 초과할 수 없습니다. Level: {value}, Max: {maxValue}");
        }

        Value = value;
        MaxValue = maxValue;
    }

    public Level Increase()
    {
        if (IsMax)
        {
            return this;
        }

        return new Level(Value + 1, MaxValue);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
