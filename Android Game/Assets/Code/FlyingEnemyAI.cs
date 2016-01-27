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
public class FlyingEnemyAI : MonoBehaviour, ITakeDamage/*, IPlayerRespawnListener */{

    private Player player;          // instance of the player class
    public float Speed;             // the movement speed of this GameObject
    public float PlayerRange;       // the distance between the Player Object and this GameObject
    public LayerMask PlayerLayer;   // the Player Object's layer
    public bool playerInRange;      // used to determine if the Player Object is in range of this GameObject
    public bool facingAway;         // if the Player Object is not facing this GameObject
    //public bool followOnLookAway;

    public GameObject DestroyedEffect;  // the destroyed effect
    public int PointsToGivePlayer;      // points awarded to the player upon killing this GameObject
    public Transform RespawnPosition;   // position where this GameObject is respawned at
   
    private Vector2 _startPosition;     // initial position

    // Use this for initialization
    public void Start () {
        player = FindObjectOfType<Player>();
        transform.position = _startPosition;
    }
	
	// Update is called once per frame
	public void Update () {

        // Variable used to determines if the PlayerLayer overlaps with the Circle
        playerInRange = Physics2D.OverlapCircle(transform.position, PlayerRange, PlayerLayer);
        /*
        if (!followOnLookAway)
        {
            if (playerInRange)
            {
                // Handles how this GameObject will approach the Player Object
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Speed * Time.deltaTime);
                return;
            }
        }*/

        // If the Player Object is on the left of this GameObject and is facing away  or vice versa
        if ((player.transform.position.x < transform.position.x && player.transform.localScale.x < 0) 
            || (player.transform.position.x > transform.position.x && player.transform.localScale.x > 0))
        {
            facingAway = true;
        }
        else
            facingAway = false;

        // If the Player Object is in range of this GameObject, and they are facing away, move this GameObject towards the PlayerObject
       if(playerInRange && facingAway)
        {
            // Handles movement of this GameObject
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Speed * Time.deltaTime);
            return;
        }
    }

    // Method draws a sphere indicating the range of view of this GameObject
    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, PlayerRange);
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        if (PointsToGivePlayer != 0)
        {
            var projectile = instigator.GetComponent<Projectile>();
            if (projectile != null && projectile.Owner.GetComponent<Player>() != null)
            {
                // Handles points
                GameManager.Instance.AddPoints(PointsToGivePlayer);

                // Handles floating text
                FloatingText.Show(string.Format("+{0}!", PointsToGivePlayer), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
            }
        }

        // Effect played upon the death of this GameObject
        Instantiate(DestroyedEffect, transform.position, transform.rotation);

        gameObject.SetActive(false); // hides this GameObject
    }
    
    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        // Re-initializes this GameObject's direction, and start position        
        transform.localScale = new Vector3(1, 1, 1); // insures this GameObject is facing in the right direction
        transform.position = _startPosition;
        gameObject.SetActive(true); // shows this GameObject
        //transform.position = RespawnPosition.position; // position where this GameObject is respawned at
    }
}
