using UnityEngine;

public class SideWall : MonoBehaviour
{
    public GameObject playerScorer;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ball")
        {
            gameManager.IncrementScore(playerScorer.tag);
        }
    }
}
