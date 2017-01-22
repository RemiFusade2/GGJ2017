using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightEngineBehaviour : MonoBehaviour {

	public Transform ragdoll;

	public Light spotLight;

	public List<Light> colorLights;

	// Use this for initialization
	void Start () 
	{
		//StartCoroutine(WaitAndRandomizeLights(5.0f));
	}
	
	// Update is called once per frame
	void Update () 
	{
		spotLight.transform.LookAt (ragdoll.position);
	}

	IEnumerator WaitAndRandomizeLights(float timer)
	{
		yield return new WaitForSeconds (timer);

		foreach (Light light in colorLights) 
		{
			light.intensity = Random.Range (0.5f, 0.8f);
			light.color = new Color (Random.Range (0.5f, 1.0f), Random.Range (0.5f, 1.0f), Random.Range (0.5f, 1.0f));

			StartCoroutine (WaitAndRandomizeLights (timer));
		}
	}
}
