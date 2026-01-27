using System;
using System.Xml.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private float _heartsPerClick;
    private float _totalHeart;

    public float HeartsPerClick;

    public event Action<float> OnTotalChange;
    public event Action<float> OnHeartGet;

    private void Awake()
    {
        if (_instance != null || _instance == gameObject)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        SetTotalHeart(0);
    }

    public void GetHeart(float heartAmount)
    {
        float value = _totalHeart + heartAmount;

        SetTotalHeart(value);
        OnHeartGet?.Invoke(heartAmount);
    }

    public void SetTotalHeart(float value)
    {
        _totalHeart = value;
        OnTotalChange?.Invoke(_totalHeart);
    }

    public void IncreaseHeartPerClick(float value)
    {
        _heartsPerClick += value;
    }

}
