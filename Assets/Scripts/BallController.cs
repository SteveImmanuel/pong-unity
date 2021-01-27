using UnityEngine;

public class BallController : MonoBehaviour
{
    public float initialForce = 25f;

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


    private void PushBall()
    {
        float yRandomForce = Random.Range(-1f, 1f);
        float xRandomForce = Random.Range(-1f, 1f);
        Vector2 force = new Vector2(xRandomForce, yRandomForce);

        rb.AddForce(force.normalized * initialForce);
    }

    public void ResetBall()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero;
    }

    public void RestartGame()
    {
        ResetBall();
        Invoke(nameof(PushBall), 2);
    }

    public Vector2 TrajectoryOrigin
    {
        get { return trajectoryOrigin; }
    }
}
