using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace LostPolygon.AndroidBluetoothMultiplayer.Examples.UNet 
{/// <summary>
/// no longer called in multiplayer scene only used in Test scene for testing purpose
/// 
/// </summary>
	public class UnitSpawner : MonoBehaviour
	{


    	[Tooltip("set to true to make the character player controlled")]
	    public bool isPlayerChar = false;
		public GameObject characterPrefab;
		public GameObject groundPlaneStage;


		//private NetworkConnection conn;
	
	//	private BluetoothMultiplayerDemoNetworkManager netMan;

		private bool spawnOnce = false;


		// Use this for initialization
		void Start () 
		{
            SpawnPlayers();
        }
		
		// Update is called once per frame
		void Update () 
		{
			
			
			
		}

		public void SpawnPlayers()
		{
			GameObject c =   Instantiate(characterPrefab, transform.position, transform.rotation) as GameObject;
			if (groundPlaneStage)
			{
				c.transform.parent = groundPlaneStage.transform;
			}
			if (isPlayerChar)
			{
			//	c.AddComponent<PlayerInput>();
				GameController._instance.Player = c.GetComponent<Character>();
             c.GetComponent<PlayerInput>().test = true ;
                
            }
            c.GetComponent<Character>().test = true;
          
        }
	}
}
