using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour
{
    public GameObject CanvasObject;
    public void LoadLevelSelectionScene()
    {
        CanvasObject.SetActive(false);
        StartCoroutine(ZoomCameraIn());
    }
    
    IEnumerator ZoomCameraIn()
    {
        for (float f = 5f; f >= 0.01f; f -= 0.1f)
        {
            Camera.main.orthographicSize = f;
            yield return null;
        }

        Camera.main.orthographicSize = 0.001f;
        SceneManager.LoadScene("LevelSelectionScene");
    }
}