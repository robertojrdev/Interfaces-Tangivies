using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour 
{
    public Button playBtn;
    public Button settingsBtn;
    public Button exitBtn;
    public Button creditsBtn;
    public Button closeCreditsBtn;
    public GameObject credits;

    private void Start()
    {
        playBtn.onClick.AddListener(Play);
        settingsBtn.onClick.AddListener(ShowSettings);
        exitBtn.onClick.AddListener(Application.Quit);
        creditsBtn.onClick.AddListener(ToggleCredits);
        closeCreditsBtn.onClick.AddListener(ToggleCredits);
    }

    private void ShowSettings()
    {

    }

    public void ToggleCredits()
    {
        credits.SetActive(!credits.activeInHierarchy);
    }

    public void Toggle(GameObject toggleGo)
    {
        toggleGo.SetActive(!toggleGo.activeInHierarchy);
    }

    void Play()
    {
        SceneManager.LoadScene(1);
    }
}
