using TMPro;
using UnityEngine;

public class UI_TotalHearts : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _totalText;

    void Start()
    {
        TotalUpdate(0);
        GameManager.Instance.OnTotalChange += TotalUpdate;
    }

    void TotalUpdate(float total)
    {
        _totalText.text = $"{total}";
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnTotalChange -= TotalUpdate;
        }  
    }
}

