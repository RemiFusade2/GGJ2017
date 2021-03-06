﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEngineBehaviour : MonoBehaviour {

	public RagdollBehaviour ragdoll;
	public Rigidbody ragdollChest;

	public float upForce;
	public float forwardForce;

	public Transform crowd;

	public Transform destination;

	public Transform peopleStart;
	public List<GameObject> peoplePrefabs;
	public float peopleDensity;

	private float lastLoudInputTime;

	private int pushDirection; // 0 = none, 1 = left, 2 = right, 3 = up, 4 = down
	private List<int> possibleDirections;
	private int indexDirection;


	public Text scoreText;
	public Text bestScoreText;

	private float timeInAir;
	private float totalTimeInAir;

	public dropthatbase musicManager;

	private bool gameOver;


	public void SendLoudInput (float loudness)
	{
		if (Time.time - lastLoudInputTime > 0.5f) 
		{
			lastLoudInputTime = Time.time;
			foreach (Transform child in crowd) 
			{
				child.GetComponent<PublicBehaviour> ().ResolveLoudInput (loudness);
			}
			ragdoll.PushRagdoll (loudness, 0.2f*((pushDirection == 1)?(Vector2.right):(pushDirection == 2)?(Vector2.left):(pushDirection == 3)?(Vector2.up):(pushDirection == 4)?(Vector2.down):(Vector2.zero)));
		}
	}

	// Use this for initialization
	void Start () 
	{
		ragdollChest.AddForce (Vector3.forward * forwardForce * 3 + Vector3.up * upForce);

		PublicGeneration (peopleDensity, peoplePrefabs);

		lastLoudInputTime = 0;

		indexDirection = 0;
		pushDirection = 0;
		possibleDirections = new List<int> ();
		possibleDirections.Add (1);
		possibleDirections.Add (2);
		possibleDirections.Add (3);
		possibleDirections.Add (4);

		StartCoroutine (WaitAndChangeDirection (5.0f));

		timeInAir = 0;
		totalTimeInAir = 0;
		gameOver = false;
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

		if (ragdoll.CanBePushed () || gameOver) 
		{
			timeInAir = 0;			
		} 
		else
		{
			timeInAir += Time.deltaTime;
			totalTimeInAir += Time.deltaTime;
		}

		if (timeInAir > 0) 
		{
			bestScoreText.text = Mathf.Round(1000*totalTimeInAir)/1000.0f + "s";
			scoreText.text = Mathf.Round(1000*timeInAir)/1000.0f  + "s in air!";
		} 
		else 
		{
			scoreText.text = "";
		}


		if (totalTimeInAir > 15.0f) {
			musicManager.SetMusicState (6);
		} else if (totalTimeInAir > 5.0f) {
			musicManager.SetMusicState (3);
		}
			
			
	}


	public void PublicGeneration(float density, List<GameObject> prefabs)
	{
		int minx = -5; int maxx = 5;
		int miny = 0; int maxy = 10;
		for (float x = minx; x < maxx; x += density)
		{
			for (float y = miny; y < maxy; y += density)
			{
				Vector3 position = peopleStart.transform.position + Vector3.right * x + Vector3.forward * y;

				GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
				GameObject newPeople = (GameObject)Instantiate (prefab, position, Quaternion.identity);
				newPeople.transform.parent = peopleStart;

				for (int i = 0; i < newPeople.transform.FindChild ("GameObject").GetChild (0).GetComponent<Renderer> ().materials.Length; i++) 
				{
					newPeople.transform.FindChild ("GameObject").GetChild (0).GetComponent<Renderer> ().materials [i].color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
				}

				newPeople.GetComponent<PublicBehaviour> ().ragdoll = ragdollChest.transform;
			}
		}
	}

	public GameObject camera;

	public void GameOver()
	{
		if (!gameOver) {
			camera.GetComponent<Grayscale> ().enabled = true;
			gameOver = true;
			musicManager.StopMusic ();
			StartCoroutine (WaitAndReloadLevel (2.0f));
		}
	}

	IEnumerator WaitAndReloadLevel(float timer)
	{
		yield return new WaitForSeconds (timer);

		SceneManager.LoadScene ("scene1");
	}
}
