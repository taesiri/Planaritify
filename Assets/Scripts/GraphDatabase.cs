using System;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class GraphDatabase : ScriptableObject
{
	[TextArea(10, 18)] public string GraphDataBaseSource;

	public string GetEdgeListAt(int selectedGraph)
	{
		return GraphDataBaseSource.Split('\n')[(selectedGraph * 3) + 2];
	}

	public string GetEmbeddingAt(int selectedGraph)
	{
		return GraphDataBaseSource.Split('\n')[(selectedGraph * 3) + 1];
	}

	public int GetLevelsBeforeThis(int selectedGraph)
	{
		if (selectedGraph == 0) return 0;
		return int.Parse(GraphDataBaseSource.Split('\n')[((selectedGraph - 1) * 3) + 0]);
	}

	public int NumberOfGraphs()
	{
		return GraphDataBaseSource.Split('\n').Length / 3;
	}
}