using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Answer : MonoBehaviour 
{
    public TextMesh text;
    Rigidbody rb;
    private bool isRight = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetAnswer(string answer, bool isRight)
    {
        text.text = answer;
        this.isRight = isRight;
    }

    public void AnswerThis()
    {

    }
}
