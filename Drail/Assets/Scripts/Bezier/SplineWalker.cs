using UnityEngine;

public class SplineWalker : MonoBehaviour {

	public BezierSpline spline;
	public Point currentPoint;

	public float duration;
	
	private float progress;
	
	public bool lookForward;
	
	public SplineWalkerMode mode;
	
	private bool goingForward = true;
	
	private void Start()
	{
		GameElements.splineWalkerInstance = this;
	}
	
	private void Update ()
	{
		if (goingForward) 
		{
			progress += Time.deltaTime / duration;
			if (progress > 1f) {
				if (mode == SplineWalkerMode.Once) {
					progress = 1f;
				}
				else if (mode == SplineWalkerMode.Loop) {
					progress -= 1f;
				}
				else {
					progress = 2f - progress;
					goingForward = false;
				}
			}
		}
		else {
			progress -= Time.deltaTime / duration;
			if (progress < 0f) {
				progress = -progress;
				goingForward = true;
			}
		}

		if (spline.GetCurrentPoint (progress) != currentPoint) 
		{
			GameElements.trackInstance.AutoGenerateNextRails();
			progress = 0f;
			currentPoint = spline.GetCurrentPoint (progress);
		}

		Vector3 position = spline.GetPoint(progress) + (new Vector3 (0, 1, 0));
		transform.localPosition = position;

		if (lookForward) 
		{
			transform.LookAt(position + spline.GetDirection(progress));
		}

		CheckIfEndRail ();
	}
	
	private void CheckIfEndRail()
	{
	
		if (currentPoint.isHole) 
		{
			Debug.Log ("Partie terminee");
		}
	}

	private void GenerateRailOnRight()
	{

	}
}