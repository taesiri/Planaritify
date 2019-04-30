using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameCore : MonoBehaviour
{
	private RaycastHit2D _hit;
	private readonly Vector2[] _touches = new Vector2[5];
	private readonly GraphVertex[] _vertices = new GraphVertex[5];

	[SerializeField] private GameState gameState;

	public TextMeshProUGUI LevelNameText;
	
	private GameState GameState
	{
		get => gameState;
		set
		{
			gameState = value;
			GameStateUpdate();
		}
	}

	public LevelLoader RefToLevelLoader { get; set; }

	public List<GraphVertex> listOfVertices;
	public List<GraphEdge> listOfEdges;

	private int _embarrassingSituationCounter;

	public TextMeshProUGUI EmbrassingText;
	public int CurrentLevel = -1;

	public GameObject LevelFinishedPanel;
	public GameObject[] MainMenuButton;

	public AudioManager AudioManagerInstance;
	
	private void GameStateUpdate()
	{
		if (GameState == GameState.Playing)
		{
			if (!IsInvoking("HandleGameState"))
				InvokeRepeating("HandleGameState", 2f, 0.5f);
		}
	}

	public void Start()
	{
		if (GameState == GameState.BeforePlaying)
		{
			if (LevelFinishedPanel)
				LevelFinishedPanel.SetActive(false);

			if (IsItIntersected())
			{
				StartGame();
			}
			else
			{
				FixGraphLayout();
				Debug.LogWarning("BAD GRAPH");
			}
		}
	}

	private void StartGame()
	{
		GameState = GameState.Playing;
		AdAnalyticsObject.GetInstance.LogLevelStart(CurrentLevel.ToString());
	}

	private void FixGraphLayout()
	{
		StartCoroutine(FadeInText(EmbrassingText, 0.005f));
		StartCoroutine(FixLayout(1));
	}

	private IEnumerator FixLayout(float delay)
	{
		_embarrassingSituationCounter++;
		yield return new WaitForSeconds(delay);

		var isSelfIntersected = IsItIntersected();

		while (!isSelfIntersected)
		{
			foreach (var graphVertex in listOfVertices)
			{
				var respawnPosition = Random.insideUnitCircle * 1.78f;
				graphVertex.MoveTo(respawnPosition, 3f);
			}

			var isMoving = true;
			while (isMoving)
			{
				foreach (var graphVertex in listOfVertices)
				{
					if (graphVertex.isMoving)
					{
						isMoving = true;
						break;
					}

					isMoving = false;
				}

				yield return new WaitForSeconds(0.5f);
			}

			isSelfIntersected = IsItIntersected();
			_embarrassingSituationCounter++;

			if (_embarrassingSituationCounter > 4)
			{
				RefToLevelLoader.LoadNextLevel();
			}
		}

		StartCoroutine(FadeOutText(EmbrassingText, 0.001f));
		StartGame();
	}

	private void GetVerticesAndEdges()
	{
		listOfVertices = FindObjectsOfType<GraphVertex>().ToList();
		listOfEdges = FindObjectsOfType<GraphEdge>().ToList();
	}

	public void Update()
	{
		if (GameState == GameState.Playing)
		{
			HandleTouchInputAndMovement();
		}
		else if (GameState == GameState.SelectLevelScreen)
		{
			HandleTouchDown();
		}
	}

	private void HandleGameState()
	{
		if (GameState == GameState.Playing)
		{
			if (Input.touchCount == 0)
			{
				var isIntersected = IsItIntersected();
				if (isIntersected)
				{
//				Debug.Log("Intersection!");
				}
				else
				{
					Debug.Log("Solved!!");
					LevelFinished();
				}
			}
		}
	}

	public void LevelFinished()
	{
		GameState = GameState.Ended;
		AudioManagerInstance.PlaySuccessSound();
		PlayerPrefs.SetInt(CurrentLevel.ToString(), 1);
		StartCoroutine(ZoomCameraIn());
		LevelFinishedPanel.SetActive(true);

		AdAnalyticsObject.GetInstance.LogLevelCompleted(CurrentLevel.ToString(), Time.timeSinceLevelLoad);

		
		foreach (var mGameObject in MainMenuButton)
		{
			mGameObject.SetActive(false);
		}
	}

	public void GoToNextLevel()
	{
		RefToLevelLoader.LoadNextLevel();
	}

	private void HandleTouchInputAndMovement()
	{
		foreach (Touch t in Input.touches)
		{
			try
			{
				_touches[t.fingerId] = Camera.main.ScreenToWorldPoint(Input.GetTouch(t.fingerId).position);
				if (Input.GetTouch(t.fingerId).phase == TouchPhase.Began)
				{
					_hit = Physics2D.Raycast(_touches[t.fingerId], Vector2.zero);
					if (_hit)
					{
						if (_hit.collider.gameObject)
						{
							var vertex = _hit.collider.gameObject.GetComponent<GraphVertex>();
							vertex.TouchDown();
							_vertices[t.fingerId] = vertex;
						}
					}
				}

				if (Input.GetTouch(t.fingerId).phase == TouchPhase.Moved)
				{
					if (_vertices[t.fingerId])
					{
						Vector3 worldPos = Camera.main.ScreenToWorldPoint((Input.GetTouch(t.fingerId).position));
						_vertices[t.fingerId].TouchMove(worldPos);
					}
				}

				if (Input.GetTouch(t.fingerId).phase == TouchPhase.Ended)
				{
					if (_vertices[t.fingerId])
					{
						_vertices[t.fingerId].TouchUp();
						_vertices[t.fingerId] = null;
					}
				}
			}
			catch (Exception exp)
			{
				Debug.LogWarning(exp.Message);
			}
		}
	}

	private void HandleTouchDown()
	{
		foreach (Touch t in Input.touches)
		{
			try
			{
				_touches[t.fingerId] = Camera.main.ScreenToWorldPoint(Input.GetTouch(t.fingerId).position);
				if (Input.GetTouch(t.fingerId).phase == TouchPhase.Began)
				{
					_hit = Physics2D.Raycast(_touches[t.fingerId], Vector2.zero);
					if (_hit)
					{
						if (_hit.collider.gameObject)
						{
							var vertex = _hit.collider.gameObject.GetComponent<GraphVertex>();
							vertex.TouchDown();
							_vertices[t.fingerId] = vertex;
						}
					}
				}

				if (Input.GetTouch(t.fingerId).phase == TouchPhase.Ended)
				{
					if (_vertices[t.fingerId])
					{
						_vertices[t.fingerId].TouchUp();
						_vertices[t.fingerId] = null;
					}
				}
			}
			catch (Exception exp)
			{
				Debug.LogWarning(exp.Message);
			}
		}
	}

	public bool IsItIntersected()
	{
		if (listOfEdges == null || listOfVertices == null)
		{
			GetVerticesAndEdges();
		}

		if (listOfEdges.Count == 0 || listOfVertices.Count == 0)
		{
			GetVerticesAndEdges();
		}

		foreach (var currentEdge in listOfEdges)
		{
			foreach (var otherEdge in listOfEdges)
			{
				if (currentEdge.GetEdgeId == otherEdge.GetEdgeId)
					continue;

				if (CGHelper.EdgesAreAdjacent(currentEdge, otherEdge))
					continue;

				// Edges are not adjacent and not the same! do the calculation

				if (CGHelper.DoesIntersect2(
					currentEdge.GetLineEndPoints(0),
					currentEdge.GetLineEndPoints(1),
					otherEdge.GetLineEndPoints(0),
					otherEdge.GetLineEndPoints(1)))
				{
					return true;
				}
			}
		}

		return false;
	}

	private GameState _stateBeforeMenu;

	public void MenuOn()
	{
		_stateBeforeMenu = GameState;
		GameState = GameState.Paused;
	}

	public void MenuOff()
	{
		GameState = _stateBeforeMenu;
	}

	// Animation Helpers
	IEnumerator ZoomCameraIn()
	{
		for (float f = 5f; f >= 0.01f; f -= 0.1f)
		{
			Camera.main.orthographicSize = f;
			yield return null;
		}

		Camera.main.orthographicSize = 0.001f;
	}


	IEnumerator FadeInText(Text textToFadeIn, float speed)
	{
		for (float f = 0f; f <= 1; f += 0.1f)
		{
			Color c = textToFadeIn.color;
			c.a = f;
			textToFadeIn.color = c;
			yield return new WaitForSeconds(speed);
			;
		}

		var fc = textToFadeIn.color;
		fc.a = 1;
		textToFadeIn.color = fc;
	}

	IEnumerator FadeOutText(Text textToFade, float speed)
	{
		for (float f = 1f; f >= 0; f -= 0.1f)
		{
			Color c = textToFade.color;
			c.a = f;
			textToFade.color = c;
			yield return new WaitForSeconds(speed);
		}

		var fc = textToFade.color;
		fc.a = 0;
		textToFade.color = fc;
	}
	
	
	IEnumerator FadeInText(TextMeshProUGUI textToFadeIn, float speed)
	{
		for (float f = 0f; f <= 1; f += 0.1f)
		{
			Color c = textToFadeIn.color;
			c.a = f;
			textToFadeIn.color = c;
			yield return new WaitForSeconds(speed);
			;
		}

		var fc = textToFadeIn.color;
		fc.a = 1;
		textToFadeIn.color = fc;
	}

	IEnumerator FadeOutText(TextMeshProUGUI textToFade, float speed)
	{
		for (float f = 1f; f >= 0; f -= 0.1f)
		{
			Color c = textToFade.color;
			c.a = f;
			textToFade.color = c;
			yield return new WaitForSeconds(speed);
		}

		var fc = textToFade.color;
		fc.a = 0;
		textToFade.color = fc;
	}

	public void DisableVertexTexts()
	{
		var gvs = FindObjectsOfType<GraphVertex>();
		foreach (var graphVertex in gvs)
		{
			graphVertex.AttachedText.text = "";
		}
	}

	public void UpdateCurrentLevelText(int currentLevel)
	{
		LevelNameText.text = "<" + currentLevel + ">";
	}
}