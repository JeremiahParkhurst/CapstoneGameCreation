using UnityEngine;
using System.Collections;

public class TouchControls : MonoBehaviour {

    private Player thePlayer;
    private PauseMenu thePauseMenu;

	// Use this for initialization
	void Start () {
        thePlayer = FindObjectOfType<Player>();
        thePauseMenu = FindObjectOfType<PauseMenu>();
	}
	
    public void LeftArrow(int horiztontalInput)
    {
        thePlayer.hInput = horiztontalInput;
    }

    public void RightArrow(int horiztonalInput)
    {
        thePlayer.hInput = horiztonalInput;
    }

    public void UpArrow(int verticalInput)
    {
        thePlayer.TouchJump();
        thePlayer.vInput = verticalInput;
    }

    public void DownArrow(int verticalInput)
    {
        thePlayer.vInput = verticalInput;
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
        thePlayer.TouchJump();
    }

    public void PauseMenu()
    {
        thePauseMenu.TouchPause();
    }
}
