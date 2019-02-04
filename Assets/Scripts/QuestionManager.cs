using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour 
{
    public static QuestionManager Instance { get; private set; }
    public static Group SelectedGroup { get; set; } = Group.Group_1;

    public enum Group { Group_1, Group_2, Group_3 }

    public Answer answerPrefab;
    public Image questionImage;
    public Transform spawnPosition;
    public float answerDistance;
    public float answerSpeed;

    private List<Question> questions = new List<Question>();
    private int questionIndex;

    private bool active = true;

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

    public void HideQuestionAndStop()
    {
        questionImage.sprite = null;
        questionImage.color = new Color(0, 0, 0, 0);
        active = false;
    }

    public void GenerateQuestion()
    {
        if (!active)
            return;

        int index = GetNexQuestionIndex();
        if (index == -1) //if has no questions
            return;
        else if (index == -2) //if finish questions
        {
            GameManager.Instance.OnFinishQuestions();
            HideQuestionAndStop();
            return;
        }

        Question question = questions[index];
        AnswerGroup answerGroup = new AnswerGroup(question, answerPrefab);
        answerGroup.SetPosition(spawnPosition.position, answerDistance);
        answerGroup.StartMovement(answerSpeed);

        //set question text
        questionImage.sprite = question.question;
    }

    private int GetNexQuestionIndex()
    {
        if (questions == null || questions.Count == 0)
            return -1;

        if(questionIndex >= questions.Count)
        {
            questionIndex = -2;
        }

        int index = questionIndex;
        questionIndex++;

        return index;
    }

    public void LoadQuestions()
    {
        string path = GetQuestionsPath();
        //print("Path: " + path);
        //if (!Directory.Exists(path))
        //    return;

        object[] assets = Resources.LoadAll(path, typeof(Sprite));
        print("Count: " + assets.Length);

        for (int i = 0; i < assets.Length / 4; i++)
        {
            Question q = new Question();
            q.question = assets[4 * i] as Sprite;
            q.answer1 = assets[4 * i + 1] as Sprite;
            q.answer2 = assets[4 * i + 2] as Sprite;
            q.answer3 = assets[4 * i + 3] as Sprite;
            q.correctOne = 1;

            questions.Add(q);
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
        //string path = Application.dataPath;
        //path = Path.Combine(path, "Resources");
        //path = Path.Combine(path, "Questions");
        string path = "Questions";
        path = Path.Combine(path, SelectedGroup.ToString());
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
    public Sprite question;
    public Sprite answer1;
    public Sprite answer2;
    public Sprite answer3;
    public int correctOne;
}
