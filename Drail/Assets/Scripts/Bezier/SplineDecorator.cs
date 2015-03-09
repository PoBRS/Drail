using UnityEngine;

public class SplineDecorator : MonoBehaviour {
	
	public BezierSpline spline;
	
	public int frequency;
	
	public bool lookForward;
	
	public GameObject Rail;
	
	private void Start () {
		//if (frequency <= 0 || items == null || items.Length == 0) {
		//	return;
		//}
		float stepSize = frequency;
	
			stepSize = 1f / (stepSize - 1);

		for (int p = 0, f = 0; f < frequency; f++) 
		{
			for (int i = 0; i < 1; i++, p++)
			{
				Transform item = Instantiate(Rail.transform) as Transform;
				Vector3 position = spline.GetPoint(p * stepSize);
				item.transform.localPosition = position;
				if (lookForward) 
				{
					item.transform.LookAt(position + spline.GetDirection(p * stepSize));
				}
				item.transform.parent = transform;
			}
		}
	}


}