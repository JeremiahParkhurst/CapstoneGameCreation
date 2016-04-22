using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    // Projectile
    public Projectile Projectile;                   // the Player Object's projectile
    public float FireRate;                          // cooldown after firing a projectile
    public Transform ProjectileFireLocation;        // the location of which the projectile is fired at
    public GameObject FireProjectileEffect;         // the effect played when the Player Object is shooting
    private float _canFireIn;                       // Player object is able to fire when this equals the FireRate

    public Sprite gunSprite;

    private Player thePlayer;

    // Sound
    public AudioClip PlayerShootSound;

    // Use this for initialization
    void Start () {
        thePlayer = FindObjectOfType<Player>();
        gunSprite = GetComponent<SpriteRenderer>().sprite;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(gunSprite.name);
	}

    /*
    * Method that determines when the Player object can fire. 
    * Handles instantiation and initialize, direction of the projectile and resets canFireIn.
    */
    public void FireProjectile()
    {
        // If the cooldown is still counting down to 0, the player cannot fire.
        if (_canFireIn > 0)
            return;

        if (FireProjectileEffect != null)
        {
            // Plays the effect in the direction the player is facing
            var effect = (GameObject)Instantiate(FireProjectileEffect, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
            effect.transform.parent = transform;
        }

        // Check direction to ensure projectiles are firing in the same direction as the Player class
        var direction = thePlayer._isFacingRight ? Vector2.right : -Vector2.right;

        // Instantiates the projectile, and initilializes the speed, and direction of the projectile
        var projectile = (Projectile)Instantiate(Projectile, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
        projectile.Initialize(gameObject, direction, thePlayer._controller.Velocity);
        _canFireIn = FireRate; // time frame, when projectiles can be shot from this GameObject      

        // Sound
        AudioSource.PlayClipAtPoint(PlayerShootSound, transform.position);

        // Animation
        //Animator.SetTrigger("Shoot");
    }
}
