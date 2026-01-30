using TMPro;
using UnityEngine;

public class UI_TotalHearts : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _totalText;

    void Start()
    {
        TotalUpdate(0);
        CurrencyManager.OnHeartChange += TotalUpdate;
    }

    void TotalUpdate(double total)
    {
        _totalText.text = $"{total}";
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            CurrencyManager.OnHeartChange -= TotalUpdate;
        }  
    }
}

