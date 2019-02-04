using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreItem : MonoBehaviour
{
    public Text indexTex;
    public Toggle toggle;
    public Image toggleImage;

    private void Start()
    {
        toggle.enabled = false;
    }

    public void Activate(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetIndex(int index)
    {
        indexTex.text = index.ToString();
    }

    public void SetCorrect(bool correct)
    {
        toggle.isOn = correct;
        toggleImage.color = correct ? Color.green : Color.red;
    }

    private bool firstTime = true;
    private void OnValidate()
    {
        if(firstTime)
        {
            firstTime = false;

            if (!indexTex)
                indexTex = GetComponentInChildren<Text>();

            if (!toggle)
                toggle = GetComponentInChildren<Toggle>();

            if (!toggleImage)
                toggleImage = GetComponentInChildren<Image>();
        }
    }
}
