using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public float speedFollow = 1;
	Vector3 startingOffest;
	public Vector3 offset;
	public float minYAngle = 30.0f;
	public float maxYAngle = 60.0f;
	public float minZoom = 5.0f;
	public float maxZoom = 40.0f;
	public float zoomSpeed = 1f;

	public float rotateSpeed = 0.1f;
	void Start(){
		startingOffest = offset;
	}
	void FixedUpdate()
	{
		if (target != null)
		{
			Vector3 trackingPosition = target.position + offset;
			Vector3 smoothedPosition = Vector3.Lerp(transform.position, trackingPosition, speedFollow);
			transform.position = smoothedPosition;

			transform.LookAt(target);
		}
	}
	void Update()
	{
		float horizontal = Input.GetAxis("Mouse X") * rotateSpeed * 360;
		float vertical = Input.GetAxis("Mouse Y") * rotateSpeed * 360;

		if (Input.GetMouseButton(1))
		{
			offset = Quaternion.AngleAxis(horizontal, Vector3.up) * offset;
			Vector3 perp = Vector3.Cross(Vector3.up, offset);

			float angle = Vector3.SignedAngle(Vector3.up, offset, perp);
			if (angle + vertical < minYAngle)
			{
				vertical = minYAngle - angle;
			}
			if (angle + vertical > maxYAngle)
			{
				vertical = maxYAngle - angle;
			}

			offset = Quaternion.AngleAxis(vertical, perp) * offset;
		}
		if (Input.mouseScrollDelta.y != 0)
		{
			float dist = offset.magnitude;
			float scale = (-Input.mouseScrollDelta.y) * zoomSpeed;
			if (dist + scale < minZoom)
			{
				scale = minZoom - dist;
			}
			if (dist + scale > maxZoom)
			{
				scale = maxZoom - dist;
			}

			offset = offset.normalized * (dist + scale);
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			offset = startingOffest;
		}
	}
}
