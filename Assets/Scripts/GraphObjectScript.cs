using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class GraphObjectScript : MonoBehaviour
{
	public GameObject EdgePrefab;
	public GameObject VertexPrefab;
	public GameObject WigglerPrefab;
	public bool EnableWiggle;

	public bool SelectGraphAtRandom;
	// TEMP
	public GraphDatabase GraphDatabase;
	public int SelectedGraph = 2;

	// Scaling and Spacing
	public float GlobalScaleFactor = 1;
	public Vector3 ScreenOffset = Vector3.zero;
	public GraphEmbeddingData EmbeddingData = new GraphEmbeddingData();
	public bool DrawOnStart = true;

	private bool _buildingGraph = false;
	
	public void Start()
	{
		if (DrawOnStart)
		{
			if (SelectGraphAtRandom)
			{
				var maxGraphs = GraphDatabase.NumberOfGraphs();
				SelectedGraph = (int) Random.Range(0, maxGraphs-0.1f);
			}
			Debug.Log("Drawing the graph: " + SelectedGraph);
			DrawGraph(GraphDatabase, SelectedGraph);
		}
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

	public void DrawRandomizedGraph()
	{
		if (!_buildingGraph)
		{
			DrawGraph(GraphDatabase, SelectedGraph, true);
		}
	}

	public void DrawGraph(GraphDatabase db, int selectedGraph, bool randomized = false)
	{
		_buildingGraph = true;
		
		var edgeList = db.GetEdgeListAt(selectedGraph);
		var embedding = db.GetEmbeddingAt(selectedGraph);
		
//		Debug.Log(edgeList);
//		Debug.Log(embedding);
		
		var vertexHashSet = new HashSet<Int32>();
		var edgesList = new List<EdgeData>();
		// Edge List
		Regex regex = new Regex(@"\[(.*?)\]");
		MatchCollection matches = regex.Matches(edgeList);

		var levelsBeforeThis = db.GetLevelsBeforeThis(selectedGraph);

		foreach (Match match in matches)
		{
			var endPoints = match.Groups[1].ToString().Split(',');
			var edgeData = new EdgeData {Source = int.Parse(endPoints[0]), Sink = int.Parse(endPoints[1])};

			vertexHashSet.Add(edgeData.Source);
			vertexHashSet.Add(edgeData.Sink);
			edgesList.Add(edgeData);
		}

		// Vertex List
		regex = new Regex(@"\{(.*?)\}");
		matches = regex.Matches(embedding);
		var positionsList = new List<Vector3>();

		foreach (Match match in matches)
		{
			var tmp = match.Groups[1].ToString().Split(',');
			var pos = new Vector3((float) double.Parse(tmp[0]), (float) double.Parse(tmp[1]), 0);
			positionsList.Add(pos);
		}

		var scaleOffset = CalculateGraphScale(positionsList);
		var vertexCounter = 1;

		var otherNodes = new List<Vector3>();

		foreach (var p in positionsList)
		{

			var correctPosition = GlobalScaleFactor *
			                      (ScreenOffset + Vector3.Scale(p - scaleOffset.Offset, scaleOffset.Scale));
			var spawnPosition = correctPosition;

			if (randomized)
			{
				spawnPosition = RandomPosition(1.78f, otherNodes, 0.625f);
			}

			var node = Instantiate(VertexPrefab, spawnPosition, Quaternion.identity);
			node.transform.parent = transform;
			var vScript = node.GetComponent<GraphVertex>();
			vScript.VertexId = vertexCounter;
			if (levelsBeforeThis == -1) vScript.LevelId = -2;
			else vScript.LevelId = levelsBeforeThis + vertexCounter;
			EmbeddingData.Add(correctPosition, vScript);
			vertexCounter++;

			otherNodes.Add(spawnPosition);
		}

		foreach (var edgeData in edgesList)
		{
			var edge = Instantiate(EdgePrefab, Vector3.zero, Quaternion.identity);
			var graphEdge = edge.GetComponent<GraphEdge>();

			var sourceVertex = EmbeddingData.GetVertexAt(edgeData.Source - 1);
			var sinkVertex = EmbeddingData.GetVertexAt(edgeData.Sink - 1);

			graphEdge.SetData(sourceVertex, sinkVertex);

			sourceVertex.AddNewEdge(graphEdge, 0);
			sinkVertex.AddNewEdge(graphEdge, 1);

			edge.transform.parent = transform;
		}

		if (EnableWiggle)
		{
			EmbeddingData.AttachPrefabToEachNode(WigglerPrefab);
		}
	}

	private Vector3 RandomPosition(float radius, List<Vector3> otherNodes, float minDistance)
	{
		var remainingTries = 5;
		while (remainingTries > 0)
		{
			var spawnPosition = Random.insideUnitCircle * radius;
			var temp = float.MaxValue;
			for (int i = 0; i < otherNodes.Count; i++)
			{
				var dist = Vector3.Distance(spawnPosition, otherNodes[i]);
				if (dist < temp)
				{
					temp = dist;
				}
			}

			if (temp > minDistance)
			{
				return spawnPosition;
			}

			remainingTries--;
		}

		return Random.insideUnitCircle * radius;
	}
}