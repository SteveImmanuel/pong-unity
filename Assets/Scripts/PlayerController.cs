using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode upButton = KeyCode.UpArrow;
    public KeyCode downButton = KeyCode.DownArrow;
    public KeyCode transformButton = KeyCode.E;
    public KeyCode activateAIButton = KeyCode.RightShift;
    public float speed = 20f;
    public float yBoundary = 9f;
    public float maxLengthScale = 1.5f;
    public float transformDuration = 0.5f;
    public float powerUpDuration = 1;
    public float cooldownDuration = 5;
    public bool activateAI = false;
    public BallController ball;
    public UIController uIController;
    public LayerMask layerMask;

    private Rigidbody2D rb;
    private Rigidbody2D ballRb;
    private CircleCollider2D ballCollider;
    private float currentVelocity;
    private ContactPoint2D lastContactPoint;
    private bool powerAvailable = true;
    private bool isPowerActivated = false;
    private float elapsedTime = 0f;
    private BoxCollider2D objCollider;
    private Vector2 prediction = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballRb = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
        objCollider = GetComponent<BoxCollider2D>();
        currentVelocity = 0f;
    }

    void MoveUp()
    {
        currentVelocity = speed;
    }

    void MoveDown()
    {
        currentVelocity = -speed;
    }

    void PredictTrajectory()
    {
        RaycastHit2D predictionCastHit = Physics2D.CircleCast(ballRb.position, ballCollider.radius, ballRb.velocity.normalized, Mathf.Infinity, layerMask);
        if (predictionCastHit.collider == null)
        {
            prediction = ball.transform.position;
        }
        else if (predictionCastHit.collider.tag == gameObject.tag)
        {
            prediction = predictionCastHit.point;
        }
    }


    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (Input.GetKeyDown(activateAIButton))
        {
            activateAI = !activateAI;
            uIController.SetAIStatus(gameObject.tag, activateAI);
        }

        if (activateAI)
        {
            PredictTrajectory();
            float targetPosition = prediction.y;
            float random = Random.Range(0, Mathf.Abs(targetPosition - transform.position.y));

            if (targetPosition - transform.position.y >= (objCollider.size.y / 2) - random)
            {
                MoveUp();
            } else if (transform.position.y - targetPosition >= (objCollider.size.y / 2) - random)
            {
                MoveDown();
            }
            else
            {
                currentVelocity = 0f;
            }
        }
        else
        {
            if (Input.GetKey(upButton))
            {
                MoveUp();
            }
            else if (Input.GetKey(downButton))
            {
                MoveDown();
            }
            else
            {
                currentVelocity = 0f;
            }

            if (Input.GetKeyDown(transformButton) && powerAvailable && !isPowerActivated)
            {
                isPowerActivated = true;
                StartCoroutine(Transform(1, maxLengthScale, transformDuration));
                elapsedTime = 0;
            }
        }

        if (elapsedTime >= powerUpDuration && isPowerActivated)
        {
            powerAvailable = false;
            isPowerActivated = false;
            StartCoroutine(Transform(maxLengthScale, 1, transformDuration));
            elapsedTime = 0;
        }

        if (elapsedTime >= cooldownDuration && !isPowerActivated)
        {
            powerAvailable = true;
        }

        Vector3 position = transform.position;
        position.y = Mathf.Clamp(position.y, -yBoundary, yBoundary);
        transform.position = position;

        if (powerAvailable)
        {
            if (isPowerActivated)
            {
                uIController.SetPowerStatus(gameObject.tag, "ACTIVE (" + string.Format("{0:F1}", powerUpDuration - elapsedTime) + ")");
            }
            else
            {
                uIController.SetPowerStatus(gameObject.tag, "READY");
            }
        } else
        {
            uIController.SetPowerStatus(gameObject.tag, "COOLDOWN (" + string.Format("{0:F1}", cooldownDuration - elapsedTime) + ")");
        }
    }

    IEnumerator Transform(float from, float to, float duration)
    {
        float elapsed = 0;
        float newScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            newScale = from + (to - from) * (elapsed / duration);
            transform.localScale = new Vector3(1, newScale, 1);
            yield return null;
        }
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

    public void ResetPlayer()
    {
        transform.localPosition = Vector3.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        transform.rotation = Quaternion.identity;
    }

    public void ChangeRbType()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
        rb.mass = 0.1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            lastContactPoint = collision.GetContact(0);
        }
    }
}
