using UnityEngine;
using System.Collections;

public class RagdollBehaviour : MonoBehaviour {

	public Rigidbody rigidbody;

	public float ragdollForce;
	public float ragdollTimer;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void PushRagdoll (float force, Vector2 direction)
	{
		bool canBePushed = false;

		for (float x = -1.0f; x < 1.0f; x += 0.1f) 
		{
			for (float z = -1.0f; z < 1.0f; z += 0.1f) 
			{	
				Ray ray = new Ray (rigidbody.transform.position + Vector3.right * x + Vector3.forward * z, -Vector3.up);
				RaycastHit raycastInfo;
				if (Physics.Raycast (ray, out raycastInfo, 1.0f) && raycastInfo.collider.tag.Equals ("People")) 
				{
					Debug.DrawRay (ray.origin, ray.direction, Color.red, 1.0f);
					canBePushed = true;
					break;
				}
			}
		}

		if (canBePushed)
		{
			StartCoroutine (WaitAndPushRagdoll (force * ragdollForce, direction, 0.01f));
		}
	}

	IEnumerator WaitAndPushRagdoll (float force, Vector2 direction, float timer)
	{
		yield return new WaitForSeconds (timer);
	
		rigidbody.AddForce (force * (Vector3.up + Vector3.right * direction.x + Vector3.forward * direction.y), ForceMode.Impulse);
	}
}
