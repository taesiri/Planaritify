using System.Collections.Generic;
using UnityEngine;

public class LevelDesigner : MonoBehaviour
{

	public GameObject EdgePrefab;
	public GameObject VertexPrefab;

	public float GlobalScaleFactor = 1;
	public Vector3 ScreenOffset = Vector3.zero;
	public GraphEmbeddingData EmbeddingData = new GraphEmbeddingData();

	public void Start()
	{
		DrawGraph();
	}

	static ScaleOffset CalculateGraphScale(List<Vector3> data)
	{
		var min = new Vector2(float.MaxValue, float.MaxValue);
		var max = new Vector2(float.MinValue, float.MinValue);

		foreach (var p in data)
		{
			if (p.x > max.x) max.x = p.x;
			if (p.y > max.y) max.y = p.y;

			if (p.x < min.x) min.x = p.x;
			if (p.y < min.y) min.y = p.y;
		}

		var xScale = 1f / (max.x - min.x);
		var yScale = 1f / (max.y - min.y);

		return new ScaleOffset {Scale = new Vector3(xScale, yScale, 1), Offset = new Vector3(min.x, min.y, 0)};
	}

	class ScaleOffset
	{
		public Vector3 Scale;
		public Vector3 Offset;
	}

	public void DrawGraph()
	{
//		var edgeList = db.GetEdgeListAt(selectedGraph);
//		var embedding = db.GetEmbeddingAt(selectedGraph);

//		var vertexHashSet = new HashSet<Int32>();
//		var edgesList = new List<EdgeData>();
//		// Edge List
//		Regex regex = new Regex(@"\[(.*?)\]");
//		MatchCollection matches = regex.Matches(edgeList);
//
//		foreach (Match match in matches)
//		{
//			var endPoints = match.Groups[1].ToString().Split(',');
//			var edgeData = new EdgeData {Source = int.Parse(endPoints[0]), Sink = int.Parse(endPoints[1])};
//
//			vertexHashSet.Add(edgeData.Source);
//			vertexHashSet.Add(edgeData.Sink);
//			edgesList.Add(edgeData);
//		}
//
//		// Vertex List
//		regex = new Regex(@"\{(.*?)\}");
//		matches = regex.Matches(embedding);
//		var positionsList = new List<Vector3>();
//
//		foreach (Match match in matches)
//		{
//			var tmp = match.Groups[1].ToString().Split(',');
//			var pos = new Vector3(float.Parse(tmp[0]), float.Parse(tmp[1]), 0);
//			positionsList.Add(pos);
//		}
//
//		var scaleOffset = CalculateGraphScale(positionsList);
//		var vertexCounter = 1;
//		
//		foreach (var p in positionsList)
//		{
//			var nodePosition = GlobalScaleFactor *
//			                   (ScreenOffset + Vector3.Scale(p - scaleOffset.Offset, scaleOffset.Scale));
//			
//			var node = Instantiate(VertexPrefab, nodePosition,Quaternion.identity);
//			node.transform.parent = transform;
//			var vScript = node.GetComponent<GraphVertex>();
//			vScript.VertexId = vertexCounter;
//			EmbeddingData.Add(nodePosition, vScript);
//			
//			vertexCounter++;
//		}
//
//		foreach (var edgeData in edgesList)
//		{
//			var edge = Instantiate(EdgePrefab, Vector3.zero, Quaternion.identity);
//			var graphEdge = edge.GetComponent<GraphEdge>();
//
//			var sourceVertex = EmbeddingData.GetVertexAt(edgeData.Source - 1);
//			var sinkVertex = EmbeddingData.GetVertexAt(edgeData.Sink - 1);
//
//			graphEdge.SetData(sourceVertex.transform, sinkVertex.transform);
//
//			sourceVertex.AddNewEdge(graphEdge, 0);
//			sinkVertex.AddNewEdge(graphEdge, 1);
//			
//			edge.transform.parent = transform;
//		}
	}
}