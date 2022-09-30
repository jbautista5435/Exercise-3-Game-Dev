using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayPlayerDetails : MonoBehaviour
{
    public GameObject playerDetails;
    public TextMeshProUGUI displayedDetails;
    
    void Start()
    {
        playerDetails = GameObject.FindWithTag("PlayerDetails");
        displayedDetails.text = "Player: " + playerDetails.GetComponent<PlayerDetails>().getPlayerName();
    }
}
