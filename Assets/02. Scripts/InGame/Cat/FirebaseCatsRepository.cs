using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseCatsRepository : ICatsRepository
{
    string _userID;
    FirebaseFirestore _db;

    private string COLLECTION_NAME = "Cats";

    
    public FirebaseCatsRepository(string userID)
    {
        _userID = userID;
        _db = FirebaseInitializer.Instance.Database;
    }

    public async UniTask Save(OwnedCatsSaveData saveData)
    {
        try
        {
            await _db.Collection(COLLECTION_NAME).Document(_userID).SetAsync(saveData);
            Debug.Log("저장 성공: ");
        }
        catch (System.Exception e)
        {
            Debug.LogError("저장 실패: " + e);
        }
    }

    public async UniTask<OwnedCatsSaveData> Load()
    {
        try
        {
            var result = await _db.Collection(COLLECTION_NAME).Document(_userID).GetSnapshotAsync();

            OwnedCatsSaveData data = result.ConvertTo<OwnedCatsSaveData>();
            
            if (data == null)
            {
                Debug.LogWarning("불러온 데이터가 없습니다.");
                return null;
            }
            else
            {
                return data;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("불러오기 실패:" + e);
            return null;
        }
    }
}