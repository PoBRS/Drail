using UnityEngine;
using System.Collections;

[System.Serializable]
public class ControlPoint : Point
{

	public ControlPoint(float x, float y, float z) : base (x,y,z)
	{
	}

	public ControlPoint(Vector3 pointCoordinates) : base (pointCoordinates)
	{
	}
	
	public ControlPoint DeepCopy()
	{
		ControlPoint other = (ControlPoint) this.MemberwiseClone();
		return other;
	}

	
}
