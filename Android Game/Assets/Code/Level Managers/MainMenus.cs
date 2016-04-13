using UnityEngine;
using System.Collections;

public class MainMenus : MonoBehaviour {

    public string theLevel;

	public void Play()
    {
        Application.LoadLevel(theLevel);
    }  
}
