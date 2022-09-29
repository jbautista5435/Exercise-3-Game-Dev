using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelLoader : MonoBehaviour
{    
    public void ResetLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel(int level)
    {
        print(level);
        if (level > 3) {
            SceneManager.LoadScene("WinScreen");
        }
        SceneManager.LoadScene("Level" + level.ToString());
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadWinScreen() {
        SceneManager.LoadScene("WinScreen");
    }
}
