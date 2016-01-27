using UnityEngine;
using System.Collections;

/*
* Resource: https://www.youtube.com/watch?v=KBSHz-ee8Sk&index=21&list=PLiyfvmtjWC_Up8XNvM3OSqgbJoMQgHkVz
*
*
*/
public class LadderController2D : MonoBehaviour {

    private Player player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
	}

    // Handles collision
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            player.onLadder = false;
        }
    }
}
