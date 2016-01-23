using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, ITakeDamage {

    private bool _isFacingRight;
    private CharacterController2D _controller;
    private float _normalizedHorizontalSpeed; // -1 if left, 1 if right

    public float MaxSpeed = 8; // max speed of the player
    public float SpeedAccelerationOnGround = 10f; // how quickly the player goes from moving to not moving on ground
    public float SpeedAccelerationInAir = 5f; // how quickly the player goes from moving to not moving on air
    public int MaxHealth = 100; // maximum health of the player
    public GameObject OuchEffect;
    public Projectile Projectile; // the player's projectile
    public float FireRate; // cooldown after firing a projectile
    public Transform ProjectileFireLocation;
    public GameObject FireProjectileEffect;

    public AudioClip PlayerHitSound, PlayerShootSound, PlayerHealthSound, PlayerDeathSound;

    public int Health { get; private set; }
    public bool IsDead { get; private set; }

    private float _canFireIn; // tracks when the player can fire

    public void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _isFacingRight = transform.localScale.x > 0;
        Health = MaxHealth;
    }

    public void Update()
    {
        _canFireIn -= Time.deltaTime; // when this reaches 0, they player can shoot again

        if(!IsDead)
            HandleInput(); // handles what the player press (left, right, jump)

        var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;

        _controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));
    }

    public void Kill()
    {
        AudioSource.PlayClipAtPoint(PlayerDeathSound, transform.position);
        _controller.HandleCollisions = false;
        GetComponent<Collider2D>().enabled = false; // collider2D.enabled = false;
        IsDead = true;
        Health = 0;

        _controller.SetForce(new Vector2(0, 20)); // and bounces player up
    }

    public void RespawnAt(Transform spawnPoint)
    {
        if(!_isFacingRight)
            Flip();

        IsDead = false;
        GetComponent<Collider2D>().enabled = true; // collider2D.enabled = true;
        _controller.HandleCollisions = true;
        Health = MaxHealth;

        transform.position = spawnPoint.position;
    }

    // Method to decrement the player's health when they are hit/damaged by an enemy/trap
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
    public void GiveHealth(int health)
    {
        AudioSource.PlayClipAtPoint(PlayerHealthSound, transform.position);
        //FloatingText.Show(string.Format("+{0}", health)
        Health = Mathf.Min(Health + health, MaxHealth);
    }*/

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            _normalizedHorizontalSpeed = 1;
            if (!_isFacingRight)
                Flip();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _normalizedHorizontalSpeed = -1;
            if (_isFacingRight)
                Flip();
        }
        else
        {
            _normalizedHorizontalSpeed = 0;
        }

        if(_controller.CanJump && Input.GetKeyDown(KeyCode.Space))
        {
            _controller.Jump();
        }

        if (Input.GetMouseButton(0))
            FireProjectile();
    }

    /*
    * Method that determines when to fire, the direction of the projectile
    * Handles instantiation and initialize, and resets canFireIn.
    */
    public void FireProjectile()
    {
        if (_canFireIn > 0)
            return;

        if (FireProjectileEffect != null)
        {
            // Plays the effect in the direction the player is facing
            var effect = (GameObject)Instantiate(FireProjectileEffect, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
            effect.transform.parent = transform;
            //Instantiate(FireProjectileEffect, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
        }
        var direction = _isFacingRight ? Vector2.right : -Vector2.right;
        var projectile = (Projectile)Instantiate(Projectile, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
        projectile.Initialize(gameObject, direction, _controller.Velocity);
        _canFireIn = FireRate;

        // Reflects the projectile to match the direction the player is facing
        //projectile.transform.localScale = new Vector3(_isFacingRight ? 1 : -1, 1, 1);

        // Sound
        AudioSource.PlayClipAtPoint(PlayerShootSound, transform.position);
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        _isFacingRight = transform.localScale.x > 0;
    }
}
