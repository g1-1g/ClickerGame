using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetWeatherTest : MonoBehaviour
{
    private const string API_KEY = "b85cc06c00f4b0f2a37ab61044db52c9";
    async void Start()
    {
        float lat = 37;
        float lon = 127;
        string url =
            $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={API_KEY}&units=metric&lang=kr";

        Debug.Log(url);

        string jsonString = await GetWeather(url);
        var data = JsonUtility.FromJson<WeatherResponse>(jsonString);

        Debug.Log(data.name);
        Debug.Log(data.main.temp);
        Debug.Log(data.weather[0].description);
    }

    private async UniTask<string> GetWeather(string url)
    {
        var text = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
        return text;
    }



}


[Serializable]
public class WeatherResponse
{
    public Coord coord;
    public WeatherInfo[] weather;
    public string @base;          // "base"는 C# 예약어 성격이라 @ 붙임
    public MainInfo main;
    public int visibility;
    public WindInfo wind;
    public CloudsInfo clouds;
    public long dt;
    public SysInfo sys;
    public int timezone;
    public int id;
    public string name;
    public int cod;
}

[Serializable]
public class Coord
{
    public float lon;
    public float lat;
}

[Serializable]
public class WeatherInfo
{
    public int id;
    public string main;
    public string description;
    public string icon;
}

[Serializable]
public class MainInfo
{
    public float temp;
    public float feels_like;
    public float temp_min;
    public float temp_max;
    public int pressure;
    public int humidity;
    public int sea_level;
    public int grnd_level;
}

[Serializable]
public class WindInfo
{
    public float speed;
    public int deg;
}

[Serializable]
public class CloudsInfo
{
    public int all;
}

[Serializable]
public class SysInfo
{
    public int type;
    public int id;
    public string country;
    public long sunrise;
    public long sunset;
}

