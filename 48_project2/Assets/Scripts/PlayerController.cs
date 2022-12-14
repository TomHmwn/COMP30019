using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

	Vector3 velocity;
	Rigidbody myRigidbody;
	void Start()
	{
		myRigidbody = GetComponent<Rigidbody>();
	}

	public void Move(Vector3 _velocity)
	{
		velocity = _velocity;

	}

	public void LookAt(Vector3 lookPoint)
	{

		Vector3 noRotateLook = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
		transform.LookAt(noRotateLook);
	}

	void FixedUpdate()
	{
		// transform.position = transform.position + velocity * Time.fixedDeltaTime;
		myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);

	}

}
