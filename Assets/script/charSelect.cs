using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class charSelect : MonoBehaviour {
    [HideInInspector]
    public lobbyPlayer LocalPlayer;

    public bool isHost = false;
    public GameObject HostUi;
    public Image[] portraits = new Image[2];
    public Sprite[] Images = new Sprite[2];

    public void SetLocalPlayerRef(lobbyPlayer p)
    {
        LocalPlayer = p;
        if (p.isClient && p.isServer)
        {
            isHost = true;
            HostUi.SetActive(true);
        }
        else
        {
            HostUi.SetActive(false);
        }
    }
    /// <summary>
    /// called from UI button
    /// </summary>
    /// <param name="choice"></param>
    public void ChooseChar(int choice)
    {
        LocalPlayer.sendChoiceToServer(choice);
        portraits[LocalPlayer.pid].sprite = Images[choice];
    }
    public void startGame(string level)
    {
        LocalPlayer.CmdCallServerChangeScene(level);
    }
    /// <summary>
    /// called from server with RCP
    /// </summary>
    /// <param name="pid"></param>
    /// <param name="choice"></param>
    public void setPics(int pid, int choice)
    {
        portraits[pid].sprite = Images[choice];

    }
   
    // Update is called once per frame
    void Update () {
		
	}
}
