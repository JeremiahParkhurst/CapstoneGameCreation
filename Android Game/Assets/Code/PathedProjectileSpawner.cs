using UnityEngine;

/*
* Resource:
*
* The class will allow GameObjects to spawn Projectile Objects towards a given destination.
* The number of projectiles fired is dependent on the FireRate and the _nextShotInSeconds.
* The desintation can be set to anything, and the projectiles will travel until it reached
* its destination. Once it reaches its destination, it will destroy the projectile and fire
* more projectiles again.
*/
public class PathedProjectileSpawner : MonoBehaviour {

    public Transform Destination;           // the location where the projectile will travel to
    public PathedProjectile Projectile;     // the projectile shot
    public GameObject SpawnEffect;          // effect played when spawning the projectile

    public float Speed;                     // the travel speed of the projectile towards its destination
    public float FireRate;                  // the rate of shots the projectile will be fired at
    public AudioClip SpawnProjectileSound;  // the sound of the projectile spawning

    private float _nextShotInSeconds;       // the cooldown before firing another shot

    Animator anim;                          // animation variable

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();    // initializes the animations
        _nextShotInSeconds = FireRate;
	}
	
	// Update is called once per frame
	void Update () {

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
	}

    // Visual indicator for line of travel for the projectile
    public void OnDrawGizmos()
    {
        if (Destination == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Destination.position);
    }
}
