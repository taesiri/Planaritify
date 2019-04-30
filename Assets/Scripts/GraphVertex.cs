using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraphVertex : MonoBehaviour
{
	public List<KeyValuePair<GraphEdge, int>> ConnectedEdges = new List<KeyValuePair<GraphEdge, int>>();
	public int VertexId = -1;
	public VertexType VertexType;
	public int LevelId = -1;
	
	public TextMeshPro AttachedText;

	private GameCore _gameCore;
	
	public GameCore GetGameCore
	{
		get
		{
			if (!_gameCore)
				_gameCore = FindObjectOfType<GameCore>();

			return _gameCore;
		}
	}
	
	public void AddNewEdge(GraphEdge edge, int endPoint)
	{
		ConnectedEdges.Add(new KeyValuePair<GraphEdge, int>(edge, endPoint));
	}

	public void ForceUpdate()
	{
		foreach (var connectedEdge in ConnectedEdges)
		{
			connectedEdge.Key.UpdateEdgeEndPoints(transform, connectedEdge.Value);
		}
	}

	private void Start()
	{
		if (VertexType == VertexType.LevelScreen)
		{
			AttachedText.text = LevelId.ToString();
		}
	}

//	public Transform AnchorBaseTransform;
//	public SpriteRenderer InnerCircle;
//	public SpriteRenderer OuterCircle;

//	public List<EdgeScript> ConnectedEdges;
	public List<int> Indexes;
	public float smoothing = 3f;
	public bool isMoving = false;
	public bool isMovedSinceStart = false;

//	public void Awake() {
//		ConnectedEdges = new List<EdgeScript> ();
//		Indexes = new List<int> ();
//	}

	public void TouchDown() {
//		InnerCircle.color = Color.green;
		if (VertexType == VertexType.LevelScreen)
		{
//			FindObjectOfType<GameCore>()
			FindObjectOfType<GameCore>().DisableVertexTexts();
			FindObjectOfType<LevelLoader>().LoadLevel(VertexId);
		}
	}

	public void TouchUp() {
//		InnerCircle.color = new Color (151/255f, 0, 204/255f);
		ForceUpdate ();
		GetGameCore.IsItIntersected();
	}

	public void TouchMove(Vector3 pos) {
//		InnerCircle.color = Color.blue;

		if (VertexType == VertexType.GameScreen)
		{
			transform.position = new Vector3(pos.x, pos.y, 0);
			ForceUpdate ();
			isMovedSinceStart = true;
		}
//		else if (VertextType == VertextType.IntroScreen)
//		{
//			Debug.Log(VertexId);
//		}
	}

//	public void AddEdge(EdgeScript edge, int index) {
//		ConnectedEdges.Add (edge);
//		Indexes.Add (index);
//
//		ForceUpdate ();
//	}
//
//	public void ForceUpdate() {
//		var coordinates = new Vector3 (transform.position.x, transform.position.y, 2);
//
//		for (int i = 0; i < ConnectedEdges.Count; i++) {
//			ConnectedEdges [i].LineSegment.SetPosition (Indexes [i], coordinates);
//		}
//	}

	public void MoveTo(Vector3 newPosition, float smoothFactor) {
		StartCoroutine ("Movement", newPosition);
		smoothing = smoothFactor;
	}

	IEnumerator Movement (Vector3 target)
	{
		isMoving = true;
		while(Vector3.Distance(transform.position, target) > 0.05f)
		{
			transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);
			ForceUpdate ();
			yield return null;
		} 
		isMoving = false;
	}


	public void Starify() {
		if (!isMovedSinceStart) {
//			InnerCircle.color = new Color (255 / 255f, 206 / 255f, 0);
		}
	}
	
}

public enum VertexType
{
	IntroScreen,
	GameScreen,
	LevelScreen
}