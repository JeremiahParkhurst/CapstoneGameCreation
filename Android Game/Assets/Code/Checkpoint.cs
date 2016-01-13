using UnityEngine;
using System.Collections;

/*
 * Class to determine where the checkpoint is located on the level 
 * and where the player respawns after death.
 */
public class Checkpoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void PlayerHitCheckpoint()
    {

    }

    private IEnumerator PlayerHitCheckpointCo(int bonus)
    {
        yield break;
    }

    public void PlayerLeftCheckpoint()
    {

    }

    public void SpawnPlayer(Player player)
    {
        player.RespawnAt(transform);
    }

    public void AssignObjectCheckpoint()
    {

    }
}
