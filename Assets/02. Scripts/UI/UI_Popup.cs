using UnityEngine;

using UnityEngine.UI;

[RequireComponent(typeof(UI_BasicAnimation))]
[RequireComponent(typeof(CanvasGroup))]
public class UI_Popup : MonoBehaviour
{
    [SerializeField]
    private bool _timeStop = false;

    [SerializeField]
    private bool _useButton = false;

    [SerializeField]
    private Button _showButton;

    [SerializeField]
    private bool _popUpAnimation = true;

    [SerializeField]
    private bool _popDownAnimation = false;

    [SerializeField]
    private float _popDuration = 0.5f;

    [SerializeField]
    private float _startScale = 0.5f;

    private UI_BasicAnimation _animation;

    private bool _isOpened = false;


    void Awake()
    {
        TryGetComponent<UI_BasicAnimation>(out _animation);
        _showButton?.onClick.AddListener(OnButtonClick);
        _animation.Hide();
    }

    private void OnButtonClick()
    {
        if (!_isOpened)
        {
            Up();
        }
        else
        {
            Down();
        }
    }

    public void Up()
    {
        if (_timeStop)
        {
            Time.timeScale = 0f;
        }
        if (_popUpAnimation)
        {
            _animation.PopUp(_popDuration, _startScale);
        }
        else
        {
            _animation?.Show();
        }
        _isOpened = true;
    }

    public void Down()
    {
        if (_timeStop)
        {
            Time.timeScale = 1f;
        }
        if (_popDownAnimation)
        {
            _animation.PopDown(_popDuration, _startScale);
        }else
        {
            _animation.Hide();
        }
        _isOpened = false;
    }

    public void OnDestroy()
    {
        _showButton?.onClick.RemoveAllListeners();
    }
}
