using UnityEngine;
using System.Collections;

public class EndLevelMenu : MonoBehaviour {

    public string levelSelect;
    public string mainMenu;
    public bool reachedEndLevel;
    public GameObject endLevelMenuCanvas;
    public string currentLevelName;

    public void Restart()
    {
        LevelManager.Instance.GotoNextLevel(currentLevelName);
    }

    public void LevelSelect()
    {
        Application.LoadLevel(levelSelect);
    }

    public void Quit()
    {
        Application.LoadLevel(mainMenu);
    }

    public void ShowEndLevelMenu()
    {
        endLevelMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideEndLevelMenu()
    {
        endLevelMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
}
