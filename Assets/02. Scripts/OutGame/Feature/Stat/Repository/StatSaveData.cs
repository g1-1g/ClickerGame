public class StatSaveData
{
    public double[] Stats;

    public static StatSaveData Default => new StatSaveData()
    {
        Stats = new double[(int)EStatType.Count]
    };
}