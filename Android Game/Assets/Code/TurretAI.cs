using UnityEngine;

public class TurretAI : MonoBehaviour, ITakeDamage, IPlayerRespawnListener
{
    public float Speed;                 // travel speed of this GameObject
    public float FireRate = 1;                  // cooldown time after firing a projectile
    public Projectile Projectile;               // this GameObject's projectile
    public GameObject DestroyedEffect;  // the destroyed effect
    public int PointsToGivePlayer;      // points awarded to the player upon killing this GameObject
    public Transform RespawnPosition;   // position where this GameObject is respawned at

    private Player player;          // instance of the player class
    public LayerMask CollisionMask; // determines what this GameObject is colliding with
    public float detectionRange;       // the distance between the Player Object and this GameObject
    public bool playerInRange;      // used to determine if the Player Object is in range of this GameObject

    private CharacterController2D _controller;  // has an instance of the CharacterController2D
    private Vector2 _direction;                 // the x-direction of this GameObject
    private Vector2 _startPosition;             // the initial spawn position of this GameObject
    private float _canFireIn;                   // the amount of time this GameObject can shoot projectiles

    public Transform ProjectileFireLocation;    // the location of which the projectile is fired at


    // Use this for initialization
    public void Start()
    {
        player = FindObjectOfType<Player>();
        _controller = GetComponent<CharacterController2D>();
        _direction = new Vector2(-1, 0);    // this GameObject will move the left upon initialization
        _startPosition = transform.position;
    }

    // Update is called once per frame
    public void Update()
    {
        // Handles when this GameObject cannot shoot
        if ((_canFireIn -= Time.deltaTime) > 0)
            return;

        // Variable used to determines if the CollisionMask overlaps with the Circle
        playerInRange = Physics2D.OverlapCircle(transform.position, detectionRange, CollisionMask);

        // If the player is in range of the turret's attack range, shoot 
        if (playerInRange && !player.IsDead)
        {
            // Instantiates the projectile, and initilializes the speed, and direction of the projectile
            var projectile = (Projectile)Instantiate(Projectile, ProjectileFireLocation.position, ProjectileFireLocation.rotation);

            //projectile.transform.rotation = Quaternion.LookRotation(player.transform.position);
            //projectile.transform.LookAt(player.transform.position);
            //projectile.transform.Translate(Vector3.forward * Time.deltaTime * 2);
            projectile.Initialize(gameObject, _direction, _controller.Velocity); // projectile moves
            
            _canFireIn = FireRate; // time frame, when projectiles can be shot from this GameObject
        }
    }

    // Method draws a sphere indicating the range of view of this GameObject
    public void OnDrawGizmosSelected()
    {
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
        gameObject.SetActive(false);                    // hides this GameObject
    }

    /*
    * @param checkpoint, the last checkpoint the Player Object has acquired
    * @param player, the Player Object
    * Method used to respawn this GameObject after the player respawns at the given checkpoint
    */
    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        transform.position = _startPosition;
        gameObject.SetActive(true);                     // shows this GameObject
        transform.position = RespawnPosition.position;  // position where this GameObject is respawned at
    }
}
