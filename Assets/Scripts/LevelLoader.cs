using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
	public int LevelToLoad;
	public GameObject LoadingCircle;
	public GameObject ScreenCanvas;

	public int TotalGames = 5;
	
	public void Start()
	{
		DontDestroyOnLoad(this);
		SceneManager.activeSceneChanged += LevelChanged;
	}

	public void LoadLevel(int vertexId)
	{
		LevelToLoad = FindObjectOfType<LevelSelection>().LevelsBeforeCurrentGraph + vertexId;
		if (ScreenCanvas)
		{
			ScreenCanvas.SetActive(false);
		}
		StartCoroutine(LoadLevelAsyncScene(0.899f));
	}

	public void LoadNextLevel()
	{
		LevelToLoad++;
		if (TotalGames >= LevelToLoad)
			SceneManager.LoadScene("GameScene");
		else
		{
			Destroy(gameObject);
			SceneManager.LoadScene("GameOver");
		}
	}

	private void LevelChanged(Scene scene1, Scene scene2)
	{
		if (TotalGames >= LevelToLoad)
		{
			var gCore = FindObjectOfType<GameCore>();
			if (gCore)
			{
				gCore.CurrentLevel = LevelToLoad;
				gCore.RefToLevelLoader = this;
				gCore.UpdateCurrentLevelText(LevelToLoad);
			}
			
			if (scene2.name == "GameScene")
			{
				var gBS = FindObjectOfType<GraphObjectScript>();
				if (gBS)
				{
					if (LevelToLoad >= gBS.GraphDatabase.NumberOfGraphs() - 1)
					{
						// Game Finished!
					}
					else
					{
						gBS.SelectedGraph = LevelToLoad;
						gBS.DrawRandomizedGraph();
					}
				}
			}

		}
	}

	public void UnsubscribeSceneChangeEvent()
	{
		SceneManager.activeSceneChanged -= LevelChanged;
	}

	IEnumerator LoadLevelAsyncScene(float time)
	{
		Vector3 originalScale = LoadingCircle.transform.localScale;
		Vector3 destinationScale = new Vector3(3.0f, 3.0f, 3.0f);

		float currentTime = 0.0f;
		do
		{
			LoadingCircle.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
			currentTime += Time.deltaTime;
			yield return null;
		} while (currentTime <= time); // && !asyncLoad.isDone

		SceneManager.LoadScene("GameScene");

		originalScale = LoadingCircle.transform.localScale;
		destinationScale = Vector3.zero;
		currentTime = 0.0f;

		do
		{
			LoadingCircle.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
			currentTime += Time.deltaTime;
			yield return null;
		} while (currentTime <= time);
		LoadingCircle.transform.localScale = Vector3.zero;
	}
}
