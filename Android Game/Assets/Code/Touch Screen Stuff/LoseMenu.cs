using UnityEngine;

public class LoseMenu : MonoBehaviour {

    public string currentLevel;         // current scene name
    public string levelSelect;          // name of the level select screen
    public string mainMenu;             // name of the main menu scene   
    public GameObject loseMenuCanvas;   // instance of the pausedMenuCanvas

    void Start()
    {
        currentLevel = Application.loadedLevelName;
        loseMenuCanvas.SetActive(false);  // hides the pause menu canvas
        Time.timeScale = 1f;                // reverts time
    }    
 
    public void Restart()
    {
        loseMenuCanvas.SetActive(false);  // hides the pause menu canvas
        Time.timeScale = 1f;                // reverts time
        Application.LoadLevel(currentLevel);
    }

    public void LevelSelect()
    {
        Application.LoadLevel(levelSelect);
    }

    public void Quit()
    {
        Application.LoadLevel(mainMenu);
    }

    public void HideLoseMenu()
    {
        loseMenuCanvas.SetActive(false);  // hides the pause menu canvas
        Time.timeScale = 1f;                // reverts time
    }

    public void ShowLoseMenu()
    {
        loseMenuCanvas.SetActive(true);   // shows the pause menu canvas
        Time.timeScale = 0f;                // freezes time        
    }
}
