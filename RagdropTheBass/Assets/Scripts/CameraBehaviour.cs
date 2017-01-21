using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour 
{

	public Transform target;

	public Vector3 offset;

	public float maxSpeed;
	public float speed;

	public float maxDistance;
	public float minDistance;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float height = offset.y;
		Vector3 fixedHeightNewCameraPosition = target.position + offset ;
		fixedHeightNewCameraPosition = new Vector3 (fixedHeightNewCameraPosition.x, height, fixedHeightNewCameraPosition.z);

		float cameraDistance = Vector3.Distance (fixedHeightNewCameraPosition, this.transform.position);
		Vector3 cameraDirectionUnit = (fixedHeightNewCameraPosition - this.transform.position).normalized;

		if (cameraDistance > minDistance) 
		{
			if (cameraDistance > maxDistance) 
			{
				this.transform.position += cameraDirectionUnit * maxSpeed;
			}
			else
			if (cameraDistance > speed) 
			{
				this.transform.position += cameraDirectionUnit * speed;
			}
			else 
			{
				this.transform.position = fixedHeightNewCameraPosition;
			}
		}
	}
}
