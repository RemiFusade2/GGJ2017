using UnityEngine;
using System.Collections;

public class PublicBehaviour : MonoBehaviour {

	public float force;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			foreach (Transform child in this.transform) 
			{
				float r = Random.Range (0, 0.1f);
				StartCoroutine (WaitAndMovePeople (child.GetComponent<Rigidbody> (), r));
			}

		}
	}

	IEnumerator WaitAndMovePeople(Rigidbody people, float timer)
	{
		yield return new WaitForSeconds (timer);

		people.AddForce (Vector3.up * force);
	}
}
