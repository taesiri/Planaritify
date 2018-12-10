using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public GameObject InGameMenuPanel;

	public void ShowHideMenu()
	{
		if (InGameMenuPanel.activeSelf)
		{
			InGameMenuPanel.SetActive(false);
		}
		else
		{
			InGameMenuPanel.SetActive(true);
		}
	}

	public void GoHome()
	{
		var lvlLoader = FindObjectOfType<LevelLoader>();
		if (lvlLoader)
		{
			lvlLoader.UnsubscribeSceneChangeEvent();
			SceneManager.MoveGameObjectToScene(lvlLoader.gameObject, SceneManager.GetActiveScene());
		}
		SceneManager.LoadScene("IntroScreen");
	}

	public void RestartCurrentLevel()
	{
		SceneManager.LoadScene(gameObject.scene.name);
	}
}