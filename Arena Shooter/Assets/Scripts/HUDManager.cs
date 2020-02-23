using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager current;

    public RectTransform offsetHUD;

    public Image forestTeamHealth;
    public List<HUDPlayer> forestTeamPlayers;

    public Image desertTeamHealth;
    public List<HUDPlayer> desertTeamPlayers;

    public Image iceTeamHealth;
    public List<HUDPlayer> iceTeamPlayers;

    void Start()
    {
        if(current)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        float value = Time.time % 1;
    }
}

[Serializable]
public class HUDPlayer
{
    [SerializeField]
    public Image avatar;
    [SerializeField]
    public Image health;
    [SerializeField]
    public Image shield;
    [SerializeField]
    public Image crossout;
}