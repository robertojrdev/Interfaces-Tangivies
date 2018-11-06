using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour 
{
    public static QuestionManager Instance { get; private set; }

    public Answer answerPrefab;
    public Text questionText;
    public Transform spawnPosition;
    public float answerDistance;
    public float answerSpeed;

    private List<Question> questions = new List<Question>();
    private int questionIndex;

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        LoadQuestions();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            GenerateQuestion();
    }

    public void GenerateQuestion()
    {
        int index = GetNexQuestionIndex();
        if (index == -1)
            return;

        Question question = questions[index];
        AnswerGroup answerGroup = new AnswerGroup(question, answerPrefab);
        answerGroup.SetPosition(spawnPosition.position, answerDistance);
        answerGroup.StartMovement(answerSpeed);

        //set question text
        questionText.text = question.question;
    }

    private int GetNexQuestionIndex()
    {
        if (questions == null || questions.Count == 0)
            return -1;

        if(questionIndex >= questions.Count)
        {
            questionIndex = 0;
        }

        int index = questionIndex;
        questionIndex++;

        return index;
    }

    public void LoadQuestions()
    {
        string path = GetQuestionsPath();
        if (!Directory.Exists(path))
            return;

        List<string> jsons = new List<string>();
        foreach (string filePath in Directory.GetFiles(path))
        {
            jsons.Add(File.ReadAllText(filePath));
        }

        foreach (string json in jsons)
        {
            Debug.Log(json);
            try
            {
                Question question = JsonUtility.FromJson<Question>(json);
                questions.Add(question);
            }
            catch(Exception ex)
            {
                Debug.Log("A json failed to read - " + ex.ToString());
            }
        }

        Debug.Log("Loaded " + questions.Count + " questions");
    }

    public void CreateQuestionAndSave(Question question)
    {
        string json = JsonUtility.ToJson(question);
        string path = GetQuestionsPath();
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        int filesCount = Directory.GetFiles(path).Length;
        Path.Combine(path, "question_" + filesCount + ".txt");

        StreamWriter stream = new StreamWriter(path);
        stream.Write(json);
        stream.Dispose();
    }

    private string GetQuestionsPath()
    {
        string path = Application.dataPath;
        path = Path.Combine(path, "Resources");
        path = Path.Combine(path, "Questions");
        return path;
    }
}

public class AnswerGroup
{
    public readonly Answer[] answers;

    public AnswerGroup(Answer[] answers)
    {
        this.answers = answers;
    }

    public AnswerGroup(Question question, Answer answerPrefab)
    {
        answers = new Answer[3];

        for (int i = 0; i < answers.Length; i++)
        {
            answers[i] = GameObject.Instantiate(answerPrefab.gameObject).GetComponent<Answer>();
            if (question.correctOne - 1 == i)
                answers[i].SetAsRight(true);
        }
        //set answer
        answers[0].SetAnswer(question.answer1);
        answers[1].SetAnswer(question.answer2);
        answers[2].SetAnswer(question.answer3);

        //set group
        for (int i = 0; i < answers.Length; i++)
        {
            answers[i].SetGroup(this);
        }
    }

    public void SetPosition(Vector3 position, float distance)
    {
        if (answers == null)
            return;

        //set position
        answers[0].gameObject.transform.position = position;
        answers[1].gameObject.transform.position = position + Vector3.up * distance;
        answers[2].gameObject.transform.position = position + Vector3.down * distance;
    }

    public void StartMovement(float speed)
    {
        if (answers == null)
            return;

        //set group and start movement
        for (int i = 0; i < answers.Length; i++)
        {
            answers[i].SetGroup(this);
            answers[i].StartMovement(Vector3.left * speed);
        }
    }

    public void Destroy()
    {
        if (answers == null)
            return;

        foreach (var answer in answers)
        {
            answer.Destroy();
        }
    }
}

[Serializable]
public class Question
{
    public string question;
    public string answer1;
    public string answer2;
    public string answer3;
    public int correctOne;
}
