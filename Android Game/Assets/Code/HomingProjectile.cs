using UnityEngine;

public class HomingProjectile : MonoBehaviour {

    public Player target;

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {

        // Handles movement of this GameObject
        transform.rotation = Quaternion.LookRotation(transform.forward);
        transform.LookAt(target.transform.position);
        transform.Translate(Vector3.forward * 1);
    }
}
