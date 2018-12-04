using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Answer : MonoBehaviour 
{
    public TextMesh text;
    Rigidbody rb;
    private bool isRight = false;
    private AnswerGroup group;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.freezeRotation = true;
    }

    public void SetGroup(AnswerGroup group)
    {
        this.group = group;
    }

    public void SetAnswer(string answer)
    {
        text.text = answer;
    }

    public void SetAsRight(bool right)
    {
        isRight = right;
    }

    public void AnswerThis()
    {
        if (isRight)
            GameManager.Instance.OnAnswerRight();
        else
            GameManager.Instance.OnAnswerWrong();

        group.Destroy();
    }

    public void StartMovement(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Spaceship player = other.transform.root.GetComponent<Spaceship>();
        if (!player)
            return;
        
        AnswerThis();
    }
}
