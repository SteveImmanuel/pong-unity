using UnityEngine;

public class SideWall : MonoBehaviour
{
    public PlayerController player;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Ball")
        {
            player.IncrementScore();

            if (player.Score < gameManager.maxScore)
            {
                collision.gameObject.SendMessage("RestartGame", SendMessageOptions.RequireReceiver);
            }
        }
    }
}
