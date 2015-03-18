using UnityEngine;

public class SplineDecorator : MonoBehaviour {
	
	public BezierSpline spline;
	
	public int frequency;
	
	public bool lookForward;
	
	public GameObject Rail;

	private void Awake () 
	{
		//GameElements.splineDecoratorInstance = this;
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

	public void AddRailSection(RailPoint pointAdded)
	{
		Vector3 pointCoordinates = pointAdded.pointCoordinates;
		Vector3 previousPointCoordinates = Vector3.zero;

		if (pointAdded != spline.GetFirstPoint() != null) 
		{
			//previousPointCoordinates = pointAdded.PreviousRailPoint.pointCoordinates;
		}
	
		float m_Length = (pointCoordinates.z - previousPointCoordinates.z);
		float m_Width = 1.0f;

		MeshBuilder meshBuilder = new MeshBuilder ();
		//Set up the vertices and triangles:
		meshBuilder.Vertices.Add(new Vector3(previousPointCoordinates.x -0.5f, previousPointCoordinates.y, previousPointCoordinates.z));
		meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(pointCoordinates.x - 0.5f, pointCoordinates.y, pointCoordinates.z));
		meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(pointCoordinates.x + 0.5f, pointCoordinates.y, pointCoordinates.z));
		meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.Vertices.Add(new Vector3(previousPointCoordinates.x + 0.5f, previousPointCoordinates.y, previousPointCoordinates.z));
		meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);
		
		meshBuilder.AddTriangle(0, 1, 2);
		meshBuilder.AddTriangle(0, 2, 3);
		
		//Create the mesh:
		MeshFilter filter = GetComponent<MeshFilter>();
		
		if (filter != null)
		{
			filter.sharedMesh = meshBuilder.CreateMesh();
		}
		Vector3 meshXPositionOffset = new Vector3 (0, 0, 0);

		//this.transform.position = pointAdded.pointCoordinates - meshXPositionOffset;

	}


}