using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseCurrencyRepository : ICurrencyRepository
{
    string _userID;
    FirebaseFirestore _db;

    private string COLLECTION_NAME = "Currency";

    public FirebaseCurrencyRepository(string userID)
    {
        _userID = userID;
        _db = FirebaseInitializer.Instance.Database;
    }

    public async UniTask Save(CurrencySaveData saveData)
    {
        try
        {
            await _db.Collection(COLLECTION_NAME).Document(_userID).SetAsync(saveData);
            Debug.Log("[Currency] 저장 성공: ");
        }
        catch (System.Exception e)
        {
            Debug.LogError("[Currency] 저장 실패: " + e);
        }
    }

    public async UniTask<CurrencySaveData> Load()
    {
        try
        {
            var result = await _db.Collection(COLLECTION_NAME).Document(_userID).GetSnapshotAsync();

            CurrencySaveData data = result.ConvertTo<CurrencySaveData>();
            Debug.LogFormat("불러오기 성공");
            if (data == null)
            {
                Debug.LogWarning("[Currency] 불러온 데이터가 null 입니다. 새로 생성합니다.");
                return CurrencySaveData.Default;
            }
            {
                return data;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("[Currency] 불러오기 실패, 새로 생성합니다.:" + e );
            return CurrencySaveData.Default;
        }
    }
}
