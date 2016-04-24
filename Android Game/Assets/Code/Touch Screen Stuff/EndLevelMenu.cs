using UnityEngine;

/*
* Resource: Adapted from PauseMenu.
*
* Handles functionality of the EndLevelMenu canvas.
*/
public class EndLevelMenu : MonoBehaviour {

    // For the buttons
    //public string currentLevelName;
    public string currentLevel;         // current scene name
    public string levelSelect;
    public string mainMenu;
     
    public GameObject endLevelCanvas; // instance of the pausedMenuCanvas

    void Start()
    {
        currentLevel = Application.loadedLevelName;
        endLevelCanvas.SetActive(false);  // hides the pause menu canvas
        Time.timeScale = 1f;                // reverts time
    }

    // Restarts the player to the currentLevelName scene
    public void Restart()
    {
        Application.LoadLevel(currentLevel);
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

    public void ShowEndLevelMenu()
    {
        endLevelCanvas.SetActive(true);  // hides the pause menu canvas
        Time.timeScale = 0f;             // reverts time
    }
}