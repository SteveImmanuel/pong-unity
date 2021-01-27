using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode upButton = KeyCode.UpArrow;
    public KeyCode downButton = KeyCode.DownArrow;
    public KeyCode transformButton = KeyCode.E;
    public float speed = 20f;
    public float yBoundary = 9f;
    public float maxLengthScale = 1.5f;
    public float transformDuration = 0.5f;
    public float powerUpDuration = 1;
    public float powerUpCoolDown = 5;

    private Rigidbody2D rb;
    private float currentVelocity;
    private ContactPoint2D lastContactPoint;
    private bool powerAvailable = true;
    private bool isPowerActivated = false;
    private float elapsedTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentVelocity = 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

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


        if (Input.GetKeyDown(transformButton) && powerAvailable && !isPowerActivated)
        {
            isPowerActivated = true;
            StartCoroutine(Transform(1, maxLengthScale, transformDuration));
            elapsedTime = 0;
        }

        if (elapsedTime >= powerUpDuration && isPowerActivated)
        {
            powerAvailable = false;
            isPowerActivated = false;
            StartCoroutine(Transform(maxLengthScale, 1, transformDuration));
            elapsedTime = 0;
        }

        if (elapsedTime >= powerUpCoolDown && !isPowerActivated)
        {
            powerAvailable = true;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ball")
        {
            lastContactPoint = collision.GetContact(0);
        }
    }
}
