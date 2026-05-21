using System;

public struct Currency
{
    public double Value { get; }

    public Currency(double value)
    {
        if (value < 0)
        {
            throw new ArgumentException($"Currency는 0보다 작을 수 없습니다. : {value}");
        }

        Value = value;
    }

    public static Currency operator +(Currency currency1, Currency currency2)
    {
        return new Currency(currency1.Value + currency2.Value);
    }

    public static Currency operator -(Currency currency1, Currency currency2)
    {
        return new Currency(currency1.Value - currency2.Value);
    }

    public static bool operator >=(Currency currency1, Currency currency2)
    {
        return currency1.Value >= currency2.Value;
    }

    public static bool operator <=(Currency currency1, Currency currency2)
    {
        return currency1.Value <= currency2.Value;
    }

    public static bool operator >(Currency currency1, Currency currency2)
    {
        return currency1.Value > currency2.Value;
    }

    public static bool operator <(Currency currency1, Currency currency2)
    {
        return currency1.Value < currency2.Value;
    }

    public static implicit operator Currency(double value)
    {
        return new Currency(value);
    }

    public static explicit operator double(Currency currency)
    {
        return currency.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
