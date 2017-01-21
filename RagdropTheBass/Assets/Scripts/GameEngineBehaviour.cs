using UnityEngine;
using System.Collections;

public class GameEngineBehaviour : MonoBehaviour {

	public RagdollBehaviour ragdoll;
	public Rigidbody ragdollChest;

	public float upForce;
	public float forwardForce;

	public Transform crowd;

	public Transform destination;

	public Transform peopleStart;
	public GameObject peoplePrefab;
	public float peopleDensity;

	private float lastLoudInputTime;

	public void SendLoudInput (float loudness)
	{
		if (Time.time - lastLoudInputTime > 0.5f) 
		{
			lastLoudInputTime = Time.time;
			foreach (Transform child in crowd) 
			{
				child.GetComponent<PublicBehaviour> ().ResolveLoudInput (loudness);
			}
			ragdoll.PushRagdoll (loudness, Vector2.zero);
		}
	}

	// Use this for initialization
	void Start () 
	{
		ragdollChest.AddForce (Vector3.forward * forwardForce * 3 + Vector3.up * upForce);

		PublicGeneration (peopleDensity, peoplePrefab);

		lastLoudInputTime = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		bool impulse = false;

		if (Input.GetKeyDown (KeyCode.UpArrow)) 
		{
			//destination.position += Vector3.forward;
			ragdoll.GetComponent<Rigidbody>().AddForce(Vector3.up * upForce + Vector3.forward * forwardForce);
		} 
		else if (Input.GetKeyDown (KeyCode.DownArrow)) 
		{
			//destination.position -= Vector3.forward;
			ragdoll.GetComponent<Rigidbody>().AddForce(Vector3.up * upForce - Vector3.forward * forwardForce);
		} 
		else if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			//destination.position -= Vector3.right;
			ragdoll.GetComponent<Rigidbody>().AddForce(Vector3.up * upForce - Vector3.right * forwardForce);
		} 
		else if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			//destination.position += Vector3.right;
			ragdoll.GetComponent<Rigidbody>().AddForce(Vector3.up * upForce + Vector3.right * forwardForce);
		}
	}


	public void PublicGeneration(float density, GameObject prefab)
	{
		for (float x = -20; x < 20; x += density)
		{
			for (float y = 0; y < 20; y += density)
			{
				Vector3 position = peopleStart.transform.position + Vector3.right * x + Vector3.forward * y;

				GameObject newPeople = (GameObject)Instantiate (prefab, position, Quaternion.identity);
				newPeople.transform.parent = peopleStart;

				newPeople.GetComponent<PublicBehaviour> ().ragdoll = ragdollChest.transform;
			}
		}
	}
}
