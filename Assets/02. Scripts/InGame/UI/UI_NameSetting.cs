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

    private void Start()
    {
        _button.onClick.AddListener(SetName);
        _popup = GetComponent<UI_Popup>();
        _popup.Show();
    }

    private void SetName()
    {
        GameManager.Instance.SetCatName(_inputField.text);
        _popup.Hide();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
