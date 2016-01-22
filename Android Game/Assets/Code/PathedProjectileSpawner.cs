using UnityEngine;
using System.Collections;

public class PathedProjectileSpawner : MonoBehaviour {

    public Transform Destination;
    public PathedProjectile Projectile;
    public GameObject SpawnEffect;

    public float Speed;
    public float FireRate;
    public AudioClip SpawnProjectileSound;

    private float _nextShotInSeconds;

    Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        _nextShotInSeconds = FireRate;
	}
	
	// Update is called once per frame
	void Update () {

        // Use this for animation
       // float move = Input.GetAxis("Horizontal");
       // anim.SetFloat("Speed", Mathf.Abs(move));

        // Spawner code
        if ((_nextShotInSeconds -= Time.deltaTime) > 0)
            return;

        _nextShotInSeconds = FireRate;
        var projectile = (PathedProjectile)Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initialize(Destination, Speed);

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
