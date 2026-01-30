using TMPro;
using UnityEngine;

public class UI_TotalHearts : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _totalText;

    void Start()
    {
        TotalUpdate();
        CurrencyManager.OnCurrencyChanged += TotalUpdate;
    }

    void TotalUpdate()
    {
        var total = CurrencyManager.Instance.Heart;
        _totalText.text = $"{total}";
    }

    private void OnDestroy()
    {
        if (CatManager.Instance != null)
        {
            CurrencyManager.OnCurrencyChanged -= TotalUpdate;
        }  
    }
}

