using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginSceneManager : MonoBehaviour
{
    // 로그인씬 (로그인/회원가입) -> 게임씬  
    private ESceneMode _mode = ESceneMode.Login;

    // 비밀번호 확인 오브젝트
    [SerializeField] private GameObject _passwordConfirmObject;

    [SerializeField] private GameObject _gotoRegisterButtonObject;

    [SerializeField] private GameObject _loginButtonObject;

    [SerializeField] private GameObject _gotoLoginButtonObject;

    [SerializeField] private GameObject _registerButtonObject;

    private Button _loginButton;


    [SerializeField] private TMP_InputField _idInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _passwordConfirmInputField;
    [SerializeField] private TextMeshProUGUI _messageText;

    private void Awake()
    {
        _loginButton = _loginButtonObject.GetComponentInChildren<Button>();
    }
    private void Start()
    {
        AddEvents();
        Refresh();

        LastEmailSetting();

        
    }

    private void LastEmailSetting()
    {
        string id = PlayerPrefs.GetString("LastEmail");
        if (id != null)
        {
            _idInputField.text = id;
        }
    }

    private void AddEvents()
    {
        _idInputField.onValueChanged.AddListener(OnEmailTextChanged);

        _gotoRegisterButtonObject.GetComponentInChildren<Button>()?.onClick.AddListener(GotoRegister);
        _loginButton?.onClick.AddListener(Login);
        _gotoLoginButtonObject.GetComponentInChildren<Button>()?.onClick.AddListener(GotoLogin);
        _registerButtonObject.GetComponentInChildren<Button>()?.onClick.AddListener(Register);
    }

    private void RemoveEvents()
    {
        _idInputField.onValueChanged.RemoveListener(OnEmailTextChanged);

        _gotoRegisterButtonObject.GetComponentInChildren<Button>()?.onClick.RemoveListener(GotoRegister);
        _loginButton?.onClick.RemoveListener(Login);
        _gotoLoginButtonObject.GetComponentInChildren<Button>()?.onClick.RemoveListener(GotoLogin);
        _registerButtonObject.GetComponentInChildren<Button>()?.onClick.RemoveListener(Register);
    }

    private void Refresh()
    {
        // 2차 비밀번호 오브젝트는 회원가입 모드일때만 노출
        _passwordConfirmObject.SetActive(_mode == ESceneMode.Register);
        _gotoRegisterButtonObject.gameObject.SetActive(_mode == ESceneMode.Login);
        _loginButtonObject.gameObject.SetActive(_mode == ESceneMode.Login);
        _gotoLoginButtonObject.gameObject.SetActive(_mode == ESceneMode.Register);
        _registerButtonObject.gameObject.SetActive(_mode == ESceneMode.Register);
    }

    public void OnEmailTextChanged(string email)
    {
        var emailSpec = new AccountEmailSpecification();
        if (!emailSpec.IsSatisfiedBy(email))
        {
            _loginButton.interactable = false;
            _messageText.text = emailSpec.Message;
            return;
        }

        _messageText.text = "";
        _loginButton.interactable = true;
    }

    public void Login()
    {
        string id = _idInputField.text;

        string password = _passwordInputField.text;

        if (!AccountManager.Instance.TryLogin(id, password).Success)
        {
            _messageText.text = AccountManager.Instance.TryLogin(id, password).Message;
            return;
        }

        SceneManager.LoadScene("Game");
    }

    private void Register()
    {
        string id = _idInputField.text;

        string password = _passwordInputField.text;

        string password2 = _passwordConfirmInputField.text;
        if (string.IsNullOrEmpty(password2) || password != password2)
        {
            _messageText.text = "패스워드가 일치하지 않습니다..";
            return;
        }

        if (!AccountManager.Instance.TryRegister(id, password).Success)
        {
            _messageText.text = AccountManager.Instance.TryRegister(id, password).Message;
            return;
        }

        _messageText.text = "";
        GotoLogin();
    }

    private void GotoLogin()
    {
        _mode = ESceneMode.Login;
        Refresh();
    }

    private void GotoRegister()
    {
        _mode = ESceneMode.Register;
        Refresh();
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

}
