using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserController : MonoBehaviour
{
	public Transform targetTransform;
	public float speed = 8.0f;

	// Update is called once per frame
	void Update()
	{
		Vector3 displacementFromTarget = targetTransform.position - transform.position;
		Vector3 directionToTarget = displacementFromTarget.normalized;
		Vector3 velocity = directionToTarget * speed;

		float distanceToTarget = displacementFromTarget.magnitude;

		if (distanceToTarget > 2.0f)
		{
			transform.Translate(velocity * Time.deltaTime);
		}
	}
}
