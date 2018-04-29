using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class TrackedPlaneController : MonoBehaviour {

	private TrackedPlane trackedPlane;
	private PlaneRenderer planeRenderer;
	private List<Vector3> polygonVertices = new List<Vector3> ();
	private Vector3 hero1spawn;
	private Vector3 hero2spawn;
	public GameObject hero1;
	public GameObject hero2;
	private bool spawnCharacters;

	void Awake()
	{
		planeRenderer = GetComponent<PlaneRenderer> ();
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		//if no plane yet, disable the renderer and return.
		if (trackedPlane == null)
		{
			planeRenderer.EnablePlane (false);
			return;
		}

		//if this plane was subsumed bu another plan, destroy this object, plane's display will render it.
		if (trackedPlane.SubsumedBy != null)
		{
			Destroy (gameObject);
			return;
		}

		//if this plane is not valid or ARCore is not tracking, disable renderering
		if (trackedPlane.TrackingState != TrackingState.Tracking || Session.Status != SessionStatus.Tracking)
		{
			planeRenderer.EnablePlane (false);
			return;
		}

		//OK. Valid plane, so enable rendering and update the polygon data if needed
		planeRenderer.EnablePlane (true);
		List<Vector3> newPolygonVertices = new List<Vector3> ();
		trackedPlane.GetBoundaryPolygon (newPolygonVertices);
		if (!AreVerticesListsEqual (polygonVertices, newPolygonVertices))
		{
			polygonVertices.Clear ();
			polygonVertices.AddRange (newPolygonVertices);
			planeRenderer.UpdateMeshWithCurrentTrackedPlane (trackedPlane.CenterPose.position, polygonVertices);
		}

		//SpawnCharacters ();
	}

	bool AreVerticesListsEqual(List<Vector3> firstList, List<Vector3> secondList)
	{
		if (firstList.Count != secondList.Count)
		{
			return false;
		}

		for (int i = 0; i < firstList.Count; i++)
		{
			if(firstList[i] != secondList[i])
			{
				return false;
			}
		}
		return true;
	}

	public void SetTrackedPlane(TrackedPlane plane)
	{
		trackedPlane = plane;
		trackedPlane.GetBoundaryPolygon (polygonVertices);
		planeRenderer.Initialize ();
		planeRenderer.UpdateMeshWithCurrentTrackedPlane (trackedPlane.CenterPose.position, polygonVertices);
	}

	void SpawnCharacters()
	{
		if ((polygonVertices.Count >= 30)&&(!spawnCharacters))
		{
			hero1spawn = new Vector3 (trackedPlane.CenterPose.position.x + trackedPlane.ExtentX - 1.0f, trackedPlane.CenterPose.position.y, trackedPlane.CenterPose.position.z);
			hero2spawn = new Vector3 (trackedPlane.CenterPose.position.x - trackedPlane.ExtentX + 1.0f, trackedPlane.CenterPose.position.y, trackedPlane.CenterPose.position.z);
			Instantiate (hero1, hero1spawn, transform.rotation * Quaternion.Euler(0.0f, 180.0f,0.0f));
			Instantiate (hero2, hero2spawn, transform.rotation * Quaternion.Euler(0.0f, 180.0f,0.0f));
			spawnCharacters = true;
		}
	}
}