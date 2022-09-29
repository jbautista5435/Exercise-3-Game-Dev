using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetails : MonoBehaviour
{
    public string playername;

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public string getPlayerName()
    {
        return playername;
    }

    public void setPlayerName(string username) 
    {
        playername = username;
    }
}
