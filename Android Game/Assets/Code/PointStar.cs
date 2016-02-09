﻿using UnityEngine;

/* 
* Resource: https://www.youtube.com/watch?v=liY46x9Xcls&index=21&list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n 
* 
* This class is applied to GameObjects the player can collect throughout the level.
* Upon colliding with this GameObject, the player will be awarded a set amount of
* points. This will also trigger an affect, before hiding this GameObject, and a 
* sound.
*/
public class PointStar : MonoBehaviour, IPlayerRespawnListener {

    public GameObject Effect;       // special effects upon colliding with the GameObject
    public int PointsToAdd = 10;    // the number of points the player is rewarded
    public AudioClip PickupSound;   // sound played when the player collides this GameObject
    public Animator anim;
    public SpriteRenderer rend;

    private bool _isCollected;

    /*
    * @param other, the other GameObject
    * Handles what happens to the GameObject
    */
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCollected)
            return;

        // Does nothing if another GameObject collides with this GameObject
        if (other.GetComponent<Player>() == null)
           return;
        
        // Handles Sound
        if (PickupSound != null)
            AudioSource.PlayClipAtPoint(PickupSound, transform.position);

        // Handles points awarded
        GameManager.Instance.AddPoints(PointsToAdd);

        // Handles effects
        Instantiate(Effect, transform.position, transform.rotation);       

        // Floating text appears when picked up
        FloatingText.Show(string.Format("+{0}!", PointsToAdd), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));

        _isCollected = true;
        anim.SetTrigger("Collect");           
    }

    // Method used to set this GameObject to false when animation is done
    public void FinishAnimationEvent()
    {
        //gameObject.SetActive(false); // hides this GameObject
        rend.enabled = false;
        anim.SetTrigger("Reset");
    }

    /*
    * @param checkpoint, the most recent checkpoint the Player Object has acquired
    * @param player, the Player Object
    * Method used to respawn this GameObject after the player respawns at the given checkpoint
    */
    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        _isCollected = false;
        gameObject.SetActive(true); // shows this GameObject
    }
}
