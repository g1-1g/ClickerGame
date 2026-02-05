
using Firebase.Firestore;

[FirestoreData]
public class CatData
{
    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public int Level { get; set; }

    [FirestoreProperty]
    public double Affection { get; set; }

    [FirestoreProperty]
    public ECatType CatType { get; set; }

    public CatData()
    {
        Name = "MOZZI";
        Level = 0;
        Affection = 0;
    }

    public CatData(ECatType catType) :this()
    {
        CatType = catType;
    }
}