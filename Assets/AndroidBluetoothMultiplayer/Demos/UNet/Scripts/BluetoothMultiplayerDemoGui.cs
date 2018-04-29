using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace LostPolygon.AndroidBluetoothMultiplayer.Examples.UNet {
    public class BluetoothMultiplayerDemoGui : BluetoothDemoGuiBase {


        public AndroidBluetoothNetworkManagerHelper AndroidBluetoothNetworkManagerHelper;
		public BluetoothMultiplayerDemoNetworkManager BluetoothDemoNetworkManager;


        public GameObject UIPanelGameObject;

		[SerializeField]
		private Button startButtonRef;
		[SerializeField]
		private Button playButtonRef;
		[SerializeField]
		private Button startServerButtonRef;
		[SerializeField]
		private Button connectServerButtonRef;
		[SerializeField]
		private Button disconnectButtonRef;
		[SerializeField]
		private Button backButtonRef;




		protected virtual void OnEnable() {
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
		}

		protected virtual void OnDisable() {
			UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
		}

		private void SceneManagerOnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode) {
			SceneLoadedHandler(scene.buildIndex);
		}

		protected virtual void SceneLoadedHandler(int buildIndex) {
			Screen.sleepTimeout = SleepTimeout.SystemSetting;
			CameraFade.StartAlphaFade(Color.black, true, 0.25f, 0.0f);
		}


        protected override void Update() 
		{
            base.Update();

			if (Input.GetKeyDown(KeyCode.Escape))
				QuitApplication();
            

            if (!AndroidBluetoothNetworkManagerHelper.IsInitialized)
                return;

            BluetoothMultiplayerMode currentMode = AndroidBluetoothMultiplayer.GetCurrentMode();
          
			disconnectButtonRef.gameObject.SetActive(currentMode != BluetoothMultiplayerMode.None);
			if (disconnectButtonRef.gameObject.activeInHierarchy) {
				disconnectButtonRef.gameObject.GetComponentInChildren<Text>().text = currentMode == BluetoothMultiplayerMode.Client ? "Disconnect" : "Stop server";
            }
				
        }

        private void Awake() {
            // Enabling verbose logging. See logcat!
            AndroidBluetoothMultiplayer.SetVerboseLog(true);
        }

        protected override void OnGoingBackToMenu() {
            // Gracefully closing all Bluetooth connectivity and loading the menu
            try {
				BluetoothDemoNetworkManager.StopHost();
                AndroidBluetoothMultiplayer.StopDiscovery();
                AndroidBluetoothMultiplayer.Stop();
            } catch {
                //
            }
        }


		public void StartButton()
		{
			startButtonRef.gameObject.SetActive (false);
			startServerButtonRef.gameObject.SetActive (true);
			connectServerButtonRef.gameObject.SetActive (true);
			backButtonRef.gameObject.SetActive (true);
		}

		public void PlayButton()
		{
			BluetoothDemoNetworkManager.ServerChangeScene ("JD Game");
			//CameraFade.StartAlphaFade(Color.black, false, 0.25f, 0f, () => BluetoothExamplesTools.LoadLevel("JD Game"));
		}

		public void LoadLevel(string levelName) {
			CameraFade.StartAlphaFade(Color.black, false, 0.25f, 0f, () => BluetoothExamplesTools.LoadLevel(levelName));
		}

        public void OnBackToMenuButton() {
            GoBackToMenu();
        }

        public void OnStartServerButton() {
			AndroidBluetoothNetworkManagerHelper.StartHost ();
			playButtonRef.gameObject.SetActive (true);
			playButtonRef.interactable = false;
			playButtonRef.GetComponentInChildren<Text> ().text = "Starting Server...";
			backButtonRef.gameObject.SetActive(false);
			startServerButtonRef.gameObject.SetActive (false);
			connectServerButtonRef.gameObject.SetActive (false);
        }

        public void OnConnectToServerButton() {
            AndroidBluetoothNetworkManagerHelper.StartClient();
			playButtonRef.gameObject.SetActive (true);
			playButtonRef.interactable = false; 											
			playButtonRef.GetComponentInChildren<Text> ().text = "Waiting On Host....";   	
			startServerButtonRef.gameObject.SetActive (false);
			connectServerButtonRef.gameObject.SetActive (false);
        }

        public void OnDisconnectButton() {
            AndroidBluetoothMultiplayer.StopDiscovery();
            AndroidBluetoothMultiplayer.Stop();
			BluetoothDemoNetworkManager.StopHost();
			startServerButtonRef.gameObject.SetActive (true);
			connectServerButtonRef.gameObject.SetActive (true);
        }

		public void QuitApplication() {
			CameraFade.StartAlphaFade(Color.black, false, 0.25f, 0f, Application.Quit);
		}


    }
}