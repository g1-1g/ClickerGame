
using Firebase.Firestore;

[FirestoreData]
public class CatSaveData
{
    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public int Level { get; set; }

    [FirestoreProperty]
    public double Affection { get; set; }

    [FirestoreProperty]
    public ECatType CatType { get; set; }

    public CatSaveData()
    {
        Name = "MOZZI";
        Level = 0;
        Affection = 0;
    }

    public CatSaveData(ECatType catType) :this()
    {
        CatType = catType;
    }
}