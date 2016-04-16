using UnityEngine;
using System.Collections;

public class TouchControls : MonoBehaviour {

    private Player thePlayer;

	// Use this for initialization
	void Start () {
        thePlayer = FindObjectOfType<Player>();
	}
	
    public void LeftArrow()
    {
        thePlayer.Move(-1);
    }

    public void RightArrow()
    {
        thePlayer.Move(1);
    }

    public void UpArrow()
    {
        thePlayer.TouchJump();
        thePlayer.MoveVertical(1);
    }

    public void DownArrow()
    {
        thePlayer.MoveVertical(-1);
    }

    public void UnpressedArrow()
    {
        thePlayer.Move(0);
        thePlayer.MoveVertical(0);
    }

    public void ShootButton()
    {
        thePlayer.TouchShoot();
    }

    public void JumpButton()
    {

    }
}
