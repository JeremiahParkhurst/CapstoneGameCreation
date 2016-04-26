using UnityEngine;

public class ReflectProjectiles : MonoBehaviour {

    // Character Essentials    
    private CharacterController2D _controller;  // has an instance of the CharacterController2D
    private Vector2 _direction;                 // the x-direction of this GameObject
    private Vector2 _startPosition;             // the initial spawn position of this GameObject
    public Projectile Projectile;       // this GameObject's projectile
    public Transform ProjectileFireLocation;    // the location of which the projectile is fired at

    // Use this for initialization
    void Start () {
        _controller = GetComponent<CharacterController2D>();    // instance of Charactercontroller2D
        _direction = new Vector2(-1, 0);                        // this GameObject will move the left upon initialization
        _startPosition = transform.position;                    // starting position of this GameObject
    }	

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Projectile>() == null)
            return;


        var projectile = other.GetComponent<SimpleProjectile>();
        if (projectile != null && projectile.Owner.GetComponent<Player>() != null)
        {
            DestroyObject(other);            
            // Instantiates the projectile, and initilializes the speed, and direction of the projectile
            projectile = (SimpleProjectile)Instantiate(Projectile, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
            projectile.Initialize(gameObject, _direction, _controller.Velocity);
        }
    }
}
