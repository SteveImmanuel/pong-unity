using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 20f;
    public float yBoundary = 9f;
    public KeyCode upButton = KeyCode.UpArrow;
    public KeyCode downButton = KeyCode.DownArrow;

    private Rigidbody2D rb;
    private float currentVelocity;
    private ContactPoint2D lastContactPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentVelocity = 0f;
    }

    void Update()
    {
        if (Input.GetKey(upButton))
        {
            currentVelocity = speed;
        }
        else if (Input.GetKey(downButton))
        {
            currentVelocity = -speed;
        }
        else
        {
            currentVelocity = 0f;
        }
        Vector3 position = transform.position;
        position.y = Mathf.Clamp(position.y, -yBoundary, yBoundary);

        transform.position = position;
    }

    private void FixedUpdate()
    {
        Vector2 velocity = new Vector2(0, currentVelocity);
        rb.velocity = velocity;
    }

    public ContactPoint2D LastContactPoint
    {
        get { return lastContactPoint; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ball")
        {
            lastContactPoint = collision.GetContact(0);
        }
    }
}
