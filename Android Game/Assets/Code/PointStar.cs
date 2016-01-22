using UnityEngine;

/* https://www.youtube.com/watch?v=liY46x9Xcls&index=21&list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n */
public class PointStar : MonoBehaviour, IPlayerRespawnListener {

    public GameObject Effect;
    public int PointsToAdd = 10;
    public AudioClip PickupSound;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null)
            return;
        
        // Sound
        if (PickupSound != null)
            AudioSource.PlayClipAtPoint(PickupSound, transform.position);

        GameManager.Instance.AddPoints(PointsToAdd);
        Instantiate(Effect, transform.position, transform.rotation);

        gameObject.SetActive(false); // does not destroy the object, instead makes them invis
    }

    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        gameObject.SetActive(true);
    }
}
