using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
	public GraphDatabase GraphDB;
	public GameObject GraphObjectPrefab;
	
	private int _numberOfGraphs;
	private int _currentGraphId = -1;
	private GameObject _currentGraph;

	public Text LevelText;

	public void Start()
	{
		_numberOfGraphs = GraphDB.NumberOfGraphs();

		LoadNextGraph();
	}
	
	public void LoadNextGraph()
	{
		_currentGraphId = ((_currentGraphId % _numberOfGraphs + _numberOfGraphs)  + 1) % _numberOfGraphs;
		LoadGraph(_currentGraphId);
	}

	public void LoadPrevGraph()
	{
		_currentGraphId = ((_currentGraphId % _numberOfGraphs + _numberOfGraphs)  - 1) % _numberOfGraphs;
		LoadGraph(_currentGraphId);
;	}

	public void LoadGraph(int id)
	{
		CleanCurrentGraph();
		LevelText.text = (1+_currentGraphId).ToString();
		
		var newGraph = Instantiate(GraphObjectPrefab, Vector3.zero, Quaternion.identity);
		newGraph.name = String.Format("Graph Object {0}", id);
		newGraph.GetComponent<GraphObjectScript>().DrawGraph(GraphDB, _currentGraphId);
		_currentGraph = newGraph;
	}
	
	public void CleanCurrentGraph()
	{
		Destroy(_currentGraph);
	}

	public int CurrentGraph
	{
		get { return _currentGraphId; }
	}
	
	public int LevelsBeforeCurrentGraph
	{
		get { return GraphDB.GetLevelsBeforeThis(_currentGraphId); }
	}
}
