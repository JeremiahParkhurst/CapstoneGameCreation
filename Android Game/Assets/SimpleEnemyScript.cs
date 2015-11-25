using UnityEngine;
using System.Collections;
//www.youtube.com/watch?v=DKQCplBJ4yE&index=16&list=PLQzQtnB2ciXRvU5GRn4mTLlz21kSVg9XN
public class SimpleEnemyScript : MonoBehaviour {

    public float velocity = 1f;
    public Transform sightStart;
    public Transform sightEnd;
    public LayerMask detectWhat;
    public bool colliding;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, GetComponent<Rigidbody2D>().velocity.y);

        colliding = Physics2D.Linecast(sightStart.position, sightEnd.position, detectWhat);

        // If the enemy collides with
        if (colliding)
        {
            // Mirrors the enemy and change the direction path of the enemy
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            velocity *= -1;
        }
	}
}
