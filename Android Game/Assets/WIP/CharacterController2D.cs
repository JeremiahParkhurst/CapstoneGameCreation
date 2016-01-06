using UnityEngine;
using System.Collections;
/*
* https://www.youtube.com/watch?v=8_rIw0-AI8w&index=9&list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n
*/
public class CharacterController2D : MonoBehaviour
{

    private const float SkinWidth = .02f;
    private const int TotalHorizontalRays = 8;
    private const int TotalVerticalRays = 4;

    private static readonly float SlopeLimitTangant = Mathf.Tan(75f * Mathf.Deg2Rad);

    public LayerMask PlatformMask;
    public ControllerParameters2D DefaultParameters;

    // properties
    public ControllerState2D State { get; private set; }
    public Vector2 Velocity { get { return _velocity; } }
    public bool CanJump { get { return false; } }
    public bool HandleCollisions { get; set; }
    public ControllerParameters2D Parameters { get { return _overrideParameters ?? DefaultParameters;  } }

    // allias
    private Vector2 _velocity;
    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _boxCollider;
    private ControllerParameters2D _overrideParameters;
    private Vector3
        _raycastTopLeft,
        _raycastBottomRight,
        _raycastBottomLeft;

    private float
        _verticalDistanceBetweenRays,
        _horizontalDistanceBetweenRays;

    /* 
    * handles initialization of the ControllerState2D and alliases
    * calculates the collider width and height
    */
    public void Awake()
    {
        HandleCollisions = true;
        State = new ControllerState2D();
        _transform = transform;
        _localScale = transform.localScale;
        _boxCollider = GetComponent<BoxCollider2D>();

        var colliderWidth = _boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * SkinWidth);
        _horizontalDistanceBetweenRays = colliderWidth / (TotalVerticalRays - 1);

        var colliderHeight = _boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth);
        _verticalDistanceBetweenRays = colliderHeight / (TotalHorizontalRays - 1);
    }

    // add force to character controller
    public void AddForce(Vector2 force)
    {
        _velocity = force;
    }

    // sets the force
    public void SetForce(Vector2 force)
    {
        _velocity += force;
    }

    // helper method to set the horizontal force
    public void SetHorizontalForce(float x)
    {
        _velocity.x = x;
    }

    // helper method to set the vertical force
    public void SetVerticalForce(float y)
    {
        _velocity.y = y;
    }

    public void Jump()
    {

    }

    // moves the character scaled by their velocity and time
    public void LateUpdate()
    {
        Move(Velocity * Time.deltaTime);
    }

    private void Move(Vector2 deltaMovement)
    {
        var wasGrounded = State.IsCollidingBelow;
        State.Reset(); // resets fields to false

        if (HandleCollisions)
        {
            HandlePlatforms();
            CalculateRayOrigins(); // calculates where the rays are going to oringinate
            if (deltaMovement.y < 0 && wasGrounded) // if character movement is going down
                HandleVerticalSlope(ref deltaMovement);

            if (Mathf.Abs(deltaMovement.x) > .001f)
                MoveHorizontally(ref deltaMovement);

            MoveVertically(ref deltaMovement); // character default affected by gravity
        }

        _transform.Translate(deltaMovement, Space.World); // represents deltaMovement

        // TODO: Handle moving platforms code needed here

        if (Time.deltaTime > 0)
            _velocity = deltaMovement / Time.deltaTime;

        _velocity.x = Mathf.Min(_velocity.x, Parameters.MaxVelocity.x);
        _velocity.y = Mathf.Min(_velocity.y, Parameters.MaxVelocity.y);

        if (State.IsMovingUpSlope)
            _velocity.y = 0;
    }

    // handle how character movement on a platform reacts
    private void HandlePlatforms()
    {

    }

    // where the rays will be instantiated
    private void CalculateRayOrigins()
    {
        var size = new Vector2(_boxCollider.size.x * Mathf.Abs(_localScale.x), _boxCollider.size.y * Mathf.Abs(_localScale.y)) / 2;
        /* original code */
        //var center = new Vector2(_boxCollider.center.x * _localScale.x, _boxCollider.center.y * _localScale.y);
        var center = new Vector2(_boxCollider.offset.x * _localScale.x, _boxCollider.offset.y * _localScale.y);

        _raycastTopLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y + size.y - SkinWidth);
        _raycastBottomRight = _transform.position + new Vector3(center.x + size.x - SkinWidth, center.y - size.y + SkinWidth);
        _raycastBottomLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y - size.y + SkinWidth);

    }

    // takes a reference to delta movement to allow it to manipulate it if there anything colliding horizontally
    private void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : -Vector2.right; // left or right
        var rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;

        for(var i = 0; i < TotalHorizontalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * _verticalDistanceBetweenRays));
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var rayCastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!rayCastHit)
                continue;

            if (i == 0 && HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(rayCastHit.normal, Vector2.up), isGoingRight))
                break;

            deltaMovement.x = rayCastHit.point.x - rayVector.x;
            rayDistance = Mathf.Abs(deltaMovement.x);

            if (isGoingRight)
            {
                deltaMovement.x -= SkinWidth;
                State.IsCollidingRight = true;
            }
            else
            {
                deltaMovement.x += SkinWidth;
                State.IsCollidingLeft = true;
            }

            if (rayDistance < SkinWidth + .0001f)
                break;
        }
    }

    // takes a reference to delta movement to allow it to manipulate it if there anything colliding vertically
    private void MoveVertically(ref Vector2 deltaMovement)
    {

    }


    private void HandleVerticalSlope(ref Vector2 deltaMovement)
    {

    }

    private bool HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
    {
        return false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

    }

    public void OnTriggerExit2D(Collider2D other)
    {

    }
}
