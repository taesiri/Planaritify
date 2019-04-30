using System;
using TMPro;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
	public GraphDatabase GraphDB;
	public GameObject GraphObjectPrefab;
	
	private int _numberOfGraphs;
	private int _currentGraphId = -1;
	private GameObject _currentGraph;

	public TextMeshProUGUI levelText;

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

	private void LoadGraph(int id)
	{
		CleanCurrentGraph();
		levelText.text = (1+_currentGraphId).ToString();
		
		var newGraph = Instantiate(GraphObjectPrefab, Vector3.zero, Quaternion.identity);
		newGraph.name = String.Format("Graph Object {0}", id);
		newGraph.GetComponent<GraphObjectScript>().DrawGraph(GraphDB, _currentGraphId);
		_currentGraph = newGraph;
	}

	private void CleanCurrentGraph()
	{
		Destroy(_currentGraph);
	}

	public int CurrentGraph => _currentGraphId;

	public int LevelsBeforeCurrentGraph => GraphDB.GetLevelsBeforeThis(_currentGraphId);
}
