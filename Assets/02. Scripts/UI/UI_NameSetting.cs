using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_NameSetting : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _inputField;
    [SerializeField]
    private Button _button;

    private UI_Popup _popup;

    private ECatType _targetCat;

    private void Start()
    {
        _button.onClick.AddListener(SetName);
        _popup = GetComponent<UI_Popup>();

        CatManager.OnCatAdded += Show;
 
    }

    private void Show(ECatType type)
    {
        _popup.Up();
        _targetCat = type;
    }

    private void SetName()
    {
        CatManager.Instance.SetName(_targetCat, _inputField.text);
        _popup.Down();
    }

    private void OnDestroy()
    {
        CatManager.OnCatAdded -= Show;
        _button.onClick.RemoveAllListeners();
    }
}
