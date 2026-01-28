using System;

public struct Currency
{
    public double Value;

    public Currency(double value)
    {
        //유효성 검사
        if (value < 0)
        {
            throw new Exception("Currency 값은 0보다 작을 수 없습니다.");
        }

        Value = value;
    }

    // 1. 재화끼리 더하기
    public static Currency operator +(Currency currency1, Currency currency2)
    {
        return new Currency(currency1.Value + currency2.Value);
    }

    // 2. 재화끼리 빼기
    public static Currency operator -(Currency a, Currency b)
    {
        return new Currency(a.Value - b.Value);
    }

    // 3. 비교 연산자들
    public static bool operator >=(Currency a, Currency b)
    {
        return a.Value >= b.Value;
    }

    public static bool operator <=(Currency a, Currency b)
    {
        return a.Value <= b.Value;
    }

    public static bool operator >(Currency a, Currency b)
    {
        return a.Value > b.Value;
    }

    public static bool operator <(Currency a, Currency b)
    {
        return a.Value < b.Value;
    }

    // double → Currency 암시적 변환    
    public static implicit operator Currency(double value)
    {
        return new Currency(value);
    }

    // Currency -> double 암시적 변환
    public static explicit operator double(Currency currency)
    {
        return currency.Value;
    }


}
