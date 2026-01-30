using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginSceneManager : MonoBehaviour
{
     

    private static LoginSceneManager _instance;
    public static LoginSceneManager Instance { get { return _instance; } }

    // 로그인씬 (로그인/회원가입) -> 게임씬  
    private ESceneMode _mode = ESceneMode.Login;

    // 비밀번호 확인 오브젝트
    [SerializeField] private GameObject _passwordConfirmObject;

    [SerializeField] private GameObject _gotoRegisterButtonObject;

    [SerializeField] private GameObject _loginButtonObject;

    [SerializeField] private GameObject _gotoLoginButtonObject;

    [SerializeField] private GameObject _registerButtonObject;

    [SerializeField] private TMP_InputField _idInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _passwordConfirmInputField;
    [SerializeField] private TextMeshProUGUI _messageText;

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
        AddButtonEvents();
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

    private void AddButtonEvents()
    {
        _gotoRegisterButtonObject.GetComponentInChildren<Button>()?.onClick.AddListener(GotoRegister);
        _loginButtonObject.GetComponentInChildren<Button>()?.onClick.AddListener(Login);
        _gotoLoginButtonObject.GetComponentInChildren<Button>()?.onClick.AddListener(GotoLogin);
        _registerButtonObject.GetComponentInChildren<Button>()?.onClick.AddListener(Register);
    }

    private void RemoveButtonEvents()
    {
        _gotoRegisterButtonObject.GetComponentInChildren<Button>()?.onClick.RemoveListener(GotoRegister);
        _loginButtonObject.GetComponentInChildren<Button>()?.onClick.RemoveListener(Login);
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

    public void Login()
    {
        string id = _idInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            _messageText.text = "아이디를 입력해주세요.";
            return;
        }

        string password = _passwordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            _messageText.text = "패스워드를 입력해주세요.";
            return;
        }
        
        if (!AccountManager.Instance.TryLogin(id, password))
        {
            _messageText.text = "아이디와 비밀번호를 확인해주세요.";
            return;
        }

        SceneManager.LoadScene("Game");
    }

    private void Register()
    {
        string id = _idInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            _messageText.text = "아이디를 입력해주세요.";
            return;
        }

        string password = _passwordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            _messageText.text = "패스워드를 입력해주세요.";
            return;
        }

        string password2 = _passwordConfirmInputField.text;
        if (string.IsNullOrEmpty(password2) || password != password2)
        {
            _messageText.text = "패스워드가 일치하지 않습니다..";
            return;
        }

        if (PlayerPrefs.HasKey($"{id}Hash"))
        {
            _messageText.text = "중복된 아이디입니다.";
            return;
        }

        if (!AccountManager.Instance.TryRegister(id, password))
        {
            _messageText.text = "아이디와 비밀번호의 형식을 확인해주세요.";
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
        RemoveButtonEvents();
    }

}
