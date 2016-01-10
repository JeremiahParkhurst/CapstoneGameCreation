using UnityEngine;
using System.Collections;

/* https://www.youtube.com/watch?v=hkLAdo9ODDs&list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n&index=12 */

public class JumpPlatform : MonoBehaviour {

    public float JumpMagnitude = 20;

    public void ControllerEnter2d(CharacterController2D controller)
    {
        controller.SetVerticalForce(JumpMagnitude);
    }

}
