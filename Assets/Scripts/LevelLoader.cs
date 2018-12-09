using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
	public GraphDatabase AllGraphDB;
	public int LevelToLoad;
	public GameObject LoadingCircle;
	public GameObject ScreenCanvas;

	public void Start()
	{
		DontDestroyOnLoad(this);
		Debug.Log("Start");

		SceneManager.activeSceneChanged += LevelChanged;
	}

	public void LoadLevel(int vertexId)
	{
		LevelToLoad = FindObjectOfType<LevelSelection>().LevelsBeforeCurrentGraph + vertexId;
		Debug.Log(LevelToLoad);

		if (ScreenCanvas)
		{
			ScreenCanvas.SetActive(false);
		}

		StartCoroutine(LoadLevelAsyncScene(0.899f));
	}

	public void LoadNextLevel()
	{
		LevelToLoad++;
		SceneManager.LoadScene("GameScene");
	}

	private void LevelChanged(Scene scene1, Scene scene2)
	{
		Debug.Log(scene1.name);
		Debug.Log(scene2.name);
		Debug.Log("OnLevelWasLoaded");
		var t = GameObject.FindWithTag("LevelText").GetComponent<Text>();
		if (t)
			t.text = LevelToLoad.ToString();
	}

	IEnumerator LoadLevelAsyncScene(float time)
	{
//		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");

		Vector3 originalScale = LoadingCircle.transform.localScale;
		Vector3 destinationScale = new Vector3(2.0f, 2.0f, 2.0f);

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

	}

	public void Update()
	{
		if (Input.GetKey(KeyCode.N))
		{
			LoadNextLevel();
		}
	}
}
