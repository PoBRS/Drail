using UnityEngine;
using System.Collections;

[System.Serializable]
public class Point
{
	[SerializeField]
	public Vector3 pointCoordinates;

	public Point(float x, float y, float z)
	{
		this.pointCoordinates = new Vector3 (x, y, z);
	}

	public Point(Vector3 pointCoordinates)
	{
		this.pointCoordinates = pointCoordinates;
	}
}
