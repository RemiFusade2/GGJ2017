using UnityEngine;
using System.Collections;

public class PublicBehaviour : MonoBehaviour {

	public float force;

	public Transform arms;

	public Vector2 destOffset;

	public Transform ragdoll;

	public float lastJumpTime;

	private Vector3 initialPosition;
	public float radiusAroundPosition;

	private Vector3 destination;

	// Use this for initialization
	void Start () 
	{
		destOffset = Random.insideUnitCircle;
		lastJumpTime = 0;
		initialPosition = this.transform.position;
	}

	public void ResolveLoudInput(float loudness)
	{
		Ray ray = new Ray (this.transform.position, -Vector3.up);
		RaycastHit raycashInfo;
		if (Physics.Raycast(ray, out raycashInfo, 1.0f) && raycashInfo.collider.tag.Equals("Ground") && (Time.time - lastJumpTime) > 0.5f ) 
		{		
			lastJumpTime = Time.time;
			float dist = Vector3.Distance (this.transform.position, ragdoll.position) *0.1f;
			float timer = Random.Range (dist-0.2f, dist-0.2f);
			if (timer < 0.1f) 
			{
				timer = 0.1f;
			}
			StartCoroutine (WaitAndMovePeople (this.GetComponent<Rigidbody> (), loudness, timer));
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			Ray ray = new Ray (this.transform.position, -Vector3.up);
			RaycastHit raycashInfo;
			if (Physics.Raycast(ray, out raycashInfo, 1.0f) && raycashInfo.collider.tag.Equals("Ground") && (Time.time - lastJumpTime) > 0.5f ) 
			{		
				float dist = Vector3.Distance (this.transform.position, ragdoll.position) *0.1f;
				float r = Random.Range (dist-0.2f, dist-0.2f);
				if (r < 0.1f) 
				{
					r = 0.1f;
				}
				StartCoroutine (WaitAndMovePeople (this.GetComponent<Rigidbody> (), r));
				lastJumpTime = Time.time;
			}
		}
		*/


		Vector3 ragdollPositionNoHeight = new Vector3 (ragdoll.transform.position.x, this.transform.position.y, ragdoll.transform.position.z);
		float distanceWithRagdoll = Vector3.Distance (this.transform.position, ragdollPositionNoHeight);
		if (distanceWithRagdoll < 5.0f)
		{
			destination = ragdollPositionNoHeight;
		} 
		else 
		{
			destination = initialPosition;
		}

		float distanceWithDestination = Vector3.Distance (destination, this.transform.position);
		float distanceFromInitialPosition = Vector3.Distance (initialPosition, this.transform.position);
		if (distanceWithDestination > 0.2f && distanceFromInitialPosition < radiusAroundPosition) 
		{
			this.transform.position += (destination - this.transform.position).normalized * 0.02f;
		}
	}

	IEnumerator WaitAndMovePeople(Rigidbody people, float loudness, float timer)
	{
		yield return new WaitForSeconds (timer);

		this.GetComponent<Rigidbody> ().AddForce (Vector3.up * force * loudness);
	}
}
