using UnityEngine;

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
	}

	public void SolveGame()
	{
	}

	public void RestartCurrentLevel()
	{
	}
}