using UnityEngine;

/*
* Resource: www.youtube.com/watch?v=ieXwKpbpGVk&list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n&index=26
*
* This class is used by GameObject who shoot projectiles. The projectiles can be destroyed by
* colliding with another GameObject. The Player Object can destroy projectiles and be awarded 
* a certain amount of points, and a simple sound/effect is played upon the death of a projectile.
*/
public class SinProjectile : Projectile, ITakeDamage
{

    public int Damage;                  // the damage this projectile inflicts
    public GameObject DestroyedEffect;  // the effect played upon the destruction of this GameObject
    public int PointsToGiveToPlayer;    // the amount of points the Player Object receives
    public float TimeToLive;            // the amount of time this GameObject lives
    public AudioClip DestroySound;      // the sound played when this GameObject dies

    public float MoveSpeed = 5.0f;
    public float frequency = 20.0f;  // Speed of sine movement
    public float magnitude = 0.5f;   // Size of sine movement
    private Vector3 axis;
    private Vector3 pos;

    void Start()
    {
        pos = transform.position;
        //DestroyObject(gameObject, 1.0f);
        axis = transform.up;  // May or may not be the axis you want

    }

    // Use this for instantiation
    public void Update()
    {
        // The amount of time this projectile lives
        if ((TimeToLive -= Time.deltaTime) <= 0)
        {
            DestroyProjectile();
            return;
        }

        // Handles the speed of the projectile
        pos += transform.right * Time.deltaTime * MoveSpeed;
        transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
        //transform.Translate(Direction * ((Mathf.Abs(InitialVelocity.x) + Speed) * Time.deltaTime), Space.World);
    }

    /*
    * @param damage, the amount of damage
    * @param instigator, the GameObject inflicting damage
    * Method allows this projectile to deal damage to another GameObject
    */
    public void TakeDamage(int damage, GameObject instigator)
    {
        if (PointsToGiveToPlayer != 0)
        {
            var projectile = instigator.GetComponent<Projectile>();
            if (projectile != null && projectile.Owner.GetComponent<PlayerController>() != null)
            {
                GameManager.Instance.AddPoints(PointsToGiveToPlayer);
                FloatingText.Show(string.Format("+{0}!", PointsToGiveToPlayer), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
            }
        }
    }

    /*
    * @param other, the other GameObject
    * Instance of this projectile is destroyed
    */
    protected override void OnCollideOther(Collider2D other)
    {
        DestroyProjectile();
    }

    /*
    * @param other, the other GameObject
    * @param takeDamage, the amount of damage that the other GameObject receives
    * On collision, the other GameObject takes damage
    */
    protected override void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage)
    {
        takeDamage.TakeDamage(Damage, gameObject);
        DestroyProjectile(); // destroys the projectile
    }

    // Method to destroy the projectile
    private void DestroyProjectile()
    {
        // Handles effects
        if (DestroyedEffect != null)
            Instantiate(DestroyedEffect, transform.position, transform.rotation);

        // Handles Sound
        if (DestroySound != null)
            AudioSource.PlayClipAtPoint(DestroySound, transform.position);

        // Destroys this GameObject
        Destroy(gameObject);
    }
}
