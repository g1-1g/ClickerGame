using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WbGetStudentCSVTest : MonoBehaviour
{
    private async void Start()
    {
        //서버에게 데이터 요청하는 작업은 비동기 => 코루틴으로 처리
        string text = await GetWebText("https://raw.githubusercontent.com/mongilteacher/skku2_script_study/refs/heads/main/students.csv");
        text = text.TrimStart('\uFEFF');

        // CSV-Helper (어떻게 구현 됐냐보다는 API 문서를 보고 사용하는 법을 익히는게 중요)
        var config = new CsvConfiguration(CultureInfo.CurrentCulture);
        var stringReader = new StringReader(text);
        var csv = new CsvReader(stringReader, config);

        List <Person> people = new List<Person>();
        people = csv.GetRecords<Person>().ToList();

        foreach ( var p in people)
        {
            Debug.Log($"Name: {p.Name}, Age: {p.Age}");
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
