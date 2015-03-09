using UnityEngine;
using System.Collections;

public class Point 
{
	[SerializeField]
	public Vector3 pointCoordinates;
	
	[SerializeField]
	public Point NextPoint { get; set; }
	[SerializeField]
	public Point previousPoint;

	public bool isHole;
	[SerializeField]
	private ControlPoint controlPoint;
	[SerializeField]
	private ControlPoint secondaryControlPoint;

	public Point PreviousPoint
	{ 
		get
		{
			return this.previousPoint;
		}
		set
		{
			this.previousPoint = value;
			this.previousPoint.NextPoint = this;
		}
	}
	
	public ControlPoint ControlPoint
	{
		get
		{
			return this.controlPoint;
		}
		set
		{
			this.controlPoint = value;
		}
	}

	public ControlPoint SecondaryControlPoint
	{
		get
		{
			return this.secondaryControlPoint;
		}
		set
		{
			this.secondaryControlPoint = value;
		}
	}

	public Point(float x, float y, float z)
	{
		this.pointCoordinates = new Vector3 (x, y, z);
	}

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
}
