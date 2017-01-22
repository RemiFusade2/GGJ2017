using UnityEngine;
using System.Collections;

public class GroundBehaviour : MonoBehaviour {

	public GameEngineBehaviour gameEngine;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.collider.tag.Equals ("Player")) {
			gameEngine.GameOver ();
		}	
	}
}
