using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance { get; private set; }

    public Text scoreText;
    public float timeToQuestions = 10;
    private AsteroidPool pool;

    public int score = 0;
    public int life = 3;

    private float timer = 0;
    private bool isLastSpawnActive = false;

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        pool = GetComponent<AsteroidPool>();
        StartGame();
        pool.SetSpawnActive(true);

        timer = timeToQuestions;

        UIController.UpdateLife(life);
        UIController.UpdateScore(score);
    }

    private void StartGame()
    {
        pool.SetSpawnActive(true);
    }

    private void SpawnQuestion()
    {
        QuestionManager.Instance.GenerateQuestion();
        isLastSpawnActive = true;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if(!isLastSpawnActive && timer <= 0)
        {
            SpawnQuestion();
            timer = timeToQuestions;
        }
    }

    public void OnAnswerRight()
    {
        isLastSpawnActive = false;
        score += 10;
        UIController.UpdateScore(score);
    }

    public void OnAnswerWrong()
    {
        isLastSpawnActive = false;
        score -= 10;
        score = Mathf.Clamp(score, 0, int.MaxValue);
        UIController.UpdateScore(score);
    }

    public void OnHitAsteroid()
    {
        life--;
        life = Mathf.Clamp(life, 0, int.MaxValue);
        if (life == 0)
            FinishGame();
        UIController.UpdateLife(life);
    }

    public void FinishGame()
    {
        Debug.Log("Game Over");
        Spaceship.Instance.Destroy();
    }
}
