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
    }

    public void DownArrow()
    {

    }

    public void UnpressedArrow()
    {

    }

    public void ShootButton()
    {

    }

    public void JumpButton()
    {

    }
}
