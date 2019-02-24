using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Spaceship : MonoBehaviour 
{
    public static Spaceship Instance { get; private set; }
    public bool MouseControl { get => mouseControl;
        set
        {
            mouseControl = value;

            if (value)
                onActivateMouseControl.Invoke();
            else
                onDeactivateMouseControl.Invoke();

            print("Called");
        }
    }

    [SerializeField] private bool mouseControl = false;
    public float maxHeightDifference;
    public float maxRotation;
    public float smoothMove = 1;

    [Header("Arduino")]
    public float scale = 0.1f;
    public float startHeight;

    [Header("Life")]
    public Image lifeBar;

    [Header("Events")]
    public UnityEvent onActivateMouseControl;
    public UnityEvent onDeactivateMouseControl;

    private Rigidbody rb;
    private float height;
    private Vector3 currentSmoothVelocity;
    private Camera cam;

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        cam = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            MouseControl = !MouseControl;

        if (MouseControl)
            SetHeightWithMouse();

        MovePlayer();
        Rotate();
    }

    public void SetHeightWithArduino(float height)
    {
        if (MouseControl)
            return;

        if (height > 100 || height < 0)
            return;

        this.height = height * scale + startHeight;
    }

    private void SetHeightWithMouse()
    {
        Plane plane = new Plane(Vector3.back, 0);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        float dist = 0;
        Vector3 hitPos;
        if (plane.Raycast(ray, out dist))
        {
            hitPos = ray.direction * dist;
            height = hitPos.y;
        }
    }

    private void MovePlayer()
    {
        Vector3 position = rb.position;
        position.y = height;
        rb.position = Vector3.SmoothDamp(rb.position, position, ref currentSmoothVelocity, smoothMove);
    }

    private void Rotate()
    {
        Vector3 rotation = rb.rotation.eulerAngles;
        float heightDifference = height - transform.position.y;
        rotation.z = (heightDifference / maxHeightDifference
);
        rotation.z = Mathf.Clamp(rotation.z, -1f, 1f);
        rotation.z *= maxRotation;
        rb.rotation = Quaternion.Euler(rotation);
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        Explosion.Explode("destroy spaceship", transform.position);
    }

    public void UpdateLifeBar(float maxLife, float currentLife)
    {
        float t = currentLife / maxLife;
        Color color = Color.Lerp(Color.red, Color.green, t);

        lifeBar.fillAmount = t;
        lifeBar.color = color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            if (asteroid)
                asteroid.Destroy();
            GameManager.Instance.OnHitAsteroid();
        }
    }
}
