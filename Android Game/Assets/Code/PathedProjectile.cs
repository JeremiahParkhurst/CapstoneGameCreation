using UnityEngine;

/*
* This class handles how projectiles are spawn, and their travel distance.
*/
public class PathedProjectile : MonoBehaviour {

    private Transform _destination; // the end point of the projectile
    private float _speed; // the velocity of the projectile

    public AudioClip DestroySound; // Sound
    public GameObject DestroyEffect; // Effects

    // Constructor
    public void Initialize(Transform destination, float speed)
    {
        _destination = destination;
        _speed = speed;
    }
	
	// Update is called once per frame
	void Update () {

        // Handles the travel path of the object
        transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * _speed);
        var distanceSquared = (_destination.transform.position - transform.position).sqrMagnitude;
        if (distanceSquared > 0.1f * 0.01f)
            return;

        // Handles special effects
        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        // Sound
        if (DestroySound != null)
            AudioSource.PlayClipAtPoint(DestroySound, transform.position);

        Destroy(gameObject); // destroys the object
	}

    // Handles what happens when this object collides with another object
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject); // destroys the object
    }
}