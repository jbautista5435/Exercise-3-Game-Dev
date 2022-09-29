using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartLoader : MonoBehaviour
{    
    public TMP_InputField nameField;
    public GameObject playerDetails;
    public void ResetLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnSubmit() 
    {
        if (!string.IsNullOrEmpty(nameField.text))
            {
                playerDetails.GetComponent<PlayerDetails>().setPlayerName(nameField.text);
            }
    }
}
