using UnityEngine;
using System.Collections;

public class PathedProjectile : MonoBehaviour {

    private Transform _destination;
    private float _speed;

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

        Destroy(gameObject);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);

    }
}
