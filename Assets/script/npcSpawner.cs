using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class npcSpawner : NetworkBehaviour {
    public GameObject enemyPrefab;
	// Use this for initialization
	void Start () {
		
	}
    public override void OnStartServer()
    {
        base.OnStartServer();

        var enemy = (GameObject)Instantiate(enemyPrefab, this.transform.position, this.transform.rotation);

        enemy.GetComponent<PlayerInput>().enabled = false ;
        enemy.GetComponent<NetworkIdentity>().localPlayerAuthority = false;
        NetworkServer.Spawn(enemy);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
