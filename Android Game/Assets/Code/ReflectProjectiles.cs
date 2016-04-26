using UnityEngine;

public class ReflectProjectiles : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Projectile>() == null)
            return;

        var projectile = other.GetComponent<Projectile>();
        if (projectile != null && projectile.Owner.GetComponent<Player>() != null)
        {
            projectile.transform.position = Vector3.Reflect(-projectile.Direction, Vector3.right);
        }
    }
}
