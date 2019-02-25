using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Results : MonoBehaviour
{
    public Transform holder;
    public FinalScore prefab;

    private void Start()
    {
        Load();
    }

    public void Load()
    {
        string result = PlayerPrefs.GetString("Save");

        if (string.IsNullOrEmpty(result))
            return;

        Dictionary<FinalScore, List<bool>> results = new Dictionary<FinalScore, List<bool>>();

        foreach (var cha in result)
        {
            switch (cha)
            {
                case '_':
                    FinalScore score = Instantiate(prefab.gameObject, holder).GetComponent<FinalScore>();
                    results.Add(score, new List<bool>());
                    break;
                case '0':
                    if(results.Count > 0)
                        results.Last().Value.Add(false);
                    break;
                case '1':
                    if(results.Count > 0)
                        results.Last().Value.Add(true);
                    break;
            }
        }

        foreach (var score in results)
        {
            score.Key.SetScore(score.Value.ToArray());
        }
    }
}
