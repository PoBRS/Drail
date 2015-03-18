using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Track : MonoBehaviour {

	public BezierSpline mainSpline = null;
	public List<BezierSpline> splineList;
	public GameObject railPrefab;

	void Awake () 
	{
		GameElements.trackInstance = this;
		this.splineList = new List<BezierSpline>();
		this.InitialRailGeneration ();
	}
	
	void Update()
	{
		//this.AutoGenerateNextRails ();
	}
	
	void AddTrackSpline (GameObject splineToAdd) 
	{
		//splinesTrackList.Add(splineToAdd);	
		//splineTrack.AppendSpline(splineToAdd.GetComponent<BezierSpline>());
	}
	
	public void AddStraightTrackSpline () 
	{
		//if(splinesTrackList.Count >= 1)
    	//{
		//	splineToAdd.transform.position = new Vector3(this.splineTrack.GetLastPoint().x + 1, 0 ,0);
   		//}
		//AddTrackSpline(splineToAdd);
	}
	
	public void RemoveTrackSpline(GameObject splineToRemove)
	{
		//splinesTrackList.Remove (splineToRemove);
		//Destroy (splineToRemove.gameObject);
	}
	
	private void InitialRailGeneration()
	{
		mainSpline.Reset ();

		for (int i = 0; i < 3; i++) 
		{
			GameObject rail = Instantiate (railPrefab);
			splineList.Add (rail.GetComponent<BezierSpline>());
		}

		foreach(BezierSpline spline in splineList)
		{
			spline.Reset();
		}

		for(int i = 0; i < 10; i++)
		{
			AddNextPoint();
		}
	}

	private void AddNextPoint()
	{
		mainSpline.AppendSpline(GameElements.prefabContainer.StraightRailPrefab.GetComponent<BezierSpline>());

		foreach(BezierSpline spline in splineList)
		{
			spline.AppendSpline(GameElements.prefabContainer.StraightRailPrefab.GetComponent<BezierSpline>());


			//spline.AddPoint();
			//RailPoint point = spline.GetLastPoint ();
			//GameObject newRail = GameObject.Instantiate (GameElements.prefabContainer.StraightRailPrefab);
			//newRail.GetComponent<SplineDecorator> ().AddRailSection (point);
			//point.linkedDecorator = newRail;
		}
		Vector3 spaceBetweenMainSpline = new Vector3(2,0,0);
		splineList[0].GetLastPoint().pointCoordinates = mainSpline.GetLastPoint().pointCoordinates - spaceBetweenMainSpline;
		splineList [1].GetLastPoint ().pointCoordinates = mainSpline.GetLastPoint ().pointCoordinates;
		splineList[2].GetLastPoint().pointCoordinates = mainSpline.GetLastPoint().pointCoordinates + spaceBetweenMainSpline;
	

	}
	
	public void AutoGenerateNextRails()
	{
				mainSpline.RemoveFirstPoint ();
		mainSpline.GetFirstPoint().controlPoints.Remove(mainSpline.GetFirstPoint().controlPoints[0]);
				foreach (BezierSpline spline in splineList) 
				{
					GameObject.Destroy (spline.GetFirstPoint ().linkedDecorator);
					spline.RemoveFirstPoint ();

				}

			AddNextPoint ();

		//Debug.Log (mainSpline.PointCount);
	}
}
