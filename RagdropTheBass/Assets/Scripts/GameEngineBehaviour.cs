using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	private int pushDirection; // 0 = none, 1 = left, 2 = right, 3 = up, 4 = down
	private List<int> possibleDirections;
	private int indexDirection;



	public void SendLoudInput (float loudness)
	{
		if (Time.time - lastLoudInputTime > 0.5f) 
		{
			lastLoudInputTime = Time.time;
			foreach (Transform child in crowd) 
			{
				child.GetComponent<PublicBehaviour> ().ResolveLoudInput (loudness);
			}
			ragdoll.PushRagdoll (loudness, 0.2f*((pushDirection == 1)?(Vector2.left):(pushDirection == 2)?(Vector2.right):(pushDirection == 3)?(Vector2.up):(pushDirection == 4)?(Vector2.down):(Vector2.zero)));
		}
	}

	// Use this for initialization
	void Start () 
	{
		ragdollChest.AddForce (Vector3.forward * forwardForce * 3 + Vector3.up * upForce);

		PublicGeneration (peopleDensity, peoplePrefab);

		lastLoudInputTime = 0;

		indexDirection = 0;
		pushDirection = 0;
		possibleDirections = new List<int> ();
		possibleDirections.Add (1);
		possibleDirections.Add (2);
		possibleDirections.Add (3);
		possibleDirections.Add (4);

		StartCoroutine (WaitAndChangeDirection (5.0f));
	}

	IEnumerator WaitAndChangeDirection(float timer)
	{
		yield return new WaitForSeconds(timer);

		indexDirection++;
		if (indexDirection == 6) {
			indexDirection = 0;
			pushDirection = 0;
			possibleDirections = new List<int> ();
			possibleDirections.Add (1);
			possibleDirections.Add (2);
			possibleDirections.Add (3);
			possibleDirections.Add (4);
		} 
		else if (indexDirection == 3)
		{
			pushDirection = 0;
		}
		else
		{
			int i = Random.Range (0, possibleDirections.Count);
			pushDirection = possibleDirections[i];
			possibleDirections.RemoveAt (i);
		}

		foreach (Transform child in crowd)
		{
			child.GetComponent<PublicBehaviour> ().SetAnim (pushDirection);
		}

		StartCoroutine (WaitAndChangeDirection (timer));
	}
	
	// Update is called once per frame
	void Update () 
	{
		bool impulse = false;

		/*
		if (Input.GetKeyDown (KeyCode.UpArrow)) 
		{
			//destination.position += Vector3.forward;
			ragdollChest.AddForce(Vector3.up * upForce + Vector3.forward * forwardForce);
		} 
		else if (Input.GetKeyDown (KeyCode.DownArrow)) 
		{
			//destination.position -= Vector3.forward;
			ragdollChest.AddForce(Vector3.up * upForce - Vector3.forward * forwardForce);
		} 
		else if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			//destination.position -= Vector3.right;
			ragdollChest.AddForce(Vector3.up * upForce - Vector3.right * forwardForce);
		} 
		else if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			//destination.position += Vector3.right;
			ragdollChest.AddForce(Vector3.up * upForce + Vector3.right * forwardForce);
		}
		*/
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
