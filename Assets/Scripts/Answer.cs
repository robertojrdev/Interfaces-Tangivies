using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Answer : MonoBehaviour 
{
    public Image image;
    Rigidbody rb;
    private bool isRight = false;
    private AnswerGroup group;
    private float minXPosition = -20;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (transform.position.x <= minXPosition)
        {
            SetAsRight(false);
            AnswerThis();
        }
    }

    public void SetGroup(AnswerGroup group)
    {
        this.group = group;
    }

    public void SetAnswer(Sprite answer)
    {
        image.sprite = answer;
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

    public void StartMovement(Vector3 velocity, float minXPosition = -20)
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
