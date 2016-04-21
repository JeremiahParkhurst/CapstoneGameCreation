using UnityEngine;

/*
* Resource: https://www.youtube.com/watch?v=lHb213yRP-Y&index=33&list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n
*
*
*/
public class FinishLevel : MonoBehaviour {
    
    // Reference Objects  
    private EndLevelMenu theEndLevelMenu;
    private Player thePlayer;

    public bool playerInPortal;     // Checks to see if the Player collided with the Portal
    public string levelToLoad;      // The scene to be loaded

    // Update is called once per frame
    void Update()
    {
        if (playerInPortal)
        {           
            theEndLevelMenu.TouchEndLevelMenu();
        }
    }

    // Initialization
    void Start()
    {
        theEndLevelMenu = FindObjectOfType<EndLevelMenu>();
        thePlayer = FindObjectOfType<Player>();
    }

    // Loads the scene
    public void LoadLevel()
    {
        Application.LoadLevel(levelToLoad);
    }

    /*
    * @param other, the object that is colliding with this object
    * Checks to see if the player collided with this object
    */
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null)
            return;
        
        playerInPortal = true;
    }

    /*
    * @param other, the object that is colliding with this object
    * Checks to see if the player no longer collided with this object
    */
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null)
            return;

        playerInPortal = false;
    }
}