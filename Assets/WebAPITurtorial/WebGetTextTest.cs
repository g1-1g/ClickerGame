using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetTextTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //서버에게 데이터 요청하는 작업은 비동기 => 코루틴으로 처리
        StartCoroutine(GetText());
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
