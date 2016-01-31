using UnityEngine;

/*
* Resources: Adapted from PathedProjectileSpawner and SimpleFlyingEnemyAI.
*
* 
*/
public class TurretAI : MonoBehaviour, ITakeDamage, IPlayerRespawnListener
{
    public float Speed;                     // travel speed of this GameObject
    public float FireRate = 1;              // cooldown time after firing a projectile
    public int PointsToGivePlayer;          // points awarded to the player upon killing this GameObject
    public Transform Destination;           // the location where the projectile will travel to
    public float detectionRange;            // the distance between the Player Object and this GameObject
    public PathedProjectile Projectile;     // the projectile shot
    public GameObject SpawnEffect;          // effect played when spawning the projectile
    public GameObject DestroyedEffect;      // the destroyed effect
    public AudioClip SpawnProjectileSound;  // the sound of the projectile spawning
    public Transform RespawnPosition;       // position where this GameObject is respawned at
    public LayerMask CollisionMask;         // determines what this GameObject is colliding with   
    public Transform ProjectileFireLocation;// the location of which the projectile is fired at
    public bool playerInRange;              // used to determine if the Player Object is in range of this GameObject

    private Player player;                      // instance of the player class
    private CharacterController2D _controller;  // has an instance of the CharacterController2D
    private Vector2 _direction;                 // the x-direction of this GameObject
    private Vector2 _startPosition;             // the initial spawn position of this GameObject
    private float _canFireIn;                   // the amount of time this GameObject can shoot projectiles

    // Use this for initialization
    public void Start()
    {
        player = FindObjectOfType<Player>();
        _controller = GetComponent<CharacterController2D>();
        _direction = new Vector2(-1, 0);        // this GameObject will move the left upon initialization
        _startPosition = transform.position;    // initial spawn position of this GameObject     
    }

    // Update is called once per frame
    public void Update()
    {
        // Handles when this GameObject cannot shoot
        if ((_canFireIn -= Time.deltaTime) > 0)
            return;       

        _canFireIn = FireRate;

        // Variable used to determines if the CollisionMask overlaps with the Circle
        playerInRange = Physics2D.OverlapCircle(transform.position, detectionRange, CollisionMask);
        
        // If the player is in range of the turret's attack range, shoot 
        if (playerInRange && !player.IsDead)
        {
            var projectile = (PathedProjectile)Instantiate(Projectile, transform.position, transform.rotation); // initializes the projectile
            projectile.Initialize(Destination, Speed); // moving the projectile
            
            // Handles projectile effects
            if (SpawnEffect != null)
                Instantiate(SpawnEffect, ProjectileFireLocation.position, ProjectileFireLocation.rotation);

            // Sound
            if (SpawnProjectileSound != null)
                AudioSource.PlayClipAtPoint(SpawnProjectileSound, transform.position);            
        }
    }

    // Method draws a sphere indicating the range of view of this GameObject
    public void OnDrawGizmosSelected()
    {
        // Does not draw a line if there is no destination present
        if (Destination == null)
            return;

        // Draws where the PathedProjectile will travel to
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Destination.position);

        // Detection range for this GameObject and the Player Object
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, detectionRange);
    }

    /*
  * @param damage, the damage this GameObject receives
  * @param instigator, the GameObject inflicting damage on this GameObject
  * Handles how this GameObject receives damage from the Player Object's projectiles
  */
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

    /*
    * @param checkpoint, the last checkpoint the Player Object has acquired
    * @param player, the Player Object
    * Method used to respawn this GameObject after the player respawns at the given checkpoint
    */
    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        // Re-initializes this GameObject's direction, and start position
        _direction = new Vector2(-1, 0);
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = _startPosition;

        gameObject.SetActive(true); // shows this GameObject
        transform.position = RespawnPosition.position; // position where this GameObject is respawned at
    }

}
