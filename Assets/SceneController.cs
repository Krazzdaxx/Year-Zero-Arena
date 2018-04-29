using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

	public GameObject trackedPlanePrefab;
	public Camera firstPersonCamera;
	//public ScoreboardController scoreboard;
	//public SnakeController snakeController;
	public Text debugtext;
	public GameObject boundingbox;
	public GameObject rotSlider;
	public GameObject scaleSlider;
	private bool isBoundingBoxPlaced = false;

	void Start()
	{
		QuitOnConnectionErrors ();
	}

	void Update()
	{
		// The session status must be Tracking in order to access the Frame.
		if (Session.Status != SessionStatus.Tracking)
		{
			const int lostTrackingSleepTimeout = 15;
			Screen.sleepTimeout = lostTrackingSleepTimeout;
			return;
		}
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		ProcessNewPlanes ();
		ProcessTouches ();
	
	}


	void QuitOnConnectionErrors()
	{
		//do not update if ARCore is not tracking
		if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
		{
			Debug.Log ("Error Permission Not Granted");
			Application.Quit ();
		} 
		else if (Session.Status.IsError ())
		{
			// this covers many different errors.
			// https://developers.google.com/ar/reference/unity/namespace/GoogleARCore
			Debug.Log("Error encountered");
			Application.Quit();
		}
	}

	void ProcessNewPlanes()
	{
		List<TrackedPlane> planes = new List<TrackedPlane> ();
		Session.GetTrackables (planes, TrackableQueryFilter.New);

		for (int i = 0; i < planes.Count; i++)
		{
			// Instantiate a plane visualization prefab and set it to track the new plane.
			// The transform is set to the origin with an identity rotation since the mesh
			// for our prefab is updated in Unity World coordinates.
			GameObject planeObject = Instantiate(trackedPlanePrefab, Vector3.zero, Quaternion.identity, transform);
			planeObject.GetComponent<TrackedPlaneController> ().SetTrackedPlane (planes [i]);
		}

	}

	void ProcessTouches()
	{
		Touch touch;
		if (Input.touchCount != 1 || (touch = Input.GetTouch (0)).phase != TouchPhase.Began)
		{
			return;
		}

		TrackableHit trackhit;

		TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds | TrackableHitFlags.PlaneWithinPolygon;

		if (Frame.Raycast (touch.position.x, touch.position.y, raycastFilter, out trackhit))
		{
			SetSelectedPlane (trackhit.Trackable as TrackedPlane);
		}

		Ray raycast = firstPersonCamera.ScreenPointToRay(touch.position);
		RaycastHit rayhit;

		if (Physics.Raycast (raycast, out rayhit))
		{
			//debugtext.text = rayhit.transform.gameObject.name;
		}
	}

	void SetSelectedPlane(TrackedPlane selectedPlane)
	{
		Debug.Log ("selected plane centered at " + selectedPlane.CenterPose.position);
		//scoreboard.SetSelectedPlane(selectedPlane);
		//snakeController.SetPlane (selectedPlane);
		//GetComponent<FoodController> ().SetSelectedPlane (selectedPlane);
		//Instantiate(boundingbox,selectedPlane.CenterPose.position,transform.rotation * Quaternion.Euler(0.0f,180.0f,0.0f));
		if(!isBoundingBoxPlaced)
		{
			boundingbox.SetActive(true);
			boundingbox.transform.position = selectedPlane.CenterPose.position;
			boundingbox.transform.rotation = selectedPlane.CenterPose.rotation;
			isBoundingBoxPlaced = true;
		}
	}

	public void RotateBoundingBox()
	{
		boundingbox.transform.rotation = Quaternion.AngleAxis (rotSlider.GetComponent<Slider> ().value, Vector3.down);
	}

	public void ScaleBoundingBox()
	{
		boundingbox.transform.localScale = new Vector3 (scaleSlider.GetComponent<Slider> ().value, scaleSlider.GetComponent<Slider> ().value, scaleSlider.GetComponent<Slider> ().value);
	}

}

