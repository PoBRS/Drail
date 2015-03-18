using UnityEngine;


public class SplineWalker : MonoBehaviour {

	public BezierSpline spline;
	public Point currentPoint;
	public Point currentPointInMainSpline;
	public float duration;
	
	private float progress;
	
	public bool lookForward;
	
	public SplineWalkerMode mode;

	private int currentRail = 1;

	private bool goingForward = true;
	
	private void Start()
	{
		GameElements.splineWalkerInstance = this;
		this.spline = GameElements.trackInstance.splineList [currentRail];

		currentPointInMainSpline = GameElements.trackInstance.mainSpline.GetFirstPoint ();
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
			currentPointInMainSpline = GameElements.trackInstance.mainSpline.GetCurrentPoint(progress);
		}

		Vector3 position = spline.GetPoint(progress) + (new Vector3 (0, 1, 0));
		transform.localPosition = position;

		if (lookForward) 
		{
			transform.LookAt(position + spline.GetDirection(progress));
		}
		ChangeRail ();
		CheckIfEndRail ();
	}
	
	private void CheckIfEndRail()
	{
	
	}

	private void GenerateRailOnRight()
	{

	}

	private void ChangeRail()
	{
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if(currentRail != 0)
			{
				currentRail--;
			}
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			if(currentRail != (GameElements.trackInstance.splineList.Count - 1))
			{
				currentRail++;
			}
		}
		this.spline = GameElements.trackInstance.splineList[currentRail];
		this.currentPoint = spline.GetCurrentPoint (progress);
	}
}