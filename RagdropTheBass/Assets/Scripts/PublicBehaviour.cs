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

	private float likeSinger; // between -1 and +1

	public Animator animator;

	public float countdown; //delay before crowd starts to leave

	void restartCountdown ()
	{
		countdown = 4f; //CD in seconds
	}

	// Use this for initialization
	void Start () 
	{
		destOffset = Random.insideUnitCircle;
		lastJumpTime = 0;
		initialPosition = this.transform.position;

		likeSinger = Random.Range (-0.1f, 0.5f);

		restartCountdown ();
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
		restartCountdown ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		countdown -= Time.deltaTime; //counting down

		float ratioFrame = 0.07f;

		Vector2 my2DPosition = new Vector2 (this.transform.position.x, this.transform.position.z);


		// Find ragdoll singer and moves towards it
		Vector2 moveTowardRagdollSinger = Vector2.zero;

		Vector2 ragdollSinger2DPosition = new Vector2 (ragdoll.transform.position.x, ragdoll.transform.position.z);
		float distanceWithSinger = Vector2.Distance (my2DPosition, ragdollSinger2DPosition);
		moveTowardRagdollSinger = (ragdollSinger2DPosition - my2DPosition).normalized * likeSinger;


		// Find closer person in crowd and move towards it
		Vector2 moveTowardRandomPerson = Vector2.zero;

		if (distanceWithSinger < 10.0f * likeSinger) 
		{
			float minDistance = float.MaxValue;
			Vector3 targetPosition = Vector3.zero;
			for (float r = 0; r < Mathf.PI * 2; r += Mathf.PI / 6.0f) 
			{
				Vector2 rayDir = new Vector2 (Mathf.Cos (r), Mathf.Sin (r));
				Ray ray = new Ray (this.transform.position, new Vector3 (this.transform.position.x + rayDir.x, 0, this.transform.position.z + rayDir.y));
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 5.0f) && hit.collider.tag.Equals ("People")) 
				{
					float dist = Vector3.Distance (hit.point, this.transform.position);
					if (dist < minDistance)
					{
						minDistance = dist;
						targetPosition = hit.point;
					}
				}
			}
			if (minDistance < 0)
			{
				// don't move at all
				moveTowardRagdollSinger = Vector2.zero;
				moveTowardRandomPerson = Vector2.zero;
			}
			if (minDistance > 1f && minDistance <= 10.0f) 
			{
				moveTowardRandomPerson = new Vector2 ((targetPosition - this.transform.position).normalized.x, (targetPosition - this.transform.position).normalized.z);
			}
		}


		Vector2 moveVec = Vector2.zero; //moved definition up here
		if (countdown >= 0f) {
			//normal behaviour
			moveVec = (moveTowardRandomPerson + moveTowardRagdollSinger);
		} else {
			//going away
			moveVec = (-moveTowardRandomPerson - moveTowardRagdollSinger);
			animator.SetInteger ("animIndex", -1);
		}

		this.transform.position += ratioFrame * new Vector3 (moveVec.x, 0, moveVec.y);


		/*

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

		*/
	}

	IEnumerator WaitAndMovePeople(Rigidbody people, float loudness, float timer)
	{
		yield return new WaitForSeconds (timer);

		this.GetComponent<Rigidbody> ().AddForce (Vector3.up * force * loudness);
	}


	public void SetAnim(int animIndex)
	{
		animator.SetInteger ("animIndex", animIndex);
	}
}
