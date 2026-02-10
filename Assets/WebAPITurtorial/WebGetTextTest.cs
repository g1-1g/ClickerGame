using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetTextTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        //서버에게 데이터 요청하는 작업은 비동기 => 코루틴으로 처리
        string text = await GetWebText("https://www.google.com/search?q=%EB%BD%80%EB%A1%9C%EB%A1%9C&sca_esv=e5f3872605fd4577&hl=ko&biw=1745&bih=859&sxsrf=ANbL-n4R8XM_m_xpv9kQtP3kiSLcrl5pBw%3A1770696215975&ei=F66KaZ-ZO-Hi2roPjbClkAg&ved=0ahUKEwjfgNW3hc6SAxVhsVYBHQ1YCYIQ4dUDCBE&uact=5&oq=%EB%BD%80%EB%A1%9C%EB%A1%9C&gs_lp=Egxnd3Mtd2l6LXNlcnAiCeu9gOuhnOuhnDIKEAAYsAMY1gQYRzIKEAAYsAMY1gQYRzIKEAAYsAMY1gQYRzIKEAAYsAMY1gQYRzIKEAAYsAMY1gQYRzIKEAAYsAMY1gQYRzIKEAAYsAMY1gQYRzIKEAAYsAMY1gQYRzIKEAAYsAMY1gQYRzIKEAAYsAMY1gQYR0jRCFDQBFj7BnACeAGQAQCYAWigAdABqgEDMC4yuAEDyAEA-AEBmAIEoALaAcICChAuGIAEGEMYigXCAgoQABiABBhDGIoFwgIFEAAYgATCAggQLhiABBixA8ICGRAuGIAEGEMYigUYlwUY3AQY3gQY4ATYAQGYAwDiAwUSATEgQIgGAZAGCroGBggBEAEYFJIHAzIuMqAH6RKyBwMwLjK4B9YBwgcDMC40yAcHgAgA&sclient=gws-wiz-serp");
        Debug.Log(text);
    }

    public async UniTask<string> GetWebText(string url)
    {
        var result = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
        return result;
    }

    IEnumerator GetText()
    {
        // URL이란 웹서버 어떤 "자원(페이지/이미지/파일/데이터/API)"이 있는 위치를 가리키는 주소
        UnityWebRequest www = UnityWebRequest.Get("https://www.google.com/search?q=%EB%BD%80%EB%A1%9C%EB%A1%9C&oq=%EB%BD%80%EB%A1%9C%EB%A1%9C&gs_lcrp=EgZjaHJvbWUqDQgAEAAY4wIYsQMYgAQyDQgAEAAY4wIYsQMYgAQyCggBEC4YsQMYgAQyBwgCEAAYgAQyBwgDEC4YgAQyBwgEEAAYgAQyCggFEC4YsQMYgAQyBwgGEAAYgAQyBwgHEAAYgAQyBggIEAAYAzIHCAkQABiABNIBCjYzNTQwajBqMTWoAgCwAgA&sourceid=chrome&ie=UTF-8");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}
