using UnityEngine;

public class Trajectory : MonoBehaviour
{

    public BallController ball;
    public CircleCollider2D ballCollider;
    public Rigidbody2D ballRb;
    public GameObject ballAtCollision;

    private bool drawBallAtCollision = false;
    private Vector2 offsetHitPoint = new Vector2();

    void Start()
    {
        ballRb = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        RaycastHit2D[] circleCastHitArray = Physics2D.CircleCastAll(ballRb.position, ballCollider.radius, ballRb.velocity.normalized);
        foreach (RaycastHit2D circleCastHit in circleCastHitArray)
        {
            // Jika terjadi tumbukan, dan tumbukan tersebut tidak dengan bola 
            // (karena garis lintasan digambar dari titik tengah bola)...
            if (circleCastHit.collider != null && circleCastHit.collider.GetComponent<BallController>() == null)
            {
                Vector2 hitPoint = circleCastHit.point;
                Vector2 hitNormal = circleCastHit.normal;
                offsetHitPoint = hitPoint + hitNormal * ballCollider.radius;
                DottedLine.DottedLine.Instance.DrawDottedLine(ball.transform.position, offsetHitPoint);
                
                if (circleCastHit.collider.GetComponent<SideWall>() == null)
                {
                    // Hitung vektor datang
                    Vector2 inVector = (offsetHitPoint - ball.TrajectoryOrigin).normalized;

                    // Hitung vektor keluar
                    Vector2 outVector = Vector2.Reflect(inVector, hitNormal);

                    // Hitung dot product dari outVector dan hitNormal. Digunakan supaya garis lintasan ketika 
                    // terjadi tumbukan tidak digambar.
                    float outDot = Vector2.Dot(outVector, hitNormal);
                    if (outDot > -1.0f && outDot < 1.0)
                    {
                        // Gambar lintasan pantulannya
                        DottedLine.DottedLine.Instance.DrawDottedLine(
                            offsetHitPoint,
                            offsetHitPoint + outVector * 10.0f);

                        // Untuk menggambar bola "bayangan" di prediksi titik tumbukan
                        drawBallAtCollision = true;
                    }
                }
            }

        }

        if (drawBallAtCollision)
        {
            // Gambar bola "bayangan" di prediksi titik tumbukan
            ballAtCollision.transform.position = offsetHitPoint;
            ballAtCollision.SetActive(true);
        }
        else
        {
            // Sembunyikan bola "bayangan"
            ballAtCollision.SetActive(false);
        }
    }
}
