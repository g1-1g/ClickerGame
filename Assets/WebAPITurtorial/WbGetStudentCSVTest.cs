using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WbGetStudentCSVTest : MonoBehaviour
{
    private async void Start()
    {
        //서버에게 데이터 요청하는 작업은 비동기 => 코루틴으로 처리
        string text = await GetWebText("https://raw.githubusercontent.com/mongilteacher/skku2_script_study/refs/heads/main/students.csv");
        Debug.Log(text);

        List<Person> people = new List<Person>();

        string[] lines = text.Split('\n');
        for ( int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
    
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] tokens = line.Split(',');
            Person p = new Person();
            p.name = tokens[1];
            p.age = int.Parse(tokens[2]);
            people.Add(p);
        }

        foreach (Person p in people)
        {
            Debug.Log($"Name: {p.name}, Age: {p.age}");
        }
    }

    public async UniTask<string> GetWebText(string url)
    {
        try
        {
            var result = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
            return result;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            return null;
        }
    }
}
