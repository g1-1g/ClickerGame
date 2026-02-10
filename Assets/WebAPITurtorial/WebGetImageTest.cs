using System.Collections;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebGetImageTest : MonoBehaviour
{
    [SerializeField]
    private RawImage _myImage;

    async void Start()
    {
        Texture myTexture = await GetWebTexture("https://image.aladin.co.kr/product/25063/38/cover500/8964135725_1.jpg");
        _myImage.texture = myTexture;
    }

    async UniTask<Texture> GetWebTexture(string url)
    {
        try
        {
            var result = (await UnityWebRequestTexture.GetTexture(url).SendWebRequest());
            if (result.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(result.error);
                return null;
            }
            else
            {
                return ((DownloadHandlerTexture)result.downloadHandler).texture;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            return null;

        }
    }
    IEnumerator GetTexture(string url)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            _myImage.texture = myTexture;
        }
    }
}
