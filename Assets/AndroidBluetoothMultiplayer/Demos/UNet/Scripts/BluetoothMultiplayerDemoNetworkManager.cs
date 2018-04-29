using UnityEngine;
using System.Collections.Generic;

using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace LostPolygon.AndroidBluetoothMultiplayer.Examples.UNet {
    public class BluetoothMultiplayerDemoNetworkManager : AndroidBluetoothNetworkManager {
        public Dictionary<int, int> currentPlayers;
        public GameObject[] prefabs = new GameObject[2];
        public GameObject lobbyPrefab;

        public bool charSelectMode = true;
        public NetworkConnection LocalPconnection;
        /// <summary>
        /// called to gointo main game
        /// </summary>
        public void onServerChangeScene(string level)
        {
            charSelectMode = false;
            ServerChangeScene(level);
        }
        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            if (currentPlayers == null)
            {
                currentPlayers = new Dictionary<int, int>();
            }
            //add current connected player to dict
            if (!currentPlayers.ContainsKey(conn.connectionId))
                currentPlayers.Add(conn.connectionId, 1);

            //if in main level set correct player prefab
            if (charSelectMode == false)
            {
                playerPrefab = prefabs[currentPlayers[conn.connectionId]];
            }
            else
            {
                playerPrefab = lobbyPrefab;
            }
            var player = (GameObject)GameObject.Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
        // called when connected to a server
        public override void OnClientConnect(NetworkConnection conn)
        {
            LocalPconnection = conn;
            base.OnClientConnect(conn);

        }

        public void setPlayerCharChoice(int Pid, int choice)
        {
            Debug.Log("choice Selected " + choice.ToString());

            currentPlayers[Pid] = choice;
        }



        // public GameObject TapMarkerPrefab; // Reference to the tap effect
        // public bool StressTestMode;

        //private const int kStressModeActors = 30;s

        public override void OnStartServer() {
            base.OnStartServer();

            // Register the handler for the CreateTapMarkerMessage that is sent from client to server
           // NetworkServer.RegisterHandler(CreateTapMarkerMessage.kMessageType, OnServerCreateTapMarkerHandler);
        }

        public override void OnStartClient(NetworkClient client) {
            base.OnStartClient(client);

            // Register the handler for the CreateTapMarkerMessage that is sent from server to clients
           // client.RegisterHandler(CreateTapMarkerMessage.kMessageType, OnClientCreateTapMarkerHandler);
        }

        public override void OnServerReady(NetworkConnection conn) {
            base.OnServerReady(conn);

            /*
				GameObject actorGameObject = (GameObject) Instantiate(playerPrefab, Vector3.zero, Quaternion.identity); //This was julian trying to spawn the player use above for standard
               
                // Set player authority
                NetworkServer.SpawnWithClientAuthority(actorGameObject, conn);

                //Create a new virtual player for this actor
               	PlayerController playerController = new PlayerController();
                playerController.gameObject = actorGameObject;
                playerController.unetView = actorGameObject.GetComponent<NetworkIdentity>();

				conn.playerControllers.Add(playerController);
           */
        }

		public void OnStartGame(NetworkConnection conn)
		{
			
		}
    }
}