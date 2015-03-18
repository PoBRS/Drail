using UnityEngine;
using System.Collections.Generic;
using System;

public class BezierSpline : MonoBehaviour
{
	[SerializeField]
    public List<RailPoint> bezierPoints;
    /// <summary>
    /// Gives out the number of points in this BezierSpline.
    /// </summary>
    public int PointCount
    {
        get
        {
            return (this.bezierPoints.Count);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointToSet"></param>
    /// <param name="newPosition"></param>
    public void SetControlPoint(Point pointToSet, Vector3 newPosition)
    {
        RailPoint pointToEnforce = null;

        if (pointToSet is ControlPoint)
        {
           // pointToEnforce = (pointToSet as ControlPoint).LinkedRailPoint;
        }
        else
        {
            pointToEnforce = (pointToSet as RailPoint);
            Vector3 delta = newPosition - pointToSet.pointCoordinates;

			(pointToSet as RailPoint).controlPoints[0].pointCoordinates += delta;
			if ((pointToSet as RailPoint).controlPoints.Count > 2)
            {
				(pointToSet as RailPoint).controlPoints[1].pointCoordinates += delta;
            }
        }
	
        pointToSet.pointCoordinates = newPosition;

		if (pointToEnforce != null) {
			EnforceMode (pointToEnforce, pointToSet);
		}
    }
    /// <summary>
    /// Remove the oldest point from the array of points.
    /// It's the one in the first position.
    /// </summary>
    public void RemoveFirstPoint()
    {
        if (bezierPoints.Count > 0) {
			bezierPoints.Remove(bezierPoints[0]);
		}
    }
    /// <summary>
    /// Return the oldest point from the array of points.
    /// It's the one in the first position
    /// </summary>
    /// <returns>The first point or NULL if there is no points.</returns>
    public RailPoint GetFirstPoint()
    {
        if (bezierPoints.Count > 0)
        {
           return bezierPoints[0];
                 }
        return null;
    }
    /// <summary>
    /// Return the newest point from the array of points.
    /// It's the one in the lst position.
    /// </summary>
    /// <returns>The last point or NULL if there is no points.</returns>
    public RailPoint GetLastPoint()
    {
        if (bezierPoints != null)
        {
            if (bezierPoints.Count > 0)
            {
                return bezierPoints[bezierPoints.Count - 1];
            }
        }
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointToEnforce"></param>
    /// <param name="selectedPoint"></param>
    private void EnforceMode(RailPoint pointToEnforce, Point selectedPoint)
    {
        BezierControlPointMode mode;

        mode = pointToEnforce.BezierControlPointMode;

        if (mode == BezierControlPointMode.Free)
        {
            return;
        }

        ControlPoint firstControlPoint = null;
        ControlPoint secondaryControlPoint = null;
        if (pointToEnforce != GetLastPoint())
        {
			firstControlPoint = pointToEnforce.controlPoints[0];
        }
        if (pointToEnforce != GetFirstPoint())
        {
			secondaryControlPoint = pointToEnforce.controlPoints[1];
        }

        if (selectedPoint == firstControlPoint)
        {
            Vector3 enforcedTangent = pointToEnforce.pointCoordinates - firstControlPoint.pointCoordinates;
            if (mode == BezierControlPointMode.Aligned)
            {
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(pointToEnforce.pointCoordinates, secondaryControlPoint.pointCoordinates);
            }
            secondaryControlPoint.pointCoordinates = pointToEnforce.pointCoordinates + enforcedTangent;
        }
        else
        {
            Vector3 enforcedTangent = pointToEnforce.pointCoordinates - secondaryControlPoint.pointCoordinates;
            if (mode == BezierControlPointMode.Aligned)
            {
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(pointToEnforce.pointCoordinates, firstControlPoint.pointCoordinates);
            }
            firstControlPoint.pointCoordinates = pointToEnforce.pointCoordinates + enforcedTangent;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public Vector3 GetPoint(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = PointCount - 2;
        }
        else
        {
            t = Mathf.Clamp01(t) * (PointCount - 1);
            i = (int)t;
            t -= i;
        }

        if (bezierPoints[i].controlPoints.Count == 2)
        {
			return transform.TransformPoint(Bezier.GetPoint(bezierPoints[i].pointCoordinates,
			                                                bezierPoints[i].controlPoints[1].pointCoordinates,
			                                                bezierPoints[i + 1].controlPoints[0].pointCoordinates,
			                                                bezierPoints[i + 1].pointCoordinates,
			                                                t));
        }
        else
        {
			return transform.TransformPoint(Bezier.GetPoint(bezierPoints[i].pointCoordinates,
			                                                bezierPoints[i].controlPoints[0].pointCoordinates,
			                                                bezierPoints[i + 1].controlPoints[0].pointCoordinates,
			                                                bezierPoints[i + 1].pointCoordinates,
			                                                t));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public Point GetCurrentPoint(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = PointCount - 2;
        }
        else
        {
            t = Mathf.Clamp01(t) * (PointCount - 1);
            i = (int)t;
            t -= i;
        }
        return bezierPoints[i];
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public Vector3 GetVelocity(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = PointCount - 2;
        }
        else
        {
            t = Mathf.Clamp01(t) * (PointCount - 1);
            i = (int)t;
            t -= i;
        }
        if (bezierPoints[i].controlPoints.Count < 2)
        {
            return transform.TransformPoint(Bezier.GetFirstDerivative(
				bezierPoints[i].pointCoordinates, bezierPoints[i].controlPoints[0].pointCoordinates, bezierPoints[i + 1].controlPoints[0].pointCoordinates, bezierPoints[i + 1].pointCoordinates, t)) - transform.position;
        }
        else
        {
            return transform.TransformPoint(Bezier.GetFirstDerivative(
				bezierPoints[i].pointCoordinates, bezierPoints[i].controlPoints[1].pointCoordinates, bezierPoints[i + 1].controlPoints[0].pointCoordinates, bezierPoints[i + 1].pointCoordinates, t)) - transform.position;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }
    /// <summary>
    /// 
    /// </summary>
    public void Reset()
    {
		bezierPoints = new List<RailPoint>();
		this.MakeStartingLine();
    }



	private void MakeStartingLine()
	{
		AddPoint(new Vector3(0,0,0));
		AddPoint(new Vector3(0,0,3));
	}
    /// <summary>
    /// 
    /// </summary>
    public void AddPoint(Vector3 offset)
    {
        List<RailPoint> pointsToAdd = new List<RailPoint>();

        RailPoint newPoint = new RailPoint(offset);
		Vector3 controlPointOffset = Vector3.zero;
		RailPoint previousPoint = this.GetLastPoint();

		if (previousPoint != null) {
			newPoint.pointCoordinates += previousPoint.pointCoordinates;
			EnforceMode (newPoint, newPoint);

			if (previousPoint != this.GetFirstPoint ()) {
				ControlPoint controlPointToAddPrevious = new ControlPoint (new Vector3 (0f, 0f, 1f) + previousPoint.pointCoordinates);
				previousPoint.AddControlPoint (controlPointToAddPrevious);
			}
		} else {
			controlPointOffset = new Vector3(0,0,2);
		}
       
		ControlPoint controlPointToAddCurrent = new ControlPoint(new Vector3(0f, 0f, -1f)  + newPoint.pointCoordinates + controlPointOffset);	
		newPoint.AddControlPoint(controlPointToAddCurrent);


        pointsToAdd.Add(newPoint);

        foreach (RailPoint points in pointsToAdd)
        {
			bezierPoints.Add(points);
        }

        pointsToAdd.Clear();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="splineWalkerPosition"></param>
    /// <returns></returns>
    public Point CheckForCurrentPoint(Vector3 splineWalkerPosition)
    {
        foreach (Point point in bezierPoints)
        {

            if (splineWalkerPosition.x == point.pointCoordinates.x && splineWalkerPosition.z == point.pointCoordinates.z)
            {
                return point;
            }
        }
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointToSet"></param>
    /// <param name="newMode"></param>
    public void SetControlPointMode(RailPoint pointToSet, BezierControlPointMode newMode)
    {

        pointToSet.BezierControlPointMode = newMode;
        EnforceMode(pointToSet, pointToSet);
    }

	public void AppendSpline(BezierSpline splineToAppend)
	{
		foreach(RailPoint point in splineToAppend.bezierPoints)
		{
			if(point == splineToAppend.GetFirstPoint())
			{
				RailPoint pointToAppend = point.DeepCopy();
				Vector3 initialPointCoordinates = pointToAppend.pointCoordinates;

				Vector3 deltaControlPoint = pointToAppend.controlPoints[pointToAppend.controlPoints.Count - 1].pointCoordinates - initialPointCoordinates;
				pointToAppend.controlPoints[pointToAppend.controlPoints.Count - 1].pointCoordinates =this.GetLastPoint().pointCoordinates + deltaControlPoint;
				this.GetLastPoint().controlPoints.Add(pointToAppend.controlPoints[pointToAppend.controlPoints.Count - 1]);
			}
			else
			{
				RailPoint pointToAppend = point.DeepCopy();

				Vector3 initialPointCoordinates = pointToAppend.pointCoordinates;

				pointToAppend.pointCoordinates += this.GetLastPoint().pointCoordinates;

				foreach(ControlPoint controlPoint in pointToAppend.controlPoints)
				{
					Vector3 deltaControlPoint = controlPoint.pointCoordinates - initialPointCoordinates;
					controlPoint.pointCoordinates = pointToAppend.pointCoordinates + deltaControlPoint;
				}
		
				this.bezierPoints.Add(pointToAppend);
			}
		
		}
	}
}