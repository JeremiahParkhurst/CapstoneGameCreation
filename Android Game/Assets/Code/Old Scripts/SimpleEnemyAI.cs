using UnityEngine;
using System.Collections;
//https://www.youtube.com/watch?v=re6fookKraU&index=28&list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n
public class SimpleEnemyAI : MonoBehaviour {//, ITakeDamage, IPlayerRespawnListener {
    /*
    public float Speed;
    public float FireRate = 1;
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
        _controller.SetHorizonalForce(_direction.x * Speed);
        if((_direction.x < 0 && _controller.IsCollidingLeft) || (_direction.x > 0 && _controller.IsCollidingRight))
        {
            _direction = -_direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if ((_cantFireIn -= Time.deltaTime) > 0)
            return;

        var raycast = Physics2D.Raycast(transform.position, _direction, 10, 1 << LayerMask.NameToLayer("Player"));
        if (!raycast)
            return;
        var projectile = (Projectile)Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initialize(gameObject, _direction, _controller.Velocity);
        _canFireIn = FireRate;

        if(ShootSound != null
            AudioSource.PlayClipAtPoint(ShootSound, transform.position);
	}

    public void TakeDamage(int damage, GameObject instigator)
    {
        Instantiate(DestroyedEffect);
        gameObject.SetActive(false);
    }

    public void OnPlayerRespawnInThisCheckPoint(Checkpoint checkpoint, Player player)
    {
        _direction = new Vector(-1, 0);
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = _startPosition;
        gameObject.SetActive(true);
    }*/
}
