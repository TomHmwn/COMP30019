using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

	public LayerMask collisionMask;
	public Color trailColour;


	float speed = 10;
	[SerializeField] float damage = 1;

	float lifetime = 3;

	// to manage if enemies move faster than the update from
	// increase it to compensate faster enemies
	float distfromSkin = 0.1f;
	void Start()
	{
		Destroy(gameObject, lifetime);

		Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, collisionMask);
		if (initialCollisions.Length > 0)
		{
			OnHitObject(initialCollisions[0], transform.position);
		}
		GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColour);


	}
	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
	}

	void Update()
	{
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions(moveDistance);
		transform.Translate(Vector3.forward * moveDistance);

	}


	void CheckCollisions(float moveDistance)
	{
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, moveDistance + distfromSkin, collisionMask, QueryTriggerInteraction.Collide))
		{
			OnHitObject(hit.collider, hit.point);
		}
	}

	void OnHitObject(Collider hit, Vector3 hitPoint)
	{
		IDamagable damagableObject = hit.GetComponent<IDamagable>();
		if (damagableObject != null)
		{
			damagableObject.TakeHit(damage, hitPoint, transform.forward);
		}
		GameObject.Destroy(gameObject);
	}
}
