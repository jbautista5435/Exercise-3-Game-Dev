using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    public int levelNumber;
    public GameObject levelloader;

    private string winScreen = "WinScreen";

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (levelNumber == 0)
            {
                levelloader.GetComponent<LevelLoader>().LoadMainMenu();
            } else if (levelNumber == 4)
            {
                levelloader.GetComponent<LevelLoader>().LoadWinScreen();
            } else 
            {
                levelloader.GetComponent<LevelLoader>().LoadLevel(levelNumber);
            }
        }
    }
}
