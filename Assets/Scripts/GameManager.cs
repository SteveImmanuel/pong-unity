using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player1;
    public PlayerController player2;
    public BallController ball;
    public GameObject trajectory;
    public int maxScore;
    public UIController uIController;

    private Rigidbody2D ballRb;
    private CircleCollider2D ballCollider;
    private bool isDebugWindowShown = false;
    private int player1Score;
    private int player2Score;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        ballRb = ball.GetComponent<Rigidbody2D>();
        ballCollider = ball.GetComponent<CircleCollider2D>();
        InitializeGame();
    }

    private void InitializeGame()
    {
        uIController.ShowControlInfo();
        player1Score = player2Score = 0;
        RestartGame();
    }

    private void RestartGame()
    {
        ball.RestartGame();
        player1.ResetPlayer();
        player2.ResetPlayer();
    }

    public void FireBallMode()
    {
        player1.ChangeRbType();
        player2.ChangeRbType();
    }

    public void IncrementScore(string tag)
    {
        if (tag == "Player1")
        {
            player1Score++;
        }
        else if (tag == "Player2")
        {
            player2Score++;
        }
        else
        {
            Debug.LogError("Unknown Player Tag:" + tag);
        }

        if (player1Score == maxScore || player2Score == maxScore)
        {
            ball.ResetBall();
        }
        else
        {
            RestartGame();
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" + player1Score);
        GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + player2Score);

        // Tombol restart untuk memulai game dari awal
        if (GUI.Button(new Rect(Screen.width / 2 - 60, 35, 120, 53), "RESTART"))
        {
            InitializeGame();
        }

        if (player1Score == maxScore)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 10, 2000, 1000), "PLAYER ONE WINS");
        }
        else if (player2Score == maxScore)
        {
            GUI.Label(new Rect(Screen.width / 2 + 30, Screen.height / 2 - 10, 2000, 1000), "PLAYER TWO WINS");
        }

        if (isDebugWindowShown)
        {
            //Color oldColor = GUI.backgroundColor;
            //GUI.backgroundColor = Color.red;

            // Simpan variabel-variabel fisika yang akan ditampilkan. 
            float ballMass = ballRb.mass;
            Vector2 ballVelocity = ballRb.velocity;
            float ballSpeed = ballRb.velocity.magnitude;
            Vector2 ballMomentum = ballMass * ballVelocity;
            float ballFriction = ballCollider.friction;

            float impulsePlayer1X = player1.LastContactPoint.normalImpulse;
            float impulsePlayer1Y = player1.LastContactPoint.tangentImpulse;
            float impulsePlayer2X = player2.LastContactPoint.normalImpulse;
            float impulsePlayer2Y = player2.LastContactPoint.tangentImpulse;

            // Tentukan debug text-nya
            string debugText =
                "Ball mass = " + ballMass + "\n" +
                "Ball velocity = " + ballVelocity + "\n" +
                "Ball speed = " + ballSpeed + "\n" +
                "Ball momentum = " + ballMomentum + "\n" +
                "Ball friction = " + ballFriction + "\n" +
                "Last impulse from player 1 = (" + impulsePlayer1X + ", " + impulsePlayer1Y + ")\n" +
                "Last impulse from player 2 = (" + impulsePlayer2X + ", " + impulsePlayer2Y + ")\n";
            // Tampilkan debug window
            GUIStyle guiStyle = new GUIStyle(GUI.skin.textArea);
            guiStyle.alignment = TextAnchor.UpperCenter;
            GUI.TextArea(new Rect(Screen.width / 2 - 200, Screen.height - 200, 400, 110), debugText, guiStyle);
            //GUI.backgroundColor = oldColor;
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height - 73, 120, 53), "TOGGLE\nDEBUG INFO"))
        {
            isDebugWindowShown = !isDebugWindowShown;
            trajectory.SetActive(!trajectory.activeSelf);
        }
    }
}
