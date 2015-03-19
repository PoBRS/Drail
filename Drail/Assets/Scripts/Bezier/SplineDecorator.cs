using UnityEngine;

public class SplineDecorator : MonoBehaviour {


	private void Awake () 
	{
		GameElements.splineDecoratorInstance = this;
		//float stepSize = frequency;
		//
		//stepSize = 1f / (stepSize - 1);
		//
		//for (int p = 0, f = 0; f < frequency; f++) 
		//{
		//	for (int i = 0; i < 1; i++, p++)
		//	{
		//		Transform item = Instantiate(Rail.transform) as Transform;
		//		Vector3 position = spline.GetPoint(p * stepSize);
		//		item.transform.localPosition = position;
		//		if (lookForward) 
		//		{
		//			item.transform.LookAt(position + spline.GetDirection(p * stepSize));
		//		}
		//		item.transform.parent = transform;
		//	}
		//}
	}

	public void AddRailSection(BezierSpline spline)
	{
		Vector3 firstPointCoordinates = spline.GetFirstPoint ().pointCoordinates;
		Vector3 previousSamplePoint = Vector3.zero;
		Vector3 previousVectorX1 = Vector3.zero;
		Vector3 previousVectorX2 = Vector3.zero;
		MeshBuilder meshBuilder = new MeshBuilder ();

		int trianglesIndexes = 0;

		for (float pointInSpline = 0; pointInSpline < 1; pointInSpline += 0.001f) 
		{
			Vector3 currentSamplePoint = spline.GetPoint (pointInSpline);
	
			//float m_Length = (pointCoordinates.z - previousPointCoordinates.z);
			//float m_Width = 1.0f;

				Vector3 perpendicularVector = Vector3.Cross(spline.GetDirection(pointInSpline), Vector3.up);
				Vector3 clampedPerpendicularVector = Vector3.ClampMagnitude(perpendicularVector, 1);

				Vector3 currentVectorX1 = new Vector3 (currentSamplePoint.x, currentSamplePoint.y, currentSamplePoint.z) + clampedPerpendicularVector;
				Vector3 currentVectorX2 = new Vector3 (currentSamplePoint.x, currentSamplePoint.y, currentSamplePoint.z) - clampedPerpendicularVector;

			if (currentSamplePoint != spline.GetPoint (0)) 
			{
				//Set up the vertices and triangles:
				meshBuilder.Vertices.Add (previousVectorX1);
				//meshBuilder.UVs.Add (new Vector2 (0.0f, 0.0f));
				//meshBuilder.Normals.Add (Vector3.up);
		
				meshBuilder.Vertices.Add (currentVectorX1);
				//meshBuilder.UVs.Add (new Vector2 (0.0f, 1.0f));
				//meshBuilder.Normals.Add (Vector3.up);
		
				meshBuilder.Vertices.Add (currentVectorX2);
				//meshBuilder.UVs.Add (new Vector2 (1.0f, 1.0f));
				//meshBuilder.Normals.Add (Vector3.up);
			
				meshBuilder.Vertices.Add (previousVectorX2);
				//meshBuilder.UVs.Add (new Vector2 (1.0f, 0.0f));
				//meshBuilder.Normals.Add (Vector3.up);

		
				meshBuilder.AddTriangle (trianglesIndexes, trianglesIndexes + 1, trianglesIndexes + 2);
				meshBuilder.AddTriangle (trianglesIndexes, trianglesIndexes + 2, trianglesIndexes + 3);

				trianglesIndexes += 4;
			}
			previousVectorX1 = currentVectorX1;
				previousVectorX2 = currentVectorX2;
		

			previousSamplePoint = currentSamplePoint;
		}
			//Create the mesh:
			MeshFilter filter = spline.gameObject.GetComponent<MeshFilter> ();
		
			if (filter != null) {
				filter.sharedMesh = meshBuilder.CreateMesh ();
			}
			//Vector3 meshXPositionOffset = new Vector3 (0, 0, 0);

			//this.transform.position = pointAdded.pointCoordinates - meshXPositionOffset;
		}



}