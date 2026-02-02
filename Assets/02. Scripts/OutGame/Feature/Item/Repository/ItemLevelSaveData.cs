using System.Linq;

public class ItemLevelSaveData
{
    public int[] Levels;

    public static ItemLevelSaveData Default => new ItemLevelSaveData()
    {
        Levels = Enumerable.Repeat(1, (int)EItemType.Count).ToArray()
    };
}