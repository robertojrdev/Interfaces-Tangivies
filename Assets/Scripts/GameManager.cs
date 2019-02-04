using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance { get; private set; }

    public Text scoreText;
    public float timeToQuestions = 10;
    private AsteroidPool pool;

    public int score = 0;
    public int life = 3;
    private int currentLife;

    private float timer = 0;
    private bool isLastSpawnActive = false;

    private List<bool> rightAnswers = new List<bool>();

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

        currentLife = life;
        Spaceship.Instance.UpdateLifeBar(life, currentLife);
        UIController.UpdateLife(currentLife);
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

    public void OnFinishQuestions()
    {
        FinalScore.ShowFinalScore(rightAnswers.ToArray());
    }

    public void OnAnswerRight()
    {
        isLastSpawnActive = false;
        score += 10;
        UIController.UpdateScore(score);
        rightAnswers.Add(true);
        Explosion.Explode("answer right", Spaceship.Instance.transform.position);
    }

    public void OnAnswerWrong()
    {
        isLastSpawnActive = false;
        score -= 10;
        score = Mathf.Clamp(score, 0, int.MaxValue);
        UIController.UpdateScore(score);
        rightAnswers.Add(false);
        Explosion.Explode("answer wrong", Spaceship.Instance.transform.position);
    }

    public void OnHitAsteroid()
    {
        currentLife--;
        currentLife = Mathf.Clamp(currentLife, 0, int.MaxValue);
        Spaceship.Instance.UpdateLifeBar(life, currentLife);
        if (currentLife == 0)
            FinishGame();
        UIController.UpdateLife(life);
    }

    public void FinishGame()
    {
        Debug.Log("Game Over");
        Spaceship.Instance.Destroy();
        FinalScore.ShowFinalScore(rightAnswers.ToArray());
        QuestionManager.Instance.HideQuestionAndStop();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
