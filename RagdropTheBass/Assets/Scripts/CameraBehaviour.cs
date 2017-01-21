using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour 
{

	public Transform target;

	public Vector3 offset;

	public float speed;

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

		Vector3 cameraDirectionUnit = (fixedHeightNewCameraPosition - this.transform.position).normalized;

		if ((fixedHeightNewCameraPosition - this.transform.position).magnitude > minDistance) 
		{
			if ((fixedHeightNewCameraPosition - this.transform.position).magnitude > speed) 
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
