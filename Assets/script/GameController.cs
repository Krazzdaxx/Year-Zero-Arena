using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    //will be setby the local player in mp games and by unitspawner in test scenes 
    [HideInInspector]
    public Character Player;

    public static GameController _instance;
    public void  SetPlayerLock(Character target)
    {
        if (Player != null)
        {
            if (target != Player)
            {

                Player.LockTarget = target.transform;
                Player.isLockedOn = true;
            }
        }
        else
        {
            Debug.LogWarning("reference to player not linked to game controller");

        }
    }
	// Use this for initialization
	void Awake() {
        _instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
