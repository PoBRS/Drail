using UnityEngine;
using System.Collections.Generic;
using System;

public class BezierSpline : MonoBehaviour
{
    public Point[] bezierPoints;
    /// <summary>
    /// Gives out the number of points in this BezierSpline.
    /// </summary>
    public int PointCount
    {
        get
        {
            return (this.bezierPoints.Length);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointToSet"></param>
    /// <param name="newPosition"></param>
    public void SetControlPoint(Point pointToSet, Vector3 newPosition)
    {
        Point pointToEnforce = null;

        if (pointToSet is ControlPoint)
        {
            pointToEnforce = (pointToSet as ControlPoint).LinkedPoint;
        }
        else
        {
            pointToEnforce = pointToSet;
            Vector3 delta = newPosition - pointToSet.pointCoordinates;

            pointToSet.ControlPoint.pointCoordinates += delta;
            if (pointToSet.SecondaryControlPoint != null)
            {
                pointToSet.SecondaryControlPoint.pointCoordinates += delta;
            }
        }
        pointToSet.pointCoordinates = newPosition;
        EnforceMode(pointToEnforce, pointToSet);
    }
    /// <summary>
    /// Remove the oldest point from the array of points.
    /// It's the one in the first position.
    /// </summary>
    public void RemoveFirstPoint()
    {
        Point[] newPointsArray = new Point[bezierPoints.Length - 1];
        for (int i = 1; i < bezierPoints.Length; i++)
        {
            newPointsArray[i - 1] = bezierPoints[i];
        }
        this.bezierPoints = newPointsArray;
    }
    /// <summary>
    /// Return the oldest point from the array of points.
    /// It's the one in the first position
    /// </summary>
    /// <returns>The first point or NULL if there is no points.</returns>
    public Point GetFirstPoint()
    {
        if (bezierPoints != null)
        {
            if (bezierPoints.Length > 0)
            {
                return bezierPoints[0];
            }
        }
        return null;
    }
    /// <summary>
    /// Return the newest point from the array of points.
    /// It's the one in the lst position.
    /// </summary>
    /// <returns>The last point or NULL if there is no points.</returns>
    public Point GetLastPoint()
    {
        if (bezierPoints != null)
        {
            if (bezierPoints.Length > 0)
            {
                return bezierPoints[bezierPoints.Length - 1];
            }
        }
        return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointToEnforce"></param>
    /// <param name="selectedPoint"></param>
    private void EnforceMode(Point pointToEnforce, Point selectedPoint)
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
            firstControlPoint = pointToEnforce.ControlPoint;
        }
        if (pointToEnforce != GetFirstPoint())
        {
            secondaryControlPoint = pointToEnforce.SecondaryControlPoint;
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

        if (bezierPoints[i].SecondaryControlPoint == null)
        {
            return transform.TransformPoint(Bezier.GetPoint(bezierPoints[i].pointCoordinates,
                                                            bezierPoints[i].ControlPoint.pointCoordinates,
                                                            bezierPoints[i + 1].ControlPoint.pointCoordinates,
                                                            bezierPoints[i + 1].pointCoordinates,
                                                            t));
        }
        else
        {
            return transform.TransformPoint(Bezier.GetPoint(bezierPoints[i].pointCoordinates,
                                                            bezierPoints[i].SecondaryControlPoint.pointCoordinates,
                                                            bezierPoints[i + 1].ControlPoint.pointCoordinates,
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
        if (bezierPoints[i].SecondaryControlPoint == null)
        {
            return transform.TransformPoint(Bezier.GetFirstDerivative(
            bezierPoints[i].pointCoordinates, bezierPoints[i].ControlPoint.pointCoordinates, bezierPoints[i + 1].ControlPoint.pointCoordinates, bezierPoints[i + 1].pointCoordinates, t)) - transform.position;
        }
        else
        {
            return transform.TransformPoint(Bezier.GetFirstDerivative(
            bezierPoints[i].pointCoordinates, bezierPoints[i].SecondaryControlPoint.pointCoordinates, bezierPoints[i + 1].ControlPoint.pointCoordinates, bezierPoints[i + 1].pointCoordinates, t)) - transform.position;
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
        bezierPoints = new Point[0];
        AddPoint();
    }
    /// <summary>
    /// 
    /// </summary>
    public void AddPoint()
    {
        List<Point> pointsToAdd = new List<Point>();

        Point newPoint = new Point(0f, 0f, 2f);

        if (this.bezierPoints.Length > 0)
        {
            Point previousPoint = GetLastPoint();
            newPoint.pointCoordinates += previousPoint.pointCoordinates;
            EnforceMode(newPoint, newPoint);

            previousPoint.SecondaryControlPoint = new ControlPoint(0f, 0f, 1f, previousPoint);
            previousPoint.SecondaryControlPoint.pointCoordinates += previousPoint.pointCoordinates;
        }
        else
        {
            Point initialisationPoint = new Point(0f, 0f, 0f);
            initialisationPoint.ControlPoint = new ControlPoint(0f, 0f, 1f, initialisationPoint);
            initialisationPoint.ControlPoint.pointCoordinates += initialisationPoint.pointCoordinates;

            pointsToAdd.Add(initialisationPoint);
        }

        newPoint.ControlPoint = new ControlPoint(0f, 0f, -1f, newPoint);
        newPoint.ControlPoint.pointCoordinates += newPoint.pointCoordinates;

        pointsToAdd.Add(newPoint);

        foreach (Point points in pointsToAdd)
        {
            if (PointCount > 0)
            {
                points.previousPoint = GetLastPoint();
                GetLastPoint().NextPoint = points;
            }

            Point[] newPointsArray = new Point[bezierPoints.Length + 1];

            for (int i = 0; i < bezierPoints.Length; i++)
            {
                newPointsArray[i] = bezierPoints[i];
            }
            newPointsArray[bezierPoints.Length] = points;

            bezierPoints = newPointsArray;
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
    public void SetControlPointMode(Point pointToSet, BezierControlPointMode newMode)
    {

        pointToSet.BezierControlPointMode = newMode;
        EnforceMode(pointToSet, pointToSet);
    }
}