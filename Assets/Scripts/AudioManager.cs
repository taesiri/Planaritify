using UnityEngine;

public class AudioManager : MonoBehaviour {
	public AudioClip[] ClipSound;
	public AudioSource SoundPlayer;

	public void PlaySuccessSound() {

		var selectedSound = ClipSound [Random.Range (0, ClipSound.Length)];
		SoundPlayer.clip = selectedSound;

		SoundPlayer.Play ();
	}
}
