using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float initialForce = 25f;
    public float minTimeBeforeFireball = 5f;
    public float maxTimeBeforeFireball = 10f;
    public float speedMultiplier = 2f;
    public float acceleration = 5f;

    private Rigidbody2D rb;
    private Vector2 trajectoryOrigin;
    private TrailRenderer trail;
    private float elapsedTime = 0f;
    private float timeBetweenFireball;
    private bool isFireball = false;

    void Start()
    {
        trajectoryOrigin = transform.position;
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponentInChildren<TrailRenderer>();
        trail.enabled = false;
        timeBetweenFireball = Random.Range(minTimeBeforeFireball, maxTimeBeforeFireball);
        RestartGame();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= timeBetweenFireball && !isFireball)
        {
            isFireball = true;
            trail.enabled = true;
            StartCoroutine(IncreaseSpeed(speedMultiplier));
        }
    }

    IEnumerator IncreaseSpeed(float speedMultiplier)
    {
        float target = rb.velocity.magnitude * speedMultiplier;
        while (rb.velocity.magnitude < target)
        {
            Vector2 deltaVelocity= rb.velocity.normalized;
            deltaVelocity*= acceleration * Time.deltaTime;
            rb.velocity += deltaVelocity;
            yield return null;
        }
        GameManager.instance.FireBallMode();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        trajectoryOrigin = transform.position;
    }


    private void PushBall()
    {
        float yRandomForce = Random.Range(-1f, 1f);
        float xRandomForce = Random.Range(-1f, 1f);
        Vector2 force = new Vector2(xRandomForce, yRandomForce);

        rb.AddForce(force.normalized * initialForce);
    }

    public void ResetBall()
    {
        trail.Clear();
        trail.enabled = false;
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero;
    }

    public void RestartGame()
    {
        isFireball = false;
        elapsedTime = 0;
        ResetBall();
        Invoke(nameof(PushBall), 2);
    }

    public Vector2 TrajectoryOrigin
    {
        get { return trajectoryOrigin; }
    }
}
