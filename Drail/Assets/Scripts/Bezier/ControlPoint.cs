using UnityEngine;
using System.Collections;

public class ControlPoint : Point
{
	[SerializeField][HideInInspector]
	public Point LinkedPoint = null;

	public ControlPoint(float x, float y, float z, Point linkedPoint) : base (x,y,z)
	{
		this.pointCoordinates = new Vector3 (x, y, z);
		this.LinkedPoint = linkedPoint;
	}
	
}
