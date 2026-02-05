using System.Collections;
using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using UnityEditor.Overlays;
using UnityEngine;

public class FirebaseItemLevelRepository : IItemLevelRepository
{
    private string _userId;
    FirebaseFirestore _db;
    private string COLLECTION_NAME = "Item";


    public FirebaseItemLevelRepository(string userId)
    {
        _userId = userId;
        _db = FirebaseInitializer.Instance.Database;
    }

    public async UniTask<ItemLevelSaveData> Load()
    {
        try
        {
            var result = await _db.Collection(COLLECTION_NAME).Document(_userId).GetSnapshotAsync();

            ItemLevelSaveData data = result.ConvertTo<ItemLevelSaveData>();
      
            if (data == null)
            {
                Debug.LogWarning("[Item] 불러올 데이터가 없습니다");
                return null;
            }
            {
                Debug.LogFormat("[Item] 불러오기 성공");
                return data;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("[Item] 불러오기 실패" + e);
            return null;
        }
    }

    public async UniTask Save(ItemLevelSaveData data)
    {
        try
        {
            await _db.Collection(COLLECTION_NAME).Document(_userId).SetAsync(data);
            Debug.Log("저장 성공: ");
        }
        catch (System.Exception e)
        {
            Debug.LogError("저장 실패: " + e);
        }
    }
}