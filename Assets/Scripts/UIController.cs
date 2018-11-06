using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
    public static UIController Instance { get; private set; }

    [SerializeField] private Text score;
    [SerializeField] private Text life;

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public static void UpdateScore(int score)
    {
        if (!Instance)
            return;
        Instance.score.text = "Score: " + score;
    }

    public static void UpdateLife(int life)
    {
        if (!Instance)
            return;
        Instance.life.text = "Life: " + life;
    }

}
