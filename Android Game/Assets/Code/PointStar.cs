using UnityEngine;

/* https://www.youtube.com/watch?v=liY46x9Xcls&index=21&list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n */
public class PointStar : MonoBehaviour, IPlayerRespawnListener {

    public GameObject Effect;
    public int PointsToAdd = 10;
    public AudioClip PickupSound; // sound

    // Handles what happens to the GameObject
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null)
           return;
        
        // Sound
        if (PickupSound != null)
            AudioSource.PlayClipAtPoint(PickupSound, transform.position);

        // Handles points
        GameManager.Instance.AddPoints(PointsToAdd);

        // Handles effects
        Instantiate(Effect, transform.position, transform.rotation);

        gameObject.SetActive(false); // does not destroy the object, instead makes them invis

        // Floating text appears when picked up
        FloatingText.Show(string.Format("+{0}!", PointsToAdd), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
    }

    // Method used to respawn stars after the player dies before reaching the next checkpoint
    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        gameObject.SetActive(true);
    }
}
