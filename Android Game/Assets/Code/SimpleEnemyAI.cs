using UnityEngine;
using System.Collections;
/* https://www.youtube.com/watch?v=re6fookKraU&index=28&list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n */
public class SimpleEnemyAI : MonoBehaviour, ITakeDamage/*, IPlayerRespawnListener*/ {

    /* player projectile https://youtu.be/re6fookKraU?list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n&t=1092 */
    public float Speed; // Travel speed
    public float FireRate = 1; // Cooldown after firing a projectile
    public Projectile Projectile;
    public GameObject DestroyedEffect;
    public int PointsToGivePlayer;
    public AudioClip ShootSound;

    private CharacterController2D _controller;
    private Vector2 _direction;
    private Vector2 _startPosition;
    private float _canFireIn;

	// Use this for initialization
	void Start () {
        _controller = GetComponent<CharacterController2D>();
        _direction = new Vector2(-1, 0);
        _startPosition = transform.position;
	}
	
	// Update is called once per frame
	public void Update () {
        _controller.SetHorizontalForce(_direction.x * Speed);

        // Checks to see if this GameObject is colliding with something in the same direction
        if ((_direction.x < 0 && _controller.State.IsCollidingLeft) || (_direction.x > 0 && _controller.State.IsCollidingRight))
        {
            _direction = -_direction; // switches direction
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if ((_canFireIn -= Time.deltaTime) > 0)
            return;

        // Casts rays to detect player
        var raycast = Physics2D.Raycast(transform.position, _direction, 10, 1 << LayerMask.NameToLayer("Player"));
        if (!raycast)
            return;
        var projectile = (Projectile)Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initialize(gameObject, _direction, _controller.Velocity);
        _canFireIn = FireRate;

        // Sound
        if(ShootSound != null)
            AudioSource.PlayClipAtPoint(ShootSound, transform.position);
	}

    // 
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
        // Handles effects and hides this GameObject
        Instantiate(DestroyedEffect, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    // Method to handle respawning of this Enemey
    public void OnPlayerRespawnInThisCheckPoint(Checkpoint checkpoint, Player player)
    {
        _direction = new Vector2(-1, 0);
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = _startPosition;
        gameObject.SetActive(true);
    }
}
