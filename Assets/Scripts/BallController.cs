using UnityEngine;

public class BallController : MonoBehaviour
{
    public float xInitialForce = 15f;
    public float yInitialForce = 15;

    private Rigidbody2D rb;
    private Vector2 trajectoryOrigin;

    void Start()
    {
        trajectoryOrigin = transform.position;
        rb = GetComponent<Rigidbody2D>();
        RestartGame();
    }

    void Update()
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        trajectoryOrigin = transform.position;
    }

    private void ResetBall()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero;
    }

    private void PushBall()
    {
        float yRandomForce = Random.Range(-yInitialForce, yInitialForce);
        float randomDirection = Random.Range(-1f, 1f);
        Vector2 force;

        if (randomDirection >= 0)
        {
            force = new Vector2(xInitialForce, yRandomForce);
        }
        else
        {
            force = new Vector2(-xInitialForce, yRandomForce);
        }

        rb.AddForce(force);
    }

    private void RestartGame()
    {
        ResetBall();
        Invoke(nameof(PushBall), 2);
    }

    public Vector2 TrajectoryOrigin
    {
        get { return trajectoryOrigin; }
    }
}
