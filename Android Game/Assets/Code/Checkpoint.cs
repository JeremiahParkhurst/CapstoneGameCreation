using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Class to determine where the checkpoint is located on the level 
 * and where the player respawns after death.
 */
public class Checkpoint : MonoBehaviour {

    private List<IPlayerRespawnListener> _listeners;

	// Use this for initialization
	public void Awake() {
        _listeners = new List<IPlayerRespawnListener>();
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

        foreach (var listener in _listeners)
            listener.OnPlayerRespawnInThisCheckpoint(this, player);
    }

    public void AssignObjectCheckpoint(IPlayerRespawnListener listener)
    {
        _listeners.Add(listener);
    }
}
