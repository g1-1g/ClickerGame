using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class OwnedCatsSaveData

{
    [FirestoreProperty]
    public ECatType CurrentCatType { get; set; }
    [FirestoreProperty]
    public CatSaveData[] OwnedCats { get; set; } 
}