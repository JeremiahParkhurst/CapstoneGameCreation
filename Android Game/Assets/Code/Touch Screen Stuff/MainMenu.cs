using UnityEngine;

public class MainMenu : MonoBehaviour {

    public string startLevel;   // name of the main menu screen
    public string levelSelect;  // name of the level select screen
    public AudioClip niggaWithARocketLauncherSound;

    // Loads the level 1 scene [New Game]
	public void NewGame()
    {
        Application.LoadLevel(startLevel);
    }  

    // Loads the level select screen
    public void LevelSelect()
    {
        AudioSource.PlayClipAtPoint(niggaWithARocketLauncherSound, transform.position);
        Application.LoadLevel(levelSelect);
    }
}
