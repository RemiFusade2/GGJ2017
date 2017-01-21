using UnityEngine;
using System.Collections;

public class MicrophoneBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AudioClip clip = Microphone.Start("Built-in Microphone", true, 1, 44100);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
