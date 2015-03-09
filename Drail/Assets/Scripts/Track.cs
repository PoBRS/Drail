using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Track : MonoBehaviour {

	public BezierSpline linkedSpline = null;
	void Awake () 
	{
		GameElements.trackInstance = this;
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
		linkedSpline.Reset ();
		for(int i = 0; i < 8; i++)
		{
			linkedSpline.AddPoint();
		}
	}
	
	public void AutoGenerateNextRails()
	{
		linkedSpline.RemoveFirstPoint ();
		linkedSpline.AddPoint ();
		Debug.Log (linkedSpline.PointCount);
	}
}
