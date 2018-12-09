using UnityEngine;

public class GraphWiggle : MonoBehaviour
{
	public GraphVertex NodeToWiggle;

	public float Radius  = 0.2f;
	public float Speed = 0.2f;
	
	private Vector2 _baseStartPoint;
	private Vector2 _destination;
	private Vector2 _startPosition;
	private float _progress;

	public void Start () {
		_startPosition = NodeToWiggle.transform.localPosition;
		_baseStartPoint = NodeToWiggle.transform.localPosition;
		_progress = 0.0f;

		PickNewRandomDestination();
	}
 
	public void Update () {
		bool isDone = false;
		_progress += Speed * Time.deltaTime;
		if (_progress >= 1.0f)
		{
			_progress = 1.0f;
			isDone = true;
		}
		NodeToWiggle.transform.localPosition = (_destination * _progress) + _startPosition * (1 - _progress);
		NodeToWiggle.ForceUpdate();
		if (isDone)
		{
			_startPosition = _destination;
			PickNewRandomDestination();
			_progress = 0.0f;
		}
	}

	void PickNewRandomDestination()
	{
		_destination = Random.insideUnitCircle * Radius + _baseStartPoint;
	}
}
