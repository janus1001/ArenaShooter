using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomHudCanvas : MonoBehaviour
{
    public static RoomHudCanvas singleton;

    public Button forestButton;
    public Button desertButton;
    public Button iceButton;

    public RectTransform playerSlotsForest;
    public RectTransform playerSlotsDesert;
    public RectTransform playerSlotsIce;
    public RectTransform playerSlotsList;

    public TMPro.TMP_Text readyText;

    public List<RectTransform> players = new List<RectTransform>();

    private void Start()
    {
        if(!singleton)
            singleton = this;
    }

    public void JoinTeam(string team)
    {
        switch (team)
        {
            case "forest":
                NetworkRoomPlayerExtended.singleton.CmdSelectTeam(Team.Forest);
                break;
            case "desert":
                NetworkRoomPlayerExtended.singleton.CmdSelectTeam(Team.Desert);
                break;
            case "ice":
                NetworkRoomPlayerExtended.singleton.CmdSelectTeam(Team.Ice);
                break;
            case "no team":
                NetworkRoomPlayerExtended.singleton.CmdSelectTeam(Team.NoTeam);
                break;
            default:
                Debug.LogError("Check team text on button.");
                break;
        }
    }

    public void ToggleReady()
    {
        NetworkRoomPlayerExtended.singleton.ToggleReady(); 
        UpdateHUD(true);
    }

    // Sets positions of buttons and player panels, hides buttons when there are two players on a team already.
    public void UpdateHUD(bool isLocalPlayerChanged) 
    {
        if(isLocalPlayerChanged)
        {
            // Show button if player chose a team
            Debug.Log(readyText.transform.parent.gameObject);
            readyText.transform.parent.gameObject.SetActive(true);

            if(!NetworkRoomPlayerExtended.singleton.readyToBegin)
            {
                readyText.text = "Mark as ready";
            }
            else
            {
                readyText.text = "Unready";
            }
        }

        if(playerSlotsForest.childCount >= 3)
        {
            forestButton.gameObject.SetActive(false);
        }
        else
        {
            forestButton.gameObject.SetActive(true);
        }

        if (playerSlotsDesert.childCount >= 3)
        {
            desertButton.gameObject.SetActive(false);
        }
        else
        {
            desertButton.gameObject.SetActive(true);
        }

        if (playerSlotsIce.childCount >= 3)
        {
            iceButton.gameObject.SetActive(false);
        }
        else
        {
            iceButton.gameObject.SetActive(true);
        }

        forestButton.transform.SetAsLastSibling();
        desertButton.transform.SetAsLastSibling();
        iceButton.transform.SetAsLastSibling();
    }
}
