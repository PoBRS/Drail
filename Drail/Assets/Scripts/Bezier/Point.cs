using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RailPoint : Point
{

	[SerializeField]
	public List<ControlPoint> controlPoints;

	public GameObject linkedDecorator;
	public bool isHole;


	private BezierControlPointMode bezierControlPointMode;
	
	public BezierControlPointMode BezierControlPointMode 
	{
		get 
		{ 
			return bezierControlPointMode; 
		}
		set 
		{
			bezierControlPointMode = value; 
		}
	}

	public RailPoint (float x, float y, float z) : base(x,y,z)
	{
		this.controlPoints = new List<ControlPoint>();
	}
	
	public RailPoint(Vector3 pointCoordinates) : base (pointCoordinates)
	{
		this.controlPoints = new List<ControlPoint>();
	}

	public void AddControlPoint(ControlPoint controlPointToAdd)
	{
		if (this.controlPoints.Count < 2) 
		{
			this.controlPoints.Add (controlPointToAdd);
		}
	}

	public RailPoint DeepCopy()
	{
		RailPoint other = (RailPoint) this.MemberwiseClone();

		List<ControlPoint> newControlPointList = new List<ControlPoint>();
		foreach (ControlPoint controlPoint in this.controlPoints) 
		{
			ControlPoint newControlPoint = controlPoint.DeepCopy();
			newControlPointList.Add(newControlPoint);
		}
		
		other.controlPoints = newControlPointList;

		return other;
	}
}
