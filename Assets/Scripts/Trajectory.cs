using UnityEngine;

public class Trajectory : MonoBehaviour
{

    public BallController ball;
    public GameObject ballAtCollision;
    public LineRenderer line;
    public LayerMask layerMask;

    private bool drawBallAtCollision = false;
    private Vector2 offsetHitPoint = new Vector2();
    private CircleCollider2D ballCollider;
    private Rigidbody2D ballRb;

    void Start()
    {
        ballRb = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        RaycastHit2D circleCastHit = Physics2D.CircleCast(ballRb.position, ballCollider.radius, ballRb.velocity.normalized, Mathf.Infinity, layerMask);

        if (circleCastHit.collider != null && circleCastHit.collider.GetComponent<BallController>() == null)
        {
            Vector2 hitPoint = circleCastHit.point;
            Vector2 hitNormal = circleCastHit.normal;
            offsetHitPoint = hitPoint + hitNormal * ballCollider.radius;

            line.positionCount = 2;
            line.SetPosition(0, ball.transform.position);
            line.SetPosition(1, offsetHitPoint);

            if (circleCastHit.collider.GetComponent<SideWall>() == null)
            {
                Vector2 inVector = (offsetHitPoint - ball.TrajectoryOrigin).normalized;

                Vector2 outVector = Vector2.Reflect(inVector, hitNormal);

                float outDot = Vector2.Dot(outVector, hitNormal);
                if (outDot > -1.0f && outDot < 1.0)
                {
                    line.positionCount = 3;
                    line.SetPosition(2, offsetHitPoint + outVector * 10.0f);
                    drawBallAtCollision = true;
                }
            }
        }


        if (drawBallAtCollision)
        {
            ballAtCollision.transform.position = offsetHitPoint;
            ballAtCollision.SetActive(true);
        }
        else
        {
            ballAtCollision.SetActive(false);
        }
    }
}
