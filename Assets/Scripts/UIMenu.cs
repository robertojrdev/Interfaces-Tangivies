using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour 
{
    public Button playBtn;
    public Button settingsBtn;
    public Button exitBtn;

    private void Start()
    {
        playBtn.onClick.AddListener(Play);
        settingsBtn.onClick.AddListener(ShowSettings);
        exitBtn.onClick.AddListener(Application.Quit);
    }

    private void ShowSettings()
    {

    }

    void Play()
    {
        SceneManager.LoadScene(1);
    }
}
