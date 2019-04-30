using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour
{
    public GameObject canvasObject;

    private void Awake()
    {
        Application.targetFrameRate = 1000;
    }

    private void Start()
    {
        AdAnalyticsObject.GetInstance.LogGameStarted();
    }

    public void LoadLevelSelectionScene()
    {
        canvasObject.SetActive(false);
        StartCoroutine(ZoomCameraIn());
    }
    
    IEnumerator ZoomCameraIn()
    {
        for (float f = 5f; f >= 0.01f; f -= 0.2f)
        {
            Camera.main.orthographicSize = f;
            yield return null;
        }

        Camera.main.orthographicSize = 0.001f;
        SceneManager.LoadScene("LevelSelectionScene");
    }
}