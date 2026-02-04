using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseCurrencyRepository : ICurrencyRepository
{
    string _userID;

    public FirebaseCurrencyRepository(string userID)
    {
        _userID = userID;
    }

    public async UniTask Save(CurrencySaveData saveData)
    {
        try
        {
            await FirebaseInitializer.Instance.Database.Collection($"{_userID}").Document("Currency").SetAsync(saveData);
            Debug.Log("저장 성공: ");
        }
        catch (System.Exception e)
        {
            Debug.LogError("저장 실패: " + e);
        }
    }

    public async UniTask<CurrencySaveData> Load()
    {
        try
        {
            var result = await FirebaseInitializer.Instance.Database.Collection($"{_userID}").Document("Currency").GetSnapshotAsync();

            CurrencySaveData data = result.ConvertTo<CurrencySaveData>();
            Debug.LogFormat("불러오기 성공");
            if (data == null)
            {
                Debug.LogWarning("불러온 데이터가 null 입니다. 새로 생성합니다.");
                return CurrencySaveData.Default;
            }
            {
                return data;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("불러오기 실패, 새로 생성합니다.:"  + e );
            return CurrencySaveData.Default;
        }
    }
}
