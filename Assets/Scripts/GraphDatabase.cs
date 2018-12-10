using System;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class GraphDatabase : ScriptableObject
{
	public bool ReadFromFile;
	public TextAsset DataBaseTextFile;
	[TextArea(10, 18)] public string GraphDataBaseSource;
	public int LineMultiplier = 3;
	private String[] _cache;

	public string GetEdgeListAt(int selectedGraph)
	{
		if (ReadFromFile)
		{
			if (_cache == null || _cache.Length == 0)
			{
				_cache = DataBaseTextFile.text.Split('\n');
			}

			return _cache[(selectedGraph * LineMultiplier) + 1];
		}

		return GraphDataBaseSource.Split('\n')[(selectedGraph * LineMultiplier) + 2];
	}

	public string GetEmbeddingAt(int selectedGraph)
	{
		if (ReadFromFile)
		{
			if (_cache == null || _cache.Length == 0)
			{
				_cache = DataBaseTextFile.text.Split('\n');
			}

			return _cache[(selectedGraph * LineMultiplier) + 0];
		}

		return GraphDataBaseSource.Split('\n')[(selectedGraph * LineMultiplier) + 1];
	}

	public int GetLevelsBeforeThis(int selectedGraph)
	{
		if (LineMultiplier == 2) return -1;
		if (selectedGraph == 0) return 0;
		return int.Parse(GraphDataBaseSource.Split('\n')[((selectedGraph - 1) * LineMultiplier) + 0]);
	}

	public int NumberOfGraphs()
	{
		if (ReadFromFile)
		{
			if (_cache == null || _cache.Length == 0)
			{
				_cache = DataBaseTextFile.text.Split('\n');
			}

			return _cache.Length / LineMultiplier;
		}

		return GraphDataBaseSource.Split('\n').Length / LineMultiplier;
	}
}