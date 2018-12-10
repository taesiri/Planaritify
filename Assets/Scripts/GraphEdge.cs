using System;
using UnityEngine;

public class GraphEdge : MonoBehaviour
{
	private LineRenderer _lineRenderer;

	public GraphVertex Source;
	public GraphVertex Sink;
	
	public Vector2 GetLineEndPoints(int idx)
	{
		return _lineRenderer.GetPosition(idx);
	}

	public Vector2Int GetVertexIds
	{
		get { return new Vector2Int(Source.VertexId, Sink.VertexId); }
	}

	public int GetEdgeId
	{
		get { return Source.VertexId * 10000 + Sink.VertexId; }
	}

	public void SetData(GraphVertex source, GraphVertex sink)
	{
		if (!_lineRenderer)
			_lineRenderer = GetComponent<LineRenderer>();

		Source = source;
		Sink = sink;
		
		var startPos = new Vector3(source.transform.position.x, source.transform.position.y, 2);
		var endPps = new Vector3(sink.transform.position.x, sink.transform.position.y, 2);

		_lineRenderer.SetPosition(0, startPos);
		_lineRenderer.SetPosition(1, endPps);

	}

	public void UpdateEdgeEndPoints(Transform vertex, int endPoint)
	{
		if (!_lineRenderer)
			_lineRenderer = GetComponent<LineRenderer>();

		var pos = new Vector3(vertex.position.x, vertex.position.y, 2);
		_lineRenderer.SetPosition(endPoint, pos);
	}
}
