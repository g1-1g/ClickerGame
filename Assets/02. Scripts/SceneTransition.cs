using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private Button _startButton;

    void Start()
    {
        _startButton.onClick.AddListener(GameStart);
    }

    private void GameStart()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveAllListeners();
    }
}
