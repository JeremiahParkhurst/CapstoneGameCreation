using UnityEngine;
using System.Collections;

public class CrumblingPlatform : MonoBehaviour, IPlayerRespawnListener
{
    private Rigidbody2D _rigidbody2D;
    public float FallDelay;
    public float DisappearDelay;

    private CharacterController2D _controller;  // has an instance of the CharacterController2D
    public Transform RespawnPosition;           // position where this GameObject is respawned at
    private Vector2 _startPosition;             // the initial spawn position of this GameObject

    // Use this for initialization
    void Start() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null)
            return;
       
        StartCoroutine(Fall());
        StartCoroutine(Delay());
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(FallDelay);
        _rigidbody2D.isKinematic = false;
        //GetComponent<Collider2D>().isTrigger = true;
        yield return 0;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(DisappearDelay);
        gameObject.SetActive(false);
        yield return 0;
    }

    /*
    * @param checkpoint, the last checkpoint the Player Object has acquired
    * @param player, the Player Object
    * Method used to respawn this GameObject after the player respawns at the given checkpoint
    */
    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        //transform.position = _startPosition;            // initial position of this GameObject
        gameObject.SetActive(true);                     // shows this GameObject
        //transform.position = RespawnPosition.position;  // position where this GameObject is respawned at
        _rigidbody2D.isKinematic = true;
    }

}
