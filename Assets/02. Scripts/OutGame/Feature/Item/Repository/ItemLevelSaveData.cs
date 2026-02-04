using System.Linq;
using Firebase.Firestore;

[FirestoreData]
public class ItemLevelSaveData
{
    [FirestoreProperty]
    public int[] Levels { get; set; }

    public static ItemLevelSaveData Default => new ItemLevelSaveData()
    {
        Levels = Enumerable.Repeat(1, (int)EItemType.Count).ToArray()
    };
}