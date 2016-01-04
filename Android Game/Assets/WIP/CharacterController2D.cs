using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{

    private const float SkinWid = .02f;
    private const int TotalHorizontalRays = 8;
    private const int TotalVerticalRays = 4;

    private static readonly float SlopeLimitTangant = Mathf.Tan(75f * Mathf.Deg2Rad);

    public LayerMask PlatformMask;
    public ControllerParameters2D DefaultParameters;

    public ControllerState2D State { get; private set; }
    public Vector2 Velocity { get; private set; }
    public bool CanJump { get { return false; } }

    // handles initialization
    public void Awake()
    {
        State = new ControllerState2D();
    }

    // add force to character controller
    public void AddForce(Vector2 force)
    {
        Velocity += force;
    }

    // sets the force
    public void SetForce(Vector2 force)
    {
        Velocity = force;
    }

    // helper method to set the horizontal force
    public void SetHorizontalForce(float x)
    {
        Velocity.x = x;
    }

    // helper method to set the vertical force
    public void SetVerticalForce(float y)
    {

    }

    public void Jump()
    {

    }

    public void LateUpdate()
    {

    }

    private void Move(Vector2 deltaMovement)
    {

    }

    // handle how character movement on a platform reacts
    private void HandlePlatforms()
    {

    }

    // where the rays will be instantiated
    private void CalculateRayOrigins()
    {

    }

    // takes a reference to delta movement to allow it to manipulate it if there anything colliding horizontally
    private void MoveHorizontally(ref Vector2 deltaMovement)
    {

    }

    // takes a reference to delta movement to allow it to manipulate it if there anything colliding vertically
    private void MoveVertically(ref Vector2 deltaMovement)
    {

    }


    private void HandleVerticalSlope(ref Vector2 deltaMovement)
    {

    }

    private void HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {

    }

    public void OnTriggerExit2D(Collider2D other)
    {

    }
}
