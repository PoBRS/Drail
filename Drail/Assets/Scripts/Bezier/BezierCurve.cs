using UnityEngine;
using System.Collections.Generic;

public class BezierCurve 
{
	public GameObject curveControlPoint;
	public List<Point> points;
	public BezierCurve previousCurve;
	public BezierCurve nextCurve;

	public void Reset () 
	{
		this.points = new List<Point> ();
		points.Add (new Point (1f, 0f, 0f));
		points.Add (new Point (2f, 0f, 0f));
		points.Add (new Point (3f, 0f, 0f));
		points.Add (new Point (4f, 0f, 0f));
	}

	public void SetPreviousCurve(BezierCurve previousCurve)
	{
		this.previousCurve = previousCurve;
	}

	public void SetStartingPoint(Vector3 startingPoint)
	{
		this.points [0] = previousCurve.points[3];
		this.points [1].pointCoordinates = startingPoint;
		this.points [2].pointCoordinates = startingPoint;
		this.points [3].pointCoordinates = startingPoint;

		this.points [1].PreviousPoint = this.points [0];
		this.points [2].PreviousPoint = this.points [1];
		this.points [3].PreviousPoint = this.points [2];
	}

	public Point GetStartingPoint()
	{
		return this.points [0];
	}

	public Point GetEndPoint()
	{
		return this.points [3];
	}

	public void SetStartingPoint(Point pointToSet)
	{
		this.points [0] = pointToSet;
	}

	public void SetEndPoint(Point pointToSet)
	{
		this.points [3] = pointToSet;
	}
	
}