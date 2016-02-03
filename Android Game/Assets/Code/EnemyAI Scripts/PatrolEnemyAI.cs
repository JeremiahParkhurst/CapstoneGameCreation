using UnityEngine;

/*
* Adapted from: FlyingEnemyAI & SimpleEnemyAI
* 
* This GameObject will draw a sphere around it which detects the certain GameObject
* set by the CollisionMask Layer. If the GameObject has the choosen Layer's property, then
* this GameObject will pursue it, dealing damage upon colliding with it. This GameObject
* will move left and right, changing direction (velocity) and continuing until the Player
* Object leaves this GameObject's sphere. This GameObject is dependent on platforms, in 
* order for this GameObject to switch directions. If there is no platforms, then this
* object will continue traveling until it collides with a platform.
*/
public class PatrolEnemyAI : MonoBehaviour, ITakeDamage, IPlayerRespawnListener
{ 
    private Player player;              // instance of the player class
    public float speed;                 // travel speed of this GameObject
    public GameObject DestroyedEffect;  // the destroyed effect
    public int PointsToGivePlayer;      // points awarded to the player upon killing this GameObject
    public Transform RespawnPosition;   // position where this GameObject is respawned at

    public float detectionRange;        // the distance between the Player Object and this GameObject
    public bool isPlayerInRange;        // used to determine if the Player Object is in range of this GameObject
    public LayerMask CollisionMask;     // determines what this GameObject is colliding with

    private CharacterController2D _controller;  // has an instance of the CharacterController2D
    private Vector2 _direction;                 // the x-direction of this GameObject
    private Vector2 _startPosition;             // the initial spawn position of this GameObject

    // Health
    public int MaxHealth = 100;             // maximum health of the this GameObject
    public int Health { get; private set; } // this GameObject's current health    

    // Sound
    public AudioClip EnemyDestroySound;     // sound played when this GameObject is destroyed

    // Animation
    Animator anim;

    // Use this for initialization
    void Start()
    {
        _controller = GetComponent<CharacterController2D>();    // instance of Charactercontroller2D
        _direction = new Vector2(-1, 0);                        // this GameObject will move the left upon initialization
        _startPosition = transform.position;                    // starting position of this GameObject
        Health = MaxHealth;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        // Sets the x-velocity of this GameObject
        _controller.SetHorizontalForce(_direction.x * speed);

        // Variable used to determines if the CollisionMask overlaps with the Circle
        isPlayerInRange = Physics2D.OverlapCircle(transform.position, detectionRange, CollisionMask);

        // Follows the Player Object of they are in range of this GameObject's sphere
        if (isPlayerInRange)
        {
            // Handles movement of this GameObject

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            return;
        }

        // Checks to see if this GameObject is colliding with something in the same direction
        if ((_direction.x < 0 && _controller.State.IsCollidingLeft) || (_direction.x > 0 && _controller.State.IsCollidingRight))
        {
            _direction = -_direction; // switches direction (velocity)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); // switch sprite direction
        }
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
        Health -= damage;                               // decrement this GameObject's health

        // If this GameObject's health reaches zero
        if (Health <= 0)
        {
            AudioSource.PlayClipAtPoint(EnemyDestroySound, transform.position);
            Health = 0;                                 // sets this GameObject's health to 0 
            gameObject.SetActive(false);                // hides this GameObject
        }
    }

    /*
    * @param checkpoint, the last checkpoint the Player Object has acquired
    * @param player, the Player Object
    * Method used to respawn this GameObject after the player respawns at the given checkpoint
    */
    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        // Re-initializes this GameObject's direction, and start position
        _direction = new Vector2(-1, 0);                // the direction set to left
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = _startPosition;            // intial position of this GameObject

        gameObject.SetActive(true);                     // shows this GameObject
        transform.position = RespawnPosition.position;  // position where this GameObject is respawned at
        
        // Resets health
        Health = MaxHealth;                             // sets current health to the GameObject's max health
    }

    // Method draws a sphere indicating the range of view of this GameObject
    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, detectionRange);
    }
}