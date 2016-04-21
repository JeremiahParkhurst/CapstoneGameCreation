using UnityEngine;
using System.Collections;

public class MonsterPortalSpawner : MonoBehaviour, ITakeDamage, IPlayerRespawnListener
{

    public Transform Destination;           // the location where the projectile will travel to
    public PathedProjectile Projectile;     // the projectile shot
    public GameObject SpawnEffect;          // effect played when spawning the projectile

    public float Speed;                     // the travel speed of the projectile towards its destination
    public float FireRate;                  // the rate of shots the projectile will be fired at
    public AudioClip SpawnProjectileSound;  // the sound of the projectile spawning

    private float _nextShotInSeconds;       // the cooldown before firing another shot

    //public Animator anim;                   // animation

    private CharacterController2D _controller;  // has an instance of the CharacterController2D
    private Vector2 _startPosition;             // the initial spawn position of this GameObject

    // Health
    public int MaxHealth = 100;             // maximum health of the this GameObject
    public int Health { get; private set; } // this GameObject's current health    
    public GameObject DestroyedEffect;      // the destroyed effect of this GameObject
    public int PointsToGivePlayer;          // points awarded to the player upon killing this GameObject
    public Transform RespawnPosition;       // position where this GameObject is respawned at

    // Sound
    public AudioClip ShootSound;            // the sound when this GameObject shoots a projectile
    public AudioClip EnemyDestroySound;     // sound played when this GameObject is destroyed


    // Use this for initialization
    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        _startPosition = transform.position;
        Health = MaxHealth;
        //anim = GetComponent<Animator>();    // initializes the animations
        _nextShotInSeconds = FireRate;
    }

    // Update is called once per frame
    void Update()
    {
        // Spawner code
        if ((_nextShotInSeconds -= Time.deltaTime) > 0)
            return;

        _nextShotInSeconds = FireRate;
        var projectile = (PathedProjectile)Instantiate(Projectile, transform.position, transform.rotation); // initializes the projectile
        projectile.Initialize(Destination, Speed); // moving the projectile

        // Handles projectile effects
        if (SpawnEffect != null)
            Instantiate(SpawnEffect, transform.position, transform.rotation);

        // Sound
        if (SpawnProjectileSound != null)
            AudioSource.PlayClipAtPoint(SpawnProjectileSound, transform.position);

      //if (anim != null)
        //   anim.SetTrigger("Fire");
    }

    // Visual indicator for line of travel for the projectile
    public void OnDrawGizmos()
    {
        if (Destination == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Destination.position);
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
        transform.localScale = new Vector3(1, 1, 1);    
        gameObject.SetActive(true);                     // shows this GameObject   

        // Resets health
        Health = MaxHealth;                             // sets current health to the GameObject's max health
    }
}
