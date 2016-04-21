using UnityEngine;

/*
* Resource: Adapted from PauseMenu
*
* Class to display buttons and hide/show the EndLevelMenu canvas overlay.
*/
public class EndLevelMenu : MonoBehaviour {

    // For the buttons
    public string currentLevelName;
    public string levelSelect;
    public string mainMenu;

    // 
    public bool reachedEndLevel;
    public GameObject endLevelMenuCanvas;       

    // Update is called once per frame
    void Update()
    {        
        if (reachedEndLevel)
        {
            endLevelMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            // need to disable gamehud time 
        }
        else
        {
            endLevelMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
            // re-enable gamehud time
        }        
    }   

    // Restarts the player to the currentLevelName scene
    public void Restart()
    {
        Application.LoadLevel(currentLevelName);
    }

    // Loads the LevelSelect screen
    public void LevelSelect()
    {
        Application.LoadLevel(levelSelect);
    }

    // Loads the MainMenu scene
    public void Quit()
    {
        Application.LoadLevel(mainMenu);
    }

    // Sets reachedEndLevel to true
    public void TouchEndLevelMenu()
    {        
        reachedEndLevel = true;
    }
}