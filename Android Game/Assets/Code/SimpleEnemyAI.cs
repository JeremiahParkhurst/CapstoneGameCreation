﻿using UnityEngine;

/* 
* Resource: https://www.youtube.com/watch?v=re6fookKraU&index=28&list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n 
* 
* This class handles how a generic enemy AI preforms. This class implements an instance of the
* CharacterController2D, which is used to check collisions, and the speed/velocity of this GameObject.
* This GameObject will also be able to shoot projectiles at the player. The player is rewarded points 
* for killing this GameObject, and can be awarded poitns form killing this GameObject's projectiles.
*/
public class SimpleEnemyAI : MonoBehaviour, ITakeDamage, IPlayerRespawnListener {

    /* player projectile https://youtu.be/re6fookKraU?list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n&t=1092 */
    public float Speed;                 // travel speed of this GameObject
    public float FireRate = 1;          // cooldown time after firing a projectile
    public Projectile Projectile;       // this GameObject's projectile
    public GameObject DestroyedEffect;  // the destroyed effect
    public int PointsToGivePlayer;      // points awarded to the player upon killing this GameObject
    public AudioClip ShootSound;        // the sound when this GameObject shoots a projectile

    private CharacterController2D _controller;  // has an instance of the CharacterController2D
    private Vector2 _direction;                 // the x-direction of this GameObject
    private Vector2 _startPosition;             // the spawn position of this GameObject
    private float _canFireIn;                   // the amount of time this GameObject can shoot projectiles

	// Use this for initialization
	void Start () {
        _controller = GetComponent<CharacterController2D>();
        _direction = new Vector2(-1, 0);
        _startPosition = transform.position;
	}
	
	// Update is called once per frame
	public void Update () {

        // Sets the x-velocity of this GameObject
        _controller.SetHorizontalForce(_direction.x * Speed);

        // Checks to see if this GameObject is colliding with something in the same direction
        if ((_direction.x < 0 && _controller.State.IsCollidingLeft) || (_direction.x > 0 && _controller.State.IsCollidingRight))
        {
            _direction = -_direction; // switches direction
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        // Handles when this GameObject cannot shoot
        if ((_canFireIn -= Time.deltaTime) > 0)
            return;

        // Casts rays to detect player
        var raycast = Physics2D.Raycast(transform.position, _direction, 10, 1 << LayerMask.NameToLayer("Player"));
        if (!raycast)
            return;

        // Instantiates the projectile, and initilializes the speed, and direction of the projectile
        var projectile = (Projectile)Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initialize(gameObject, _direction, _controller.Velocity);
        _canFireIn = FireRate; // time frame, when projectiles can be shot from this GameObject

        // Handles Sound
        if(ShootSound != null)
            AudioSource.PlayClipAtPoint(ShootSound, transform.position);
	}

    // Handles how this GameObject receives damage
    public void TakeDamage(int damage, GameObject instigator)
    {
        if(PointsToGivePlayer != 0)
        {
            var projectile = instigator.GetComponent<Projectile>();
            if(projectile != null && projectile.Owner.GetComponent<Player>() != null)
            {
                // Handles points
                GameManager.Instance.AddPoints(PointsToGivePlayer);

                // Handles floating text
                FloatingText.Show(string.Format("+{0}!", PointsToGivePlayer), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
            }
        }

        // Effect played upon the death of this GameObject
        Instantiate(DestroyedEffect, transform.position, transform.rotation);
         
        gameObject.SetActive(false); // hides this GameObject
    }

    // Method used to respawn this GameObject after the player respawns at the given checkpoint
    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        // Re-initializes this GameObject's direction, and start position
        _direction = new Vector2(-1, 0);
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = _startPosition;
        gameObject.SetActive(true); // shows this GameObject
    }
}
