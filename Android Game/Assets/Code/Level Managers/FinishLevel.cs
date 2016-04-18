using UnityEngine;

/*
* Resource: https://www.youtube.com/watch?v=lHb213yRP-Y&index=33&list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n
*
*
*/
public class FinishLevel : MonoBehaviour {

    //public string LevelName; // the next level/scene or null if it is the last level/go back to start screen etc.
    private EndLevelMenu theEndLevelMenu;

    public void Update()
    {
        theEndLevelMenu = FindObjectOfType<EndLevelMenu>();        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null)
        {
            theEndLevelMenu.HideEndLevelMenu();
            return;
        }
            
        theEndLevelMenu.ShowEndLevelMenu();        
        //LevelManager.Instance.GotoNextLevel(LevelName);
    }
}
