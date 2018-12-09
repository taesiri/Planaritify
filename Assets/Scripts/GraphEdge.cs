using UnityEngine;

public class GraphEdge : MonoBehaviour {

	private LineRenderer _lineRenderer;
	
	public void SetData(Transform source, Transform sink)
	{
		if(!_lineRenderer)
			_lineRenderer = GetComponent<LineRenderer>();
		
		var startPos = new Vector3(source.position.x, source.position.y, 2);
		var endPps = new Vector3(sink.position.x, sink.position.y, 2);

		_lineRenderer.SetPosition(0, startPos);
		_lineRenderer.SetPosition(1, endPps);

//		Debug.Log("Updating endpoints");
	}

	public void UpdateEdgeEndPoints(Transform vertex, int endPoint)
	{
		if(!_lineRenderer)
			_lineRenderer = GetComponent<LineRenderer>();
		
		var pos = new Vector3(vertex.position.x, vertex.position.y, 2);
		_lineRenderer.SetPosition(endPoint, pos);
	}
}
