using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor
{
	
	private const int lineSteps = 10;
	private const int stepsPerCurve = 10;
	private const float directionScale = 0.5f;

	private BezierSpline spline;
	private Transform handleTransform;
	private Quaternion handleRotation;

	private const float handleSize = 0.04f;
	private const float pickSize = 0.06f;
	
	private Point selectedPoint = null;

	private void OnSceneGUI ()
	{
		spline = target as BezierSpline;

		if (spline.bezierPoints != null) {
			if (spline.bezierPoints.Count > 1) {
				handleTransform = spline.transform;
				handleRotation = Tools.pivotRotation == PivotRotation.Local ?
				handleTransform.rotation : Quaternion.identity;

				Vector3 pointCoordinates = Vector3.zero;
				Vector3 controlPointCoordinates = Vector3.zero;
				Vector3 secondaryControlPointCoordinates = Vector3.zero;

				foreach (RailPoint point in spline.bezierPoints) {
					if (point.controlPoints.Count > 0) {
						Vector3 previousPointCoordinates = pointCoordinates;
						Vector3 previousControlPointCoordinates = controlPointCoordinates;
						Vector3 previousSecondaryControlPointCoordinates = secondaryControlPointCoordinates;

						Handles.color = Color.white;
						pointCoordinates = ShowPoint (point);
						Handles.color = Color.gray;
						controlPointCoordinates = ShowPoint (point.controlPoints [0]);

						if (point.controlPoints.Count == 2) {
							secondaryControlPointCoordinates = ShowPoint (point.controlPoints [1]);
							Handles.DrawLine (pointCoordinates, secondaryControlPointCoordinates);
						}
				
						Handles.DrawLine (pointCoordinates, controlPointCoordinates);

						if (point != spline.GetFirstPoint ()) {
							if (point != spline.bezierPoints[1]) {
								Handles.DrawBezier (previousPointCoordinates, pointCoordinates, previousSecondaryControlPointCoordinates, controlPointCoordinates, Color.white, null, 2f);
							} else {
								Handles.DrawBezier (previousPointCoordinates, pointCoordinates, previousControlPointCoordinates, controlPointCoordinates, Color.white, null, 2f);
							}
					
						}
					}
				}

				ShowDirections ();
			}
		} else {
			spline.Reset ();
		}
	}
	
	private void ShowDirections ()
	{
		Handles.color = Color.green;
		Vector3 point = spline.GetPoint (0f);
		Handles.DrawLine (point, point + spline.GetDirection (0f) * directionScale);
		int steps = stepsPerCurve * spline.PointCount;
		for (int i = 1; i <= steps; i++) {
			point = spline.GetPoint (i / (float)steps);
			Handles.DrawLine (point, point + spline.GetDirection (i / (float)steps) * directionScale);
		}
	}

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();
		spline = target as BezierSpline;
		EditorGUI.BeginChangeCheck ();
		if (selectedPoint != null && selectedPoint != spline.GetLastPoint ()) {
			DrawSelectedPointInspector ();
		}
		if (GUILayout.Button ("Add Curve")) {
			Undo.RecordObject (spline, "Add Curve");
			spline.AddPoint (new Vector3(0,0,3));
			EditorUtility.SetDirty (spline);
		}
	}

	private void DrawSelectedPointInspector ()
	{
		GUILayout.Label ("Selected Point");
		EditorGUI.BeginChangeCheck ();
		Vector3 point = EditorGUILayout.Vector3Field ("Position", selectedPoint.pointCoordinates);
		if (EditorGUI.EndChangeCheck ()) {
			Undo.RecordObject (spline, "Move Point");
			EditorUtility.SetDirty (spline);
			spline.SetControlPoint (selectedPoint, point);
		}

		EditorGUI.BeginChangeCheck ();


		BezierControlPointMode mode = GetControlPointModeForEditor ();
	

		if (EditorGUI.EndChangeCheck ()) {
			Undo.RecordObject (spline, "Change Point Mode");

			spline.SetControlPointMode ((selectedPoint as RailPoint), mode);
			EditorUtility.SetDirty (spline);
		}
	}

	private BezierControlPointMode GetControlPointModeForEditor ()
	{
		BezierControlPointMode mode = BezierControlPointMode.Free;
		if (selectedPoint is RailPoint) {
			mode = (BezierControlPointMode)EditorGUILayout.EnumPopup ("Mode", (selectedPoint as RailPoint).BezierControlPointMode);
		}
		return mode;		
	}

	private static Color[] modeColors = {
		Color.white,
		Color.yellow,
		Color.cyan
	};

	private Vector3 ShowPoint (Point pointToShow)
	{

		if (pointToShow == null) {
			return Vector3.zero;
		}
		Vector3 point = handleTransform.TransformPoint (pointToShow.pointCoordinates);
		float size = AssignPointSize (point);

		ChangeHandlesColorPointMode (pointToShow);

		if (Handles.Button (point, handleRotation, size * handleSize, size * pickSize, Handles.DotCap)) {
			selectedPoint = pointToShow;
			Repaint ();
		}

		if (selectedPoint == pointToShow) {
			EditorGUI.BeginChangeCheck ();
			point = Handles.DoPositionHandle (point, handleRotation);
			if (EditorGUI.EndChangeCheck ()) {
				Undo.RecordObject (spline, "Move Point");
				EditorUtility.SetDirty (spline);
				spline.SetControlPoint (selectedPoint, handleTransform.InverseTransformPoint (point));
			}
		}
		return point;
	}

	private float AssignPointSize (Vector3 pointToCheck)
	{
		float size = HandleUtility.GetHandleSize (pointToCheck);
		if (selectedPoint == spline.GetFirstPoint ()) {
			size *= 2f;
		}
		return size;
	}

	private void ChangeHandlesColorPointMode (Point pointToShow)
	{
		if (pointToShow is ControlPoint) {
			//Handles.color = modeColors [(int)(pointToShow as ControlPoint).LinkedRailPoint.BezierControlPointMode];
		} else {
			Handles.color = modeColors [(int)(pointToShow as RailPoint).BezierControlPointMode];
		}
	}
}