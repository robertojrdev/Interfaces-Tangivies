using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    private static FinalScore instance;

    public Transform holder;
    public ScoreItem prefab;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }

    private void Update()
    {
        //reset
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
            PlayerPrefs.SetString("Save", "");
    }

    public static void ShowFinalScore(params bool[] rightAnswers)
    {
        if (!instance)
        {
            FinalScore[] s = Resources.FindObjectsOfTypeAll(typeof(FinalScore)) as FinalScore[];
            if (s.Length > 0)
                instance = s[0];

            if(!instance)
                return;
        }

        instance.gameObject.SetActive(true);

        int index = 1;
        foreach (var answer in rightAnswers)
        {
            ScoreItem item = Instantiate(instance.prefab.gameObject, instance.holder).GetComponent<ScoreItem>();
            item.Activate(true);
            item.SetIndex(index);
            item.SetCorrect(answer);
            index++;
        }
    }

    public void SetScore(params bool[] rightAnswers)
    {
        gameObject.SetActive(true);

        int index = 1;
        foreach (var answer in rightAnswers)
        {
            ScoreItem item = Instantiate(prefab.gameObject, holder).GetComponent<ScoreItem>();
            item.Activate(true);
            item.SetIndex(index);
            item.SetCorrect(answer);
            index++;
        }
    }
}
