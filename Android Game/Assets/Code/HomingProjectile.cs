using UnityEngine;
using System.Collections;

public class HomingProjectile : MonoBehaviour {

    private Player player;          // instance of the player class
    public float Speed;             // the movement speed of this GameObject

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        
        // Handles movement of this GameObject
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Speed * Time.deltaTime);
        return;
    }
}
