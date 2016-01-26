using UnityEngine;

/*
* Resource: https://www.youtube.com/watch?v=-yQjn9ekh5I
* 
* This class is used for flying enemies. The flying enemy will follow the player
* and used alongside with conjunction of GiveDamageToPlayer, will allow this GameObject
* to damage and hinder the player's movements.
*
* TODO: Handle respawn of this enemy in a default position, when the player dies,
* and to respawn enemies tagged with this script.
*/
public class FlyingEnemyAI : MonoBehaviour {

    private Player player;          // instance of the player class
    public float speed;             // the movement speed of this GameObject
    public float playerRange;       // the distance between the Player Object and this GameObject
    public LayerMask playerLayer;   // the Player Object's layer
    public bool IsInRange;          // used to determine if the Player Object is in range of this GameObject
    public bool facingAway;
    public bool followOnLookAway;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {

        // Determines if the Player Object is in range of view of this GameObject
        IsInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);

        if (!followOnLookAway)
        {
            if (IsInRange)
            {
                // Handles how this GameObject will approach the Player Object
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                return;
            }
        }

        // If the Player Object is on the left of this GameObject and is facing away  or vice versa
        if ((player.transform.position.x < transform.position.x && transform.localScale.x < 0) || (player.transform.position.x > transform.position.x && transform.localScale.x > 0))
        {
            facingAway = true;
        }
        else
            facingAway = false;

        // If the Player Object is in range of this GameObject, and they are facing away, move this GameObject towards the PlayerObject
       if(IsInRange && facingAway)
        {
            // Handles how this GameObject will approach the Player Object
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            return;
        }
    }

    // Method draws a sphere indicating the range of view of this GameObject
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, playerRange);
    }
}
