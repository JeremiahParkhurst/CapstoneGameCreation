﻿using UnityEngine;

/*
* This is the player class which handles movement, and controls of the player.
* The player has a set amount of health which can be decremented by another
* GameObject. If the Player object's health reaches zero, the Kill method
* will be invoked, and the player will respawn at the respawn point. 
*/
public class Player : MonoBehaviour, ITakeDamage {

    private bool _isFacingRight;                    // checks if the player is facing right
    private CharacterController2D _controller;      // instance of the CharacterController2D
    private float _normalizedHorizontalSpeed;       // -1 if left, 1 if right

    public float MaxSpeed = 8;                      // max speed of the player
    public float SpeedAccelerationOnGround = 10f;   // how quickly the player goes from moving to not moving on ground
    public float SpeedAccelerationInAir = 5f;       // how quickly the player goes from moving to not moving on air
    public int MaxHealth = 100;                     // maximum health of the player
    public GameObject OuchEffect;                   // effect played when the player is receiving damage
    public Projectile Projectile;                   // the player's projectile
    public float FireRate;                          // cooldown after firing a projectile
    public Transform ProjectileFireLocation;        // the location of which the projectile is fired at
    public GameObject FireProjectileEffect;         // the effect played when the player is shooting

    public AudioClip PlayerHitSound, PlayerShootSound, PlayerHealthSound, PlayerDeathSound;

    public int Health { get; private set; }
    public bool IsDead { get; private set; }

    private float _canFireIn; // tracks when the player can fire

    // Use this for initialization
    public void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _isFacingRight = transform.localScale.x > 0;
        Health = MaxHealth;
    }

    // Update is called once per frame
    public void Update()
    {
        _canFireIn -= Time.deltaTime; // When this reaches 0, they player can shoot again

        if(!IsDead)
            HandleInput(); // Handles what the player press (left, right, jump, shoot)

        // Changes movement factor depending on if the Player object is falling in midair, or when it is grounded
        var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;

        // Handles horizontal velocity
        _controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));
    }

    /*
    * Method invoked when the player's health reaches zero.
    */
    public void Kill()
    {
        // Sound
        AudioSource.PlayClipAtPoint(PlayerDeathSound, transform.position);

        _controller.HandleCollisions = false; // player will fall through object
        GetComponent<Collider2D>().enabled = false; // collider2D.enabled = false;
        IsDead = true;
        Health = 0;

        _controller.SetForce(new Vector2(0, 20)); // and bounces player up
    }

    /*
    * @param spawnPoint, the location where the player is respawned
    * Method called to respawn the player, and re-initializes the Player's beginning state.
    */
    public void RespawnAt(Transform spawnPoint)
    {
        // Ensures that player is always facing right upon respawn
        if(!_isFacingRight)
            Flip();

        IsDead = false;
        GetComponent<Collider2D>().enabled = true; // collider2D.enabled = true;
        _controller.HandleCollisions = true;
        Health = MaxHealth; // sets current health to the Player object's max health

        transform.position = spawnPoint.position; // respawns the player at the spawnPoint
    }

    /*
    * @param damage, the damage taken by the Player object
    * @param instigator, the GameObject initializing the damage dealt to the Player object
    * Method to decrement the player's health when they are hit/damaged by an enemy/trap
    */
    public void TakeDamage(int damage, GameObject instigator)
    {
        // Floating text
        FloatingText.Show(string.Format("-{0}", damage), "PlayerTakeDamageText", new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));

        // Sound
        AudioSource.PlayClipAtPoint(PlayerHitSound, transform.position);

        // Decrement's the player's health
        Instantiate(OuchEffect, transform.position, transform.rotation);
        Health -= damage;

        // If the player's Health reaches zero, call KillPlayer
        if (Health <= 0)
            LevelManager.Instance.KillPlayer();
    }

    /*
    * Method that allows the Player object to recover lost health.
    * @param health, the health of the player
    * @param instigator, the GameObject initializing the health recovery
    */
    public void GiveHealth(int health, GameObject instigator)
    {
        // Sound
        AudioSource.PlayClipAtPoint(PlayerHealthSound, transform.position);

        // Floating text
        FloatingText.Show(string.Format("+{0}", health), "PlayerGotHealthText", new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));

        // Increment's the player's health
        Health = Mathf.Min(Health + health, MaxHealth);
    }

    /*
    * Method that allows the user to control the Player object.
    * Left = A
    * Right = D
    * Space = Jump
    * Left Click = Shoot
    *    
    * TODO: Up = Climb Ladder
    */
    private void HandleInput()
    {
        // Handle right direction, and changing the Player object's sprite to match
        if (Input.GetKey(KeyCode.D))
        {
            _normalizedHorizontalSpeed = 1;
            if (!_isFacingRight)
                Flip();
        }

        // Handle left direction, and changing the Player object's sprite to match
        else if (Input.GetKey(KeyCode.A))
        {
            _normalizedHorizontalSpeed = -1;
            if (_isFacingRight)
                Flip();
        }

        // If the player is not moving (not pressing 'A' or 'D')
        else
        {
            _normalizedHorizontalSpeed = 0;
        }

        // Handles jumping
        if(_controller.CanJump && Input.GetKeyDown(KeyCode.Space))
        {
            _controller.Jump();
        }

        // Handles shooting
        if (Input.GetMouseButton(0))
            FireProjectile();
    }

    /*
    * Method that determines when the Player object can fire. 
    * Handles instantiation and initialize, direction of the projectile and resets canFireIn.
    */
    public void FireProjectile()
    {
        // If the cooldown is still counting down to 0, the player cannot fire.
        if (_canFireIn > 0)
            return;

        if (FireProjectileEffect != null)
        {
            // Plays the effect in the direction the player is facing
            var effect = (GameObject)Instantiate(FireProjectileEffect, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
            effect.transform.parent = transform;            
        }

        // Check direction to ensure projectiles are firing in the same direction as the Player class
        var direction = _isFacingRight ? Vector2.right : -Vector2.right;

        // Instantiates the projectile, and initilializes the speed, and direction of the projectile
        var projectile = (Projectile)Instantiate(Projectile, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
        projectile.Initialize(gameObject, direction, _controller.Velocity);
        _canFireIn = FireRate; // time frame, when projectiles can be shot from this GameObject

        // Reflects the projectile to match the direction the player is facing
        //projectile.transform.localScale = new Vector3(_isFacingRight ? 1 : -1, 1, 1);

        // Sound
        AudioSource.PlayClipAtPoint(PlayerShootSound, transform.position);
    }

    /*
    * Method to vertically flip the Player object's sprite
    */
    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        _isFacingRight = transform.localScale.x > 0;
    }
}
