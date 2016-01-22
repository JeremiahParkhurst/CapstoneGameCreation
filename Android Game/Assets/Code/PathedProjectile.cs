using UnityEngine;
using System.Collections;

public class PathedProjectile : MonoBehaviour {

    private Transform _destination;
    private float _speed;
    public AudioClip DestroySound;

    public GameObject DestroyEffect;

    public void Initialize(Transform destination, float speed)
    {
        _destination = destination;
        _speed = speed;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * _speed);
        var distanceSquared = (_destination.transform.position - transform.position).sqrMagnitude;
        if (distanceSquared > 0.1f * 0.01f)
            return;
        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        if (DestroySound != null)
            AudioSource.PlayClipAtPoint(DestroySound, transform.position);

        Destroy(gameObject);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}