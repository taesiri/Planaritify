using UnityEngine;

public class CaptureScreenshot : MonoBehaviour {

	public void Update()
	{
		if (Input.GetKey(KeyCode.S))
		{
			ScreenCapture.CaptureScreenshot("SomeLevel.png");
		}
	}
	
}
