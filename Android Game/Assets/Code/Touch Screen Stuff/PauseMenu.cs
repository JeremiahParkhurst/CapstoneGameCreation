using UnityEngine;

/*
* Resource: https://www.youtube.com/watch?v=Wrelb5WBnoQ&index=18&list=PLiyfvmtjWC_Up8XNvM3OSqgbJoMQgHkVz
*
* Class to display buttons and hide/show the PauseMenu canvas overlay.
*/
public class PauseMenu : MonoBehaviour {

    public string levelSelect;          // name of the level select screen
    public string mainMenu;             // name of the main menu scene
    //public bool isPaused;               // checks to see if the game is currently paused
    public GameObject pausedMenuCanvas;	// instance of the pausedMenuCanvas
	
    void Start()
    {
         pausedMenuCanvas.SetActive(false);  // hides the pause menu canvas
            Time.timeScale = 1f;                // reverts time
    }

	// Update is called once per frame
	void Update () {/*
        //Debug.Log(Time.timeScale);
        if (isPaused)
        {
            pausedMenuCanvas.SetActive(true);   // shows the pause menu canvas
            Time.timeScale = 0f;                // freezes time
            // need to stop gamehud time
        }

        else
        {
            pausedMenuCanvas.SetActive(false);  // hides the pause menu canvas
            Time.timeScale = 1f;                // reverts time
            // need to revert gamehud time
        }*/
	}
   
    public void Resume()
    {
        pausedMenuCanvas.SetActive(false);  // hides the pause menu canvas
        Time.timeScale = 1f;                // reverts time
    }

    public void LevelSelect()
    {
        Application.LoadLevel(levelSelect);
    }

    public void Quit()
    {
        Application.LoadLevel(mainMenu);
    }

    public void TouchPause()
    {
        pausedMenuCanvas.SetActive(true);   // shows the pause menu canvas
        Time.timeScale = 0f;                // freezes time        
    }
}
